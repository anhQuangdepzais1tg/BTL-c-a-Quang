using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class AuthRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public AuthRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Tìm tài khoản theo tên đăng nhập
        public TaiKhoan? GetByUsername(string tenDangNhap)
        {
            return _context.TaiKhoan
                .FirstOrDefault(x =>
                    x.TenDangNhap == tenDangNhap &&
                    !x.IsDeleted);
        }

        // Kiểm tra tên đăng nhập đã tồn tại
        public bool UsernameExists(string tenDangNhap)
        {
            return _context.TaiKhoan
                .Any(x => x.TenDangNhap == tenDangNhap);
        }

        // Kiểm tra email đã tồn tại
        public bool EmailExists(string email)
        {
            return _context.TaiKhoan
                .Any(x => x.Email == email);
        }

        // Thêm tài khoản
        public void Add(TaiKhoan taiKhoan)
        {
            _context.TaiKhoan.Add(taiKhoan);
            _context.SaveChanges();
        }

        // Lấy quyền
        public Quyen? GetQuyen(int maQuyen)
        {
            return _context.Quyen
                .FirstOrDefault(x =>
                    x.MaQuyen == maQuyen &&
                    !x.IsDeleted);
        }

        // Ghi nhận đăng nhập sai
        public void LoginFail(TaiKhoan taiKhoan)
        {
            taiKhoan.SoLanDangNhapSai++;

            // Sai từ 5 lần trở lên thì khóa 15 phút
            if (taiKhoan.SoLanDangNhapSai >= 5)
            {
                taiKhoan.KhoaDenNgay = DateTime.Now.AddMinutes(15);
            }

            _context.SaveChanges();
        }

        // Ghi nhận đăng nhập thành công
        public void LoginSuccess(TaiKhoan taiKhoan)
        {
            taiKhoan.SoLanDangNhapSai = 0;
            taiKhoan.KhoaDenNgay = null;
            taiKhoan.LanDangNhapCuoi = DateTime.Now;

            _context.SaveChanges();
        }
    }
}