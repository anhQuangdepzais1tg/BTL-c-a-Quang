
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Controllers
{
    public class ThanhToanCreateDto
    {
        public int MaHdn { get; set; }
        public DateOnly NgayThanhToan { get; set; }
        public decimal SoTien { get; set; }
        public string PhuongThuc { get; set; } = string.Empty;
        public int? NguoiTao { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly ThanhToanService _service;

        public ThanhToanController(ThanhToanService service)
        {
            _service = service;
        }

        // GET: api/ThanhToan
        [HttpGet]
        public async Task<IActionResult> LayDanhSach()
        {
            var danhSach = await _service.LayDanhSach();

            var ketQua = danhSach.Select(x => new
            {
                x.MaTt,
                x.MaHdn,
                x.NgayThanhToan,
                x.SoTien,
                x.PhuongThuc,
                x.NgayTao,
                x.NguoiTao
            });

            return Ok(ketQua);
        }

        // GET: api/ThanhToan/1
        [HttpGet("{maTt:int}")]
        public async Task<IActionResult> TimTheoMa(int maTt)
        {
            var x = await _service.TimTheoMa(maTt);

            if (x == null)
                return NotFound(new
                {
                    message = "Không tìm thấy thanh toán."
                });

            return Ok(new
            {
                x.MaTt,
                x.MaHdn,
                x.NgayThanhToan,
                x.SoTien,
                x.PhuongThuc,
                x.NgayTao,
                x.NguoiTao
            });
        }

        // POST: api/ThanhToan
        [HttpPost]
        public async Task<IActionResult> Them(
            [FromBody] ThanhToanCreateDto dto)
        {
            var thanhToan = new ThanhToan
            {
                MaHdn = dto.MaHdn,
                NgayThanhToan = dto.NgayThanhToan,
                SoTien = dto.SoTien,
                PhuongThuc = dto.PhuongThuc,
                NguoiTao = dto.NguoiTao
            };

            var ketQua = await _service.Them(thanhToan);

            if (ketQua != "Thêm thanh toán thành công.")
                return BadRequest(new { message = ketQua });

            return Ok(new { message = ketQua });
        }

        // PUT: api/ThanhToan/1
        [HttpPut("{maTt:int}")]
        public async Task<IActionResult> CapNhat(
            int maTt, [FromBody] ThanhToanCreateDto dto)
        {
            var thanhToan = new ThanhToan
            {
                MaHdn = dto.MaHdn,
                NgayThanhToan = dto.NgayThanhToan,
                SoTien = dto.SoTien,
                PhuongThuc = dto.PhuongThuc,
                NguoiTao = dto.NguoiTao
            };

            var ketQua = await _service.CapNhat(maTt, thanhToan);

            if (ketQua == "Không tìm thấy thanh toán.")
                return NotFound(new { message = ketQua });

            if (ketQua != "Cập nhật thanh toán thành công.")
                return BadRequest(new { message = ketQua });

            return Ok(new { message = ketQua });
        }

        // DELETE: api/ThanhToan/1
        [HttpDelete("{maTt:int}")]
        public async Task<IActionResult> Xoa(int maTt)
        {
            var ketQua = await _service.Xoa(maTt);

            if (ketQua == "Không tìm thấy thanh toán.")
                return NotFound(new { message = ketQua });

            return Ok(new { message = ketQua });
        }
    }
}
