
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Models.DTO;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly HoaDonService _hoaDonService;

        public HoaDonController(HoaDonService hoaDonService)
        {
            _hoaDonService = hoaDonService;
        }

        // GET: api/HoaDon
        [HttpGet]
        public async Task<IActionResult> LayDanhSach()
        {
            var danhSach = await _hoaDonService.LayDanhSach();

            // Chỉ trả về dữ liệu cần thiết, tránh lỗi JSON vòng lặp
            var ketQua = danhSach.Select(x => new
            {
                x.MaHdn,
                x.MaHd,
                x.NgayLap,
                x.SoTien,
                x.TrangThai,
                x.NgayTao,
                x.NgayCapNhat
            });

            return Ok(ketQua);
        }

        // GET: api/HoaDon/1
        [HttpGet("{maHdn:int}")]
        public async Task<IActionResult> TimTheoMa(int maHdn)
        {
            var hoaDon = await _hoaDonService.TimTheoMa(maHdn);

            if (hoaDon == null)
                return NotFound(new { message = "Không tìm thấy hóa đơn." });

            return Ok(new
            {
                hoaDon.MaHdn,
                hoaDon.MaHd,
                hoaDon.NgayLap,
                hoaDon.SoTien,
                hoaDon.TrangThai,
                hoaDon.NgayTao,
                hoaDon.NgayCapNhat
            });
        }

        // POST: api/HoaDon

        [HttpPost]
        public async Task<IActionResult> Them(
            [FromBody] HoaDonCreateDto dto)
        {
            var hoaDon = new HoaDon
            {
                MaHd = dto.MaHd,
                NgayLap = dto.NgayLap,
                SoTien = dto.SoTien,
                TrangThai = dto.TrangThai,
                NguoiTao = dto.NguoiTao
            };

            var ketQua = await _hoaDonService.Them(hoaDon);

            if (ketQua != "Thêm hóa đơn thành công.")
                return BadRequest(new { message = ketQua });

            return Ok(new { message = ketQua });
        }


        // PUT: api/HoaDon/1

        [HttpPut("{maHdn:int}")]
        public async Task<IActionResult> CapNhat(
            int maHdn,
            [FromBody] HoaDonCreateDto dto)
        {
            var hoaDon = new HoaDon
            {
                MaHd = dto.MaHd,
                NgayLap = dto.NgayLap,
                SoTien = dto.SoTien,
                TrangThai = dto.TrangThai,
                NguoiTao = dto.NguoiTao
            };

            var ketQua = await _hoaDonService.CapNhat(maHdn, hoaDon);

            if (ketQua == "Không tìm thấy hóa đơn.")
                return NotFound(new { message = ketQua });

            if (ketQua != "Cập nhật hóa đơn thành công.")
                return BadRequest(new { message = ketQua });

            return Ok(new { message = ketQua });
        }


        // DELETE: api/HoaDon/1
        [HttpDelete("{maHdn:int}")]
        public async Task<IActionResult> Xoa(int maHdn)
        {
            var ketQua = await _hoaDonService.Xoa(maHdn);

            if (ketQua == "Không tìm thấy hóa đơn.")
                return NotFound(new { message = ketQua });

            return Ok(new { message = ketQua });
        }
    }
}
