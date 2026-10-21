
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Models.DTO;

namespace QuanLyBaoHiem.BLL
{
    public class NguoiDung
    {
        private readonly NguoiDungData _data;
        private readonly IConfiguration _config;

        public NguoiDung(
            NguoiDungData data,
            IConfiguration config)
        {
            _data = data;
            _config = config;
        }

        // Đăng ký tài khoản
        public string DangKy(DangKyRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.TenDangNhap) ||
                string.IsNullOrWhiteSpace(request.MatKhau) ||
                string.IsNullOrWhiteSpace(request.Email))
                throw new Exception("Vui lòng nhập đầy đủ thông tin.");

            string tenDangNhap = request.TenDangNhap.Trim();
            string email = request.Email.Trim();

            if (request.MatKhau.Length < 8)
                throw new Exception("Mật khẩu phải có ít nhất 8 ký tự.");

            if (_data.TimTaiKhoan(tenDangNhap) != null)
                throw new Exception("Tên đăng nhập đã tồn tại.");

            if (_data.EmailTonTai(email))
                throw new Exception("Email đã được sử dụng.");

            var quyen = _data.TimQuyen("Khách hàng");

            if (quyen == null)
                throw new Exception("Không tìm thấy quyền Khách hàng.");

            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = tenDangNhap,
                Email = email,
                MatKhauHash =
                    BCrypt.Net.BCrypt.HashPassword(request.MatKhau),
                MaQuyen = quyen.MaQuyen,
                KichHoat = true,
                DaXacMinhEmail = false,
                SoLanDangNhapSai = 0,
                IsDeleted = false,
                NgayTao = DateTime.Now
            };

            _data.ThemTaiKhoan(taiKhoan);
            return "Đăng ký tài khoản thành công.";
        }

        // Tạo JWT
        private string TaoJwt(TaiKhoan tk, Quyen quyen)
        {
            string jwtKey = _config["Jwt:Key"]
                ?? throw new Exception("Thiếu cấu hình Jwt:Key.");

            string issuer = _config["Jwt:Issuer"]
                ?? throw new Exception("Thiếu cấu hình Jwt:Issuer.");

            string audience = _config["Jwt:Audience"]
                ?? throw new Exception("Thiếu cấu hình Jwt:Audience.");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, tk.MaTk.ToString()),
                new Claim(ClaimTypes.Name, tk.TenDangNhap),
                new Claim(ClaimTypes.Role, quyen.TenQuyen)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        // Sinh chuỗi token ngẫu nhiên
        private string TaoTokenNgauNhien()
        {
            return Convert.ToBase64String(
                RandomNumberGenerator.GetBytes(48))
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        // Đăng nhập
        public object DangNhap(
            DangNhapRequest request,
            string? diaChiIp = null,
            string? thietBi = null)
        {
            if (string.IsNullOrWhiteSpace(request.TenDangNhap) ||
                string.IsNullOrWhiteSpace(request.MatKhau))
                throw new Exception("Vui lòng nhập tên đăng nhập và mật khẩu.");

            var tk = _data.TimTaiKhoan(request.TenDangNhap.Trim());

            if (tk == null ||
                !BCrypt.Net.BCrypt.Verify(request.MatKhau, tk.MatKhauHash))
                throw new Exception("Tên đăng nhập hoặc mật khẩu không đúng.");

            if (!tk.KichHoat)
                throw new Exception("Tài khoản chưa được kích hoạt.");

            var quyen = _data.TimQuyenTheoMa(tk.MaQuyen);

            if (quyen == null)
                throw new Exception("Tài khoản chưa được gán quyền.");

            string refreshToken = TaoTokenNgauNhien();

            var refresh = new RefreshToken
            {
                MaTk = tk.MaTk,
                Token = refreshToken,
                NgayTao = DateTime.Now,
                NgayHetHan = DateTime.Now.AddDays(7),
                DaThuHoi = false,
                DiaChiIp = diaChiIp,
                ThietBi = thietBi
            };

            _data.ThemRefreshToken(refresh);

            return new
            {
                thongBao = "Đăng nhập thành công.",
                maTK = tk.MaTk,
                tenDangNhap = tk.TenDangNhap,
                quyen = quyen.TenQuyen,
                token = TaoJwt(tk, quyen),
                refreshToken = refreshToken,
                refreshTokenHetHan = refresh.NgayHetHan
            };
        }

        // Làm mới JWT và thay refresh token cũ bằng token mới
        public object LamMoiToken(string tokenCu, string? diaChiIp = null,
            string? thietBi = null)
        {
            if (string.IsNullOrWhiteSpace(tokenCu))
                throw new Exception("Vui lòng nhập refresh token.");

            var refresh = _data.TimRefreshToken(tokenCu);

            if (refresh == null || refresh.DaThuHoi ||
                refresh.NgayHetHan <= DateTime.Now)
                throw new Exception("Refresh token không hợp lệ hoặc đã hết hạn.");

            var tk = _data.TimTaiKhoanTheoMa(refresh.MaTk);

            if (tk == null || !tk.KichHoat)
                throw new Exception("Tài khoản không hợp lệ hoặc đã bị khóa.");

            var quyen = _data.TimQuyenTheoMa(tk.MaQuyen);

            if (quyen == null)
                throw new Exception("Tài khoản chưa được gán quyền.");

            // Thu hồi token cũ
            refresh.DaThuHoi = true;
            _data.CapNhatRefreshToken(refresh);

            // Tạo token mới
            string tokenMoi = TaoTokenNgauNhien();

            var refreshMoi = new RefreshToken
            {
                MaTk = tk.MaTk,
                Token = tokenMoi,
                NgayTao = DateTime.Now,
                NgayHetHan = DateTime.Now.AddDays(7),
                DaThuHoi = false,
                DiaChiIp = diaChiIp,
                ThietBi = thietBi
            };

            _data.ThemRefreshToken(refreshMoi);

            return new
            {
                token = TaoJwt(tk, quyen),
                refreshToken = tokenMoi,
                refreshTokenHetHan = refreshMoi.NgayHetHan
            };
        }

        // Thu hồi refresh token
        public string ThuHoiToken(string token)
        {
            var refresh = _data.TimRefreshToken(token);

            if (refresh == null || refresh.DaThuHoi)
                throw new Exception("Refresh token không hợp lệ.");

            refresh.DaThuHoi = true;
            _data.CapNhatRefreshToken(refresh);

            return "Đã thu hồi refresh token.";
        }

        // Tạo token đặt lại mật khẩu
        // Chỉ trả token trực tiếp để kiểm thử đồ án.
        public object QuenMatKhau(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Vui lòng nhập email.");

            var tk = _data.TimTaiKhoanTheoEmail(email.Trim());

            // Không tiết lộ email có tồn tại hay không.
            if (tk == null)
            {
                return new
                {
                    thongBao = "Nếu email hợp lệ, yêu cầu đặt lại mật khẩu đã được tiếp nhận."
                };
            }

            string token = TaoTokenNgauNhien();

            var reset = new PasswordResetToken
            {
                MaTk = tk.MaTk,
                Token = token,
                NgayTao = DateTime.Now,
                NgayHetHan = DateTime.Now.AddMinutes(15),
                DaSuDung = false
            };

            _data.ThemPasswordResetToken(reset);

            return new
            {
                thongBao = "Đã tạo token đặt lại mật khẩu. Token chỉ hiển thị phục vụ kiểm thử.",
                token = token,
                hetHan = reset.NgayHetHan
            };
        }

        // Đặt lại mật khẩu
        public string DatLaiMatKhau(string token, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Vui lòng nhập token.");

            if (string.IsNullOrWhiteSpace(matKhauMoi) ||
                matKhauMoi.Length < 8)
                throw new Exception("Mật khẩu mới phải có ít nhất 8 ký tự.");

            var reset = _data.TimPasswordResetToken(token);

            if (reset == null || reset.DaSuDung ||
                reset.NgayHetHan <= DateTime.Now)
                throw new Exception("Token không hợp lệ hoặc đã hết hạn.");

            var tk = _data.TimTaiKhoanTheoMa(reset.MaTk);

            if (tk == null || !tk.KichHoat)
                throw new Exception("Tài khoản không hợp lệ hoặc đã bị khóa.");

            tk.MatKhauHash =
                BCrypt.Net.BCrypt.HashPassword(matKhauMoi);

            _data.CapNhatTaiKhoan(tk);

            reset.DaSuDung = true;
            _data.CapNhatPasswordResetToken(reset);

            return "Đặt lại mật khẩu thành công.";
        }
    }
}
