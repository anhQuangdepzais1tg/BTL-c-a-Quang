
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class NguoiDungData
    {
        private readonly QuanLyBaoHiemContext _db;

        public NguoiDungData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // Tìm tài khoản theo tên đăng nhập
        public TaiKhoan? TimTaiKhoan(string tenDangNhap)
        {
            return _db.TaiKhoans.FirstOrDefault(x =>
                x.TenDangNhap == tenDangNhap && !x.IsDeleted);
        }

        // Tìm tài khoản theo mã
        public TaiKhoan? TimTaiKhoanTheoMa(int maTk)
        {
            return _db.TaiKhoans.FirstOrDefault(x =>
                x.MaTk == maTk && !x.IsDeleted);
        }

        // Tìm tài khoản theo email
        public TaiKhoan? TimTaiKhoanTheoEmail(string email)
        {
            return _db.TaiKhoans.FirstOrDefault(x =>
                x.Email == email && !x.IsDeleted);
        }

        // Kiểm tra email đã được sử dụng chưa
        public bool EmailTonTai(string email)
        {
            return _db.TaiKhoans.Any(x =>
                x.Email == email && !x.IsDeleted);
        }

        // Lấy quyền theo tên
        public Quyen? TimQuyen(string tenQuyen)
        {
            return _db.Quyens.FirstOrDefault(x =>
                x.TenQuyen == tenQuyen && !x.IsDeleted);
        }

        // Lấy quyền theo mã
        public Quyen? TimQuyenTheoMa(int maQuyen)
        {
            return _db.Quyens.FirstOrDefault(x =>
                x.MaQuyen == maQuyen && !x.IsDeleted);
        }

        // Thêm tài khoản
        public void ThemTaiKhoan(TaiKhoan taiKhoan)
        {
            _db.TaiKhoans.Add(taiKhoan);
            _db.SaveChanges();
        }

        // Thêm RefreshToken
        public void ThemRefreshToken(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            _db.SaveChanges();
        }

        // Tìm RefreshToken
        public RefreshToken? TimRefreshToken(string token)
        {
            return _db.RefreshTokens.FirstOrDefault(x =>
                x.Token == token);
        }

        // Cập nhật RefreshToken
        public void CapNhatRefreshToken(RefreshToken token)
        {
            _db.RefreshTokens.Update(token);
            _db.SaveChanges();
        }

        // Thêm PasswordResetToken
        public void ThemPasswordResetToken(PasswordResetToken token)
        {
            _db.PasswordResetTokens.Add(token);
            _db.SaveChanges();
        }

        // Tìm PasswordResetToken
        public PasswordResetToken? TimPasswordResetToken(string token)
        {
            return _db.PasswordResetTokens.FirstOrDefault(x =>
                x.Token == token);
        }

        // Cập nhật PasswordResetToken
        public void CapNhatPasswordResetToken(PasswordResetToken token)
        {
            _db.PasswordResetTokens.Update(token);
            _db.SaveChanges();
        }

        // Cập nhật tài khoản
        public void CapNhatTaiKhoan(TaiKhoan taiKhoan)
        {
            _db.TaiKhoans.Update(taiKhoan);
            _db.SaveChanges();
        }
    }
}
