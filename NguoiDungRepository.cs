using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class NguoiDungRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public NguoiDungRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // =====================================================
        // TÀI KHOẢN
        // =====================================================

        // Tìm tài khoản theo tên đăng nhập
        public TaiKhoan? GetByUsername(string tenDangNhap)
        {
            return _context.TaiKhoan
                .FirstOrDefault(x =>
                    x.TenDangNhap == tenDangNhap &&
                    !x.IsDeleted);
        }

        // Tìm tài khoản theo mã
        public TaiKhoan? GetById(int maTK)
        {
            return _context.TaiKhoan
                .FirstOrDefault(x =>
                    x.MaTK == maTK &&
                    !x.IsDeleted);
        }

        // Tìm tài khoản theo email
        public TaiKhoan? GetByEmail(string email)
        {
            return _context.TaiKhoan
                .FirstOrDefault(x =>
                    x.Email == email &&
                    !x.IsDeleted);
        }

        // Kiểm tra tên đăng nhập
        public bool UsernameExists(string tenDangNhap)
        {
            return _context.TaiKhoan.Any(x =>
                x.TenDangNhap == tenDangNhap &&
                !x.IsDeleted);
        }

        // Kiểm tra email
        public bool EmailExists(string email)
        {
            return _context.TaiKhoan.Any(x =>
                x.Email == email &&
                !x.IsDeleted);
        }

        // Thêm tài khoản
        public void Add(TaiKhoan user)
        {
            _context.TaiKhoan.Add(user);
            _context.SaveChanges();
        }

        // =====================================================
        // ĐĂNG NHẬP
        // =====================================================

        // Đăng nhập sai
        public void LoginFail(TaiKhoan user)
        {
            user.SoLanDangNhapSai++;

            _context.SaveChanges();
        }

        // Đăng nhập thành công
        public void LoginSuccess(
            TaiKhoan user,
            string? ip)
        {
            user.SoLanDangNhapSai = 0;

            _context.SaveChanges();
        }

        // =====================================================
        // QUYỀN
        // =====================================================

        public Quyen? GetQuyen(int maQuyen)
        {
            return _context.Quyen
                .FirstOrDefault(x =>
                    x.MaQuyen == maQuyen &&
                    !x.IsDeleted);
        }

        // =====================================================
        // REFRESH TOKEN
        // =====================================================

        // Thêm Refresh Token
        public void AddRefreshToken(
            RefreshToken refreshToken)
        {
            _context.RefreshToken.Add(refreshToken);

            _context.SaveChanges();
        }

        // Tìm Refresh Token
        public RefreshToken? GetRefreshToken(
            string token)
        {
            return _context.RefreshToken
                .FirstOrDefault(x =>
                    x.Token == token &&
                    !x.DaThuHoi &&
                    x.NgayHetHan > DateTime.Now);
        }

        // Thu hồi Refresh Token
        public void RevokeRefreshToken(
            RefreshToken token)
        {
            token.DaThuHoi = true;

            _context.SaveChanges();
        }

        // Thu hồi tất cả Refresh Token
        public void RevokeAllRefreshTokens(
            int maTK)
        {
            var tokens = _context.RefreshToken
                .Where(x =>
                    x.MaTK == maTK &&
                    !x.DaThuHoi)
                .ToList();

            foreach (var token in tokens)
            {
                token.DaThuHoi = true;
            }

            _context.SaveChanges();
        }

        // =====================================================
        // PASSWORD RESET
        // =====================================================

        public void AddResetToken(
            PasswordResetToken token)
        {
            _context.PasswordResetToken.Add(token);

            _context.SaveChanges();
        }

        public PasswordResetToken? GetResetToken(
            string token)
        {
            return _context.PasswordResetToken
                .FirstOrDefault(x =>
                    x.Token == token &&
                    !x.DaSuDung &&
                    x.NgayHetHan > DateTime.Now);
        }

        // =====================================================
        // LƯU
        // =====================================================

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}