using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Data;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly QuanLyBaoHiemContext _db;

        public SearchController(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // =====================================================
        // 1. TÌM KIẾM KHÁCH HÀNG
        // =====================================================
        [HttpGet("khachhang")]
        public IActionResult KhachHang(
            string? keyword,
            int page = 1,
            int size = 20)
        {
            var query = _db.KhachHang
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.HoTen.Contains(keyword) ||
                    x.SoDienThoai.Contains(keyword) ||
                    x.CCCD.Contains(keyword));
            }

            return Page(
                query.OrderBy(x => x.MaKH),
                page,
                size);
        }

        // =====================================================
        // 2. TÌM KIẾM NHÂN VIÊN
        // =====================================================
        [HttpGet("nhanvien")]
        public IActionResult NhanVien(
            string? keyword,
            int page = 1,
            int size = 20)
        {
            var query = _db.NhanVien
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.HoTen.Contains(keyword) ||
                    x.SoDienThoai.Contains(keyword) ||
                    x.ChucVu.Contains(keyword));
            }

            return Page(
                query.OrderBy(x => x.MaNV),
                page,
                size);
        }

        // =====================================================
        // 3. TÌM KIẾM LOẠI BẢO HIỂM
        // =====================================================
        [HttpGet("loaibaohiem")]
        public IActionResult LoaiBaoHiem(
            string? keyword,
            decimal? minPhi,
            decimal? maxPhi,
            int page = 1,
            int size = 20)
        {
            var query = _db.LoaiBaoHiem
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.TenLoai.Contains(keyword) ||
                    (x.MoTa != null &&
                     x.MoTa.Contains(keyword)));
            }

            if (minPhi.HasValue)
            {
                query = query.Where(
                    x => x.MucPhi >= minPhi.Value);
            }

            if (maxPhi.HasValue)
            {
                query = query.Where(
                    x => x.MucPhi <= maxPhi.Value);
            }

            return Page(
                query.OrderBy(x => x.MaLoai),
                page,
                size);
        }

        // =====================================================
        // 4. TÌM KIẾM HỢP ĐỒNG
        // =====================================================
        [HttpGet("hopdong")]
        public IActionResult HopDong(
            string? trangThai,
            int? maKH,
            int? maLoai,
            int page = 1,
            int size = 20)
        {
            var query = _db.HopDong
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(
                    x => x.TrangThai == trangThai);
            }

            if (maKH.HasValue)
            {
                query = query.Where(
                    x => x.MaKH == maKH.Value);
            }

            if (maLoai.HasValue)
            {
                query = query.Where(
                    x => x.MaLoai == maLoai.Value);
            }

            return Page(
                query.OrderByDescending(x => x.MaHD),
                page,
                size);
        }

        // =====================================================
        // 5. TÌM KIẾM BỒI THƯỜNG
        // =====================================================
        [HttpGet("boithuong")]
        public IActionResult BoiThuong(
            string? trangThai,
            int? maHD,
            int page = 1,
            int size = 20)
        {
            var query = _db.BoiThuong
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(
                    x => x.TrangThai == trangThai);
            }

            if (maHD.HasValue)
            {
                query = query.Where(
                    x => x.MaHD == maHD.Value);
            }

            return Page(
                query.OrderByDescending(x => x.MaBT),
                page,
                size);
        }

        // =====================================================
        // 6. TÌM KIẾM HÓA ĐƠN
        // =====================================================
        [HttpGet("hoadon")]
        public IActionResult HoaDon(
            string? trangThai,
            int? maHD,
            DateTime? tuNgay,
            DateTime? denNgay,
            int page = 1,
            int size = 20)
        {
            var query = _db.HoaDon
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(
                    x => x.TrangThai == trangThai);
            }

            if (maHD.HasValue)
            {
                query = query.Where(
                    x => x.MaHD == maHD.Value);
            }

            if (tuNgay.HasValue)
            {
                query = query.Where(
                    x => x.NgayLap >= tuNgay.Value.Date);
            }

            if (denNgay.HasValue)
            {
                query = query.Where(
                    x => x.NgayLap <= denNgay.Value.Date);
            }

            return Page(
                query.OrderByDescending(x => x.MaHDN),
                page,
                size);
        }

        // =====================================================
        // HÀM PHÂN TRANG DÙNG CHUNG
        // =====================================================
        private IActionResult Page<T>(
            IQueryable<T> query,
            int page,
            int size)
        {
            // Nếu page nhỏ hơn 1
            if (page < 1)
            {
                page = 1;
            }

            // Chỉ cho phép size từ 1 đến 100
            if (size < 1 || size > 100)
            {
                size = 20;
            }

            // Tổng số bản ghi
            var total = query.Count();

            // Lấy dữ liệu theo trang
            var data = query
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();

            // Trả kết quả
            return Ok(new
            {
                pageIndex = page,
                pageSize = size,
                totalCount = total,

                totalPages =
                    (int)Math.Ceiling(
                        total / (double)size),

                data
            });
        }
    }
}