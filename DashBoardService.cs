using QuanLyBaoHiem.Data;

namespace QuanLyBaoHiem.Services
{
    public class DashboardService
    {
        private readonly QuanLyBaoHiemContext _db;

        public DashboardService(
            QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        public object TongQuan()
        {
            return new
            {
                tongKhachHang =
                    _db.KhachHang.Count(
                        x => !x.IsDeleted),

                tongNhanVien =
                    _db.NhanVien.Count(
                        x => !x.IsDeleted),

                tongLoaiBaoHiem =
                    _db.LoaiBaoHiem.Count(
                        x => !x.IsDeleted),

                tongHopDong =
                    _db.HopDong.Count(
                        x => !x.IsDeleted),

                tongBoiThuong =
                    _db.BoiThuong.Count(
                        x => !x.IsDeleted),

                tongHoaDon =
                    _db.HoaDon.Count(
                        x => !x.IsDeleted),

                tongThanhToan =
                    _db.ThanhToan.Count(
                        x => !x.IsDeleted),

                tongTaiLieu =
                    _db.TaiLieuDinhKem.Count(
                        x => !x.IsDeleted)
            };
        }

        public object HopDongTheoTrangThai()
        {
            return _db.HopDong
                .Where(x => !x.IsDeleted)
                .GroupBy(x => x.TrangThai)
                .Select(g => new
                {
                    trangThai = g.Key,
                    soLuong = g.Count()
                })
                .ToList();
        }

        public object BoiThuongTheoTrangThai()
        {
            return _db.BoiThuong
                .Where(x => !x.IsDeleted)
                .GroupBy(x => x.TrangThai)
                .Select(g => new
                {
                    trangThai = g.Key,
                    soLuong = g.Count(),
                    tongTien = g.Sum(x => x.SoTien)
                })
                .ToList();
        }

        public object DoanhThu()
        {
            return _db.ThanhToan
                .Where(x => !x.IsDeleted)
                .GroupBy(x => new
                {
                    x.NgayThanhToan.Year,
                    x.NgayThanhToan.Month
                })
                .OrderBy(x => x.Key.Year)
                .ThenBy(x => x.Key.Month)
                .Select(g => new
                {
                    nam = g.Key.Year,
                    thang = g.Key.Month,
                    doanhThu = g.Sum(
                        x => x.SoTien)
                })
                .ToList();
        }
    }
}