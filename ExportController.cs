using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Data;
using System.Text;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExportController : ControllerBase
    {
        private readonly QuanLyBaoHiemContext _db;

        public ExportController(
            QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // =====================================================
        // 1. XUẤT KHÁCH HÀNG
        // =====================================================
        [HttpGet("khachhang/csv")]
        public IActionResult KhachHangCsv()
        {
            var data = _db.KhachHang
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.MaKH,
                    x.HoTen,
                    x.SoDienThoai,
                    x.DiaChi,
                    x.CCCD
                })
                .ToList();

            var sb = new StringBuilder();

            sb.AppendLine(
                "MaKH,HoTen,SoDienThoai,DiaChi,CCCD");

            foreach (var x in data)
            {
                sb.AppendLine(
                    $"{x.MaKH}," +
                    $"\"{x.HoTen}\"," +
                    $"\"{x.SoDienThoai}\"," +
                    $"\"{x.DiaChi}\"," +
                    $"\"{x.CCCD}\"");
            }

            return File(
                Encoding.UTF8.GetPreamble()
                    .Concat(
                        Encoding.UTF8.GetBytes(
                            sb.ToString()))
                    .ToArray(),

                "text/csv",
                "khachhang.csv");
        }

        // =====================================================
        // 2. XUẤT HỢP ĐỒNG
        // =====================================================
        [HttpGet("hopdong/csv")]
        public IActionResult HopDongCsv()
        {
            var data = _db.HopDong
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.MaHD,
                    x.MaKH,
                    x.MaLoai,
                    x.MaNV,
                    x.NgayBatDau,
                    x.NgayKetThuc,
                    x.SoTien,
                    x.TrangThai
                })
                .ToList();

            var sb = new StringBuilder();

            sb.AppendLine(
                "MaHD,MaKH,MaLoai,MaNV,NgayBatDau,NgayKetThuc,SoTien,TrangThai");

            foreach (var x in data)
            {
                sb.AppendLine(
                    $"{x.MaHD}," +
                    $"{x.MaKH}," +
                    $"{x.MaLoai}," +
                    $"{x.MaNV}," +
                    $"{x.NgayBatDau:yyyy-MM-dd}," +
                    $"{x.NgayKetThuc:yyyy-MM-dd}," +
                    $"{x.SoTien}," +
                    $"\"{x.TrangThai}\"");
            }

            return File(
                Encoding.UTF8.GetPreamble()
                    .Concat(
                        Encoding.UTF8.GetBytes(
                            sb.ToString()))
                    .ToArray(),

                "text/csv",
                "hopdong.csv");
        }

        // =====================================================
        // 3. XUẤT HÓA ĐƠN
        // =====================================================
        [HttpGet("hoadon/csv")]
        public IActionResult HoaDonCsv()
        {
            var data = _db.HoaDon
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.MaHDN,
                    x.MaHD,
                    x.NgayLap,
                    x.SoTien,
                    x.TrangThai
                })
                .ToList();

            var sb = new StringBuilder();

            sb.AppendLine(
                "MaHDN,MaHD,NgayLap,SoTien,TrangThai");

            foreach (var x in data)
            {
                sb.AppendLine(
                    $"{x.MaHDN}," +
                    $"{x.MaHD}," +
                    $"{x.NgayLap:yyyy-MM-dd}," +
                    $"{x.SoTien}," +
                    $"\"{x.TrangThai}\"");
            }

            return File(
                Encoding.UTF8.GetPreamble()
                    .Concat(
                        Encoding.UTF8.GetBytes(
                            sb.ToString()))
                    .ToArray(),

                "text/csv",
                "hoadon.csv");
        }

        // =====================================================
        // 4. XUẤT THANH TOÁN
        // =====================================================
        [HttpGet("thanhtoan/csv")]
        public IActionResult ThanhToanCsv()
        {
            var data = _db.ThanhToan
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    x.MaTT,
                    x.MaHDN,
                    x.NgayThanhToan,
                    x.SoTien,
                    x.PhuongThuc
                })
                .ToList();

            var sb = new StringBuilder();

            sb.AppendLine(
                "MaTT,MaHDN,NgayThanhToan,SoTien,PhuongThuc");

            foreach (var x in data)
            {
                sb.AppendLine(
                    $"{x.MaTT}," +
                    $"{x.MaHDN}," +
                    $"{x.NgayThanhToan:yyyy-MM-dd}," +
                    $"{x.SoTien}," +
                    $"\"{x.PhuongThuc}\"");
            }

            return File(
                Encoding.UTF8.GetPreamble()
                    .Concat(
                        Encoding.UTF8.GetBytes(
                            sb.ToString()))
                    .ToArray(),

                "text/csv",
                "thanhtoan.csv");
        }
    }
}