using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class HoaDonRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public HoaDonRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy danh sách hóa đơn chưa bị xóa
        public List<HoaDon> GetAll()
        {
            return _context.HoaDon
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaHDN)
                .ToList();
        }

        // Lấy hóa đơn theo mã
        public HoaDon? GetById(int id)
        {
            return _context.HoaDon
                .FirstOrDefault(x =>
                    x.MaHDN == id &&
                    !x.IsDeleted);
        }

        // Thêm hóa đơn
        public bool Add(HoaDon hoaDon)
        {
            // Kiểm tra hợp đồng có tồn tại không
            var hopDong = _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == hoaDon.MaHD &&
                    !x.IsDeleted);

            if (hopDong == null)
                return false;

            // Thiết lập thông tin hóa đơn
            hoaDon.NgayTao = DateTime.Now;
            hoaDon.TrangThai = "Chưa thanh toán";
            hoaDon.IsDeleted = false;

            _context.HoaDon.Add(hoaDon);
            _context.SaveChanges();

            return true;
        }

        // Cập nhật hóa đơn
        public bool Update(int id, HoaDon hoaDonMoi)
        {
            var data = _context.HoaDon
                .FirstOrDefault(x =>
                    x.MaHDN == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.NgayLap = hoaDonMoi.NgayLap;
            data.SoTien = hoaDonMoi.SoTien;
            data.TrangThai = hoaDonMoi.TrangThai;
            data.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();

            return true;
        }

        // Đánh dấu hóa đơn quá hạn
        public bool QuaHan(int id)
        {
            var data = _context.HoaDon
                .FirstOrDefault(x =>
                    x.MaHDN == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            // Không cho chuyển hóa đơn đã thanh toán thành quá hạn
            if (data.TrangThai == "Đã thanh toán")
                return false;

            data.TrangThai = "Quá hạn";
            data.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();

            return true;
        }

        // Xóa mềm
        public bool DeleteSoft(int id)
        {
            var data = _context.HoaDon
                .FirstOrDefault(x =>
                    x.MaHDN == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.IsDeleted = true;
            data.NgayXoa = DateTime.Now;

            _context.SaveChanges();

            return true;
        }
    }
}