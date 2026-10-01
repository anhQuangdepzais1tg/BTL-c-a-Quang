using Microsoft.IdentityModel.Tokens;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuanLyBaoHiem.Services
{
    public class AuthService
    {
        private readonly AuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(
            AuthRepository repository,
            IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        // ĐĂNG KÝ
        public string Register(RegisterRequest request)
        {
            // Kiểm tra tên đăng nhập
            if (_repository.UsernameExists(request.TenDangNhap))
            {
                throw new Exception("Tên đăng nhập đã tồn tại");
            }

            // Kiểm tra email
            if (_repository.EmailExists(request.Email))
            {
                throw new Exception("Email đã tồn tại");
            }

            // Tạo tài khoản
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = request.TenDangNhap,

                // Mã hóa mật khẩu
                MatKhauHash = BCrypt.Net.BCrypt.HashPassword(request.MatKhau),

                Email = request.Email,

                // 3 = Khách hàng
                MaQuyen = 3,

                SoLanDangNhapSai = 0,
                Bat2FA = false,
                KichHoat = true,
                DaXacMinhEmail = false,
                IsDeleted = false,
                NgayTao = DateTime.Now
            };

            _repository.Add(taiKhoan);

            return "Đăng ký tài khoản thành công";
        }

        // ĐĂNG NHẬP
        public LoginResponse Login(LoginRequest request)
        {
            var taiKhoan =
                _repository.GetByUsername(request.TenDangNhap);

            if (taiKhoan == null)
            {
                throw new Exception(
                    "Tên đăng nhập hoặc mật khẩu không đúng");
            }

            // Kiểm tra tài khoản bị khóa
            if (taiKhoan.KhoaDenNgay.HasValue &&
                taiKhoan.KhoaDenNgay.Value > DateTime.Now)
            {
                throw new Exception("Tài khoản đang bị khóa");
            }

            // Kiểm tra mật khẩu
            bool dungMatKhau =
                BCrypt.Net.BCrypt.Verify(
                    request.MatKhau,
                    taiKhoan.MatKhauHash);

            if (!dungMatKhau)
            {
                _repository.LoginFail(taiKhoan);

                throw new Exception(
                    "Tên đăng nhập hoặc mật khẩu không đúng");
            }

            // Kiểm tra tài khoản hoạt động
            if (!taiKhoan.KichHoat)
            {
                throw new Exception(
                    "Tài khoản chưa được kích hoạt");
            }

            var quyen =
                _repository.GetQuyen(taiKhoan.MaQuyen);

            if (quyen == null)
            {
                throw new Exception(
                    "Không tìm thấy quyền tài khoản");
            }

            // Ghi nhận đăng nhập thành công
            _repository.LoginSuccess(taiKhoan);

            // Tạo JWT
            string token =
                CreateToken(taiKhoan, quyen);

            return new LoginResponse
            {
                Token = token,
                MaTK = taiKhoan.MaTK,
                TenDangNhap = taiKhoan.TenDangNhap,
                Email = taiKhoan.Email,
                MaQuyen = taiKhoan.MaQuyen,
                TenQuyen = quyen.TenQuyen
            };
        }

        // Tạo JWT
        private string CreateToken(
            TaiKhoan taiKhoan,
            Quyen quyen)
        {
            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    taiKhoan.MaTK.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    taiKhoan.TenDangNhap),

                new Claim(
                    ClaimTypes.Email,
                    taiKhoan.Email),

                new Claim(
                    ClaimTypes.Role,
                    quyen.TenQuyen)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}