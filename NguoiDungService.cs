using Microsoft.IdentityModel.Tokens;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBaoHiem.Services
{
    public class NguoiDungService
    {
        private readonly NguoiDungRepository _repo;
        private readonly IConfiguration _config;

        public NguoiDungService(
            NguoiDungRepository repo,
            IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // =====================================================
        // ĐĂNG KÝ
        // =====================================================

        public string Register(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống");

            if (request.TenDangNhap.Length < 3)
                throw new Exception("Tên đăng nhập tối thiểu 3 ký tự");

            if (string.IsNullOrWhiteSpace(request.MatKhau))
                throw new Exception("Mật khẩu không được để trống");

            if (request.MatKhau.Length < 6)
                throw new Exception("Mật khẩu tối thiểu 6 ký tự");

            if (_repo.UsernameExists(request.TenDangNhap))
                throw new Exception("Tên đăng nhập đã tồn tại");

            if (_repo.EmailExists(request.Email))
                throw new Exception("Email đã tồn tại");

            var user = new TaiKhoan
            {
                TenDangNhap = request.TenDangNhap.Trim(),

                MatKhauHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        request.MatKhau),

                Email = request.Email.Trim(),

                // 3 = User
                MaQuyen = 3,

                SoLanDangNhapSai = 0,

                Bat2FA = false,

                KichHoat = true,

                DaXacMinhEmail = false,

                IsDeleted = false,

                NgayTao = DateTime.Now
            };

            _repo.Add(user);

            return "Đăng ký tài khoản thành công";
        }

        // =====================================================
        // ĐĂNG NHẬP
        // =====================================================

        public LoginResponse Login(
            LoginRequest request,
            string? ip = null)
        {
            var user =
                _repo.GetByUsername(
                    request.TenDangNhap);

            if (user == null)
                throw new Exception(
                    "Tên đăng nhập hoặc mật khẩu không đúng");

            bool dungMatKhau =
                BCrypt.Net.BCrypt.Verify(
                    request.MatKhau,
                    user.MatKhauHash);

            if (!dungMatKhau)
            {
                _repo.LoginFail(user);

                throw new Exception(
                    "Tên đăng nhập hoặc mật khẩu không đúng");
            }

            if (user.KhoaDenNgay.HasValue &&
                user.KhoaDenNgay > DateTime.Now)
            {
                throw new Exception(
                    "Tài khoản đang bị khóa");
            }

            if (!user.KichHoat)
            {
                throw new Exception(
                    "Tài khoản chưa được kích hoạt");
            }

            var quyen =
                _repo.GetQuyen(user.MaQuyen);

            if (quyen == null)
                throw new Exception(
                    "Không tìm thấy quyền tài khoản");

            // Đăng nhập thành công
            _repo.LoginSuccess(user, ip);

            // Tạo Refresh Token
            var refreshToken =
                Convert.ToBase64String(
                    RandomNumberGenerator.GetBytes(48));

            var refresh = new RefreshToken
            {
                MaTK = user.MaTK,

                Token = refreshToken,

                NgayTao = DateTime.Now,

                NgayHetHan =
                    DateTime.Now.AddDays(7),

                DaThuHoi = false,

                IpAddress = ip,

                UserAgent = null
            };

            // Lưu Refresh Token
            _repo.AddRefreshToken(refresh);

            return new LoginResponse
            {
                Token =
                    CreateToken(
                        user,
                        quyen),

                RefreshToken =
                    refreshToken,

                MaTK =
                    user.MaTK,

                TenDangNhap =
                    user.TenDangNhap,

                Email =
                    user.Email,

                MaQuyen =
                    user.MaQuyen,

                TenQuyen =
                    quyen.TenQuyen
            };
        }

        // =====================================================
        // REFRESH TOKEN
        // =====================================================

        public LoginResponse Refresh(
            string refreshToken)
        {
            var oldToken =
                _repo.GetRefreshToken(
                    refreshToken);

            if (oldToken == null)
                throw new Exception(
                    "Refresh token không hợp lệ hoặc đã hết hạn");

            var user =
                _repo.GetById(oldToken.MaTK);

            if (user == null)
                throw new Exception(
                    "Tài khoản không tồn tại");

            var quyen =
                _repo.GetQuyen(user.MaQuyen);

            if (quyen == null)
                throw new Exception(
                    "Không tìm thấy quyền");

            // Thu hồi token cũ
            _repo.RevokeRefreshToken(
                oldToken);

            // Tạo token mới
            var newRefreshToken =
                Convert.ToBase64String(
                    RandomNumberGenerator.GetBytes(48));

            _repo.AddRefreshToken(
                new RefreshToken
                {
                    MaTK = user.MaTK,

                    Token = newRefreshToken,

                    NgayTao = DateTime.Now,

                    NgayHetHan =
                        DateTime.Now.AddDays(7),

                    DaThuHoi = false
                });

            return new LoginResponse
            {
                Token =
                    CreateToken(
                        user,
                        quyen),

                RefreshToken =
                    newRefreshToken,

                MaTK =
                    user.MaTK,

                TenDangNhap =
                    user.TenDangNhap,

                Email =
                    user.Email,

                MaQuyen =
                    user.MaQuyen,

                TenQuyen =
                    quyen.TenQuyen
            };
        }

        // =====================================================
        // ĐĂNG XUẤT
        // =====================================================

        public void Logout(int maTK)
        {
            _repo.RevokeAllRefreshTokens(maTK);
        }

        // =====================================================
        // ĐỔI MẬT KHẨU
        // =====================================================

        public void ChangePassword(
            int maTK,
            ChangePasswordRequest request)
        {
            var user =
                _repo.GetById(maTK);

            if (user == null)
                throw new Exception(
                    "Tài khoản không tồn tại");

            bool dungMatKhau =
                BCrypt.Net.BCrypt.Verify(
                    request.MatKhauCu,
                    user.MatKhauHash);

            if (!dungMatKhau)
                throw new Exception(
                    "Mật khẩu cũ không đúng");

            if (string.IsNullOrWhiteSpace(
                request.MatKhauMoi))
            {
                throw new Exception(
                    "Mật khẩu mới không được để trống");
            }

            if (request.MatKhauMoi.Length < 6)
                throw new Exception(
                    "Mật khẩu mới tối thiểu 6 ký tự");

            user.MatKhauHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.MatKhauMoi);

            _repo.Save();
        }

        // =====================================================
        // QUÊN MẬT KHẨU
        // =====================================================

        public string ForgotPassword(
            string email)
        {
            var user =
                _repo.GetByEmail(email);

            if (user == null)
                throw new Exception(
                    "Email không tồn tại");

            var token =
                Convert.ToBase64String(
                    RandomNumberGenerator.GetBytes(32));

            _repo.AddResetToken(
                new PasswordResetToken
                {
                    MaTK = user.MaTK,

                    Token = token,

                    NgayTao = DateTime.Now,

                    NgayHetHan =
                        DateTime.Now.AddMinutes(15),

                    DaSuDung = false
                });

            // Tạm thời trả token để test Swagger
            return token;
        }

        // =====================================================
        // RESET MẬT KHẨU
        // =====================================================

        public void ResetPassword(
            ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(
                request.MatKhauMoi))
            {
                throw new Exception(
                    "Mật khẩu mới không được để trống");
            }

            if (request.MatKhauMoi.Length < 6)
                throw new Exception(
                    "Mật khẩu mới tối thiểu 6 ký tự");

            var resetToken =
                _repo.GetResetToken(
                    request.Token);

            if (resetToken == null)
                throw new Exception(
                    "Token đặt lại mật khẩu không hợp lệ hoặc đã hết hạn");

            var user =
                _repo.GetById(
                    resetToken.MaTK);

            if (user == null)
                throw new Exception(
                    "Tài khoản không tồn tại");

            user.MatKhauHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.MatKhauMoi);

            resetToken.DaSuDung = true;

            _repo.Save();
        }

        // =====================================================
        // TẠO JWT
        // =====================================================

        private string CreateToken(
            TaiKhoan user,
            Quyen quyen)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.MaTK.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.TenDangNhap),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    quyen.TenQuyen)
            };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _config["Jwt:Key"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _config["Jwt:Issuer"],

                    audience:
                        _config["Jwt:Audience"],

                    claims:
                        claims,

                    expires:
                        DateTime.Now.AddHours(2),

                    signingCredentials:
                        credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}