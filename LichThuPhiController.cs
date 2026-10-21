
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Controllers
{
    // DTO nhận dữ liệu POST/PUT, tránh yêu cầu MaHdNavigation
    public class LichThuPhiDto
    {
        public int MaHd { get; set; }
        public int KyThu { get; set; }
        public DateOnly NgayDenHan { get; set; }
        public decimal SoTien { get; set; }
        public string TrangThai { get; set; } = string.Empty;
        public int? MaHdn { get; set; }
        public bool DaCanhBao { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class LichThuPhiController : ControllerBase
    {
        private readonly LichThuPhiService _service;

        public LichThuPhiController(LichThuPhiService service)
        {
            _service = service;
        }

        // GET: api/LichThuPhi
        [HttpGet]
        public async Task<IActionResult> LayDanhSach()
        {
            var danhSach = await _service.LayDanhSach();

            return Ok(danhSach.Select(x => new
            {
                x.MaLichThu,
                x.MaHd,
                x.KyThu,
                x.NgayDenHan,
                x.SoTien,
                x.TrangThai,
                x.MaHdn,
                x.DaCanhBao,
                x.NgayTao
            }));
        }

        // GET: api/LichThuPhi/1
        [HttpGet("{maLichThu:int}")]
        public async Task<IActionResult> TimTheoMa(int maLichThu)
        {
            var x = await _service.TimTheoMa(maLichThu);

            if (x == null)
                return NotFound(new
                {
                    message = "Không tìm thấy lịch thu phí."
                });

            return Ok(new
            {
                x.MaLichThu,
                x.MaHd,
                x.KyThu,
                x.NgayDenHan,
                x.SoTien,
                x.TrangThai,
                x.MaHdn,
                x.DaCanhBao,
                x.NgayTao
            });
        }

        // POST: api/LichThuPhi
        [HttpPost]
        public async Task<IActionResult> Them([FromBody] LichThuPhiDto dto)
        {
            var lich = new LichThuPhi
            {
                MaHd = dto.MaHd,
                KyThu = dto.KyThu,
                NgayDenHan = dto.NgayDenHan,
                SoTien = dto.SoTien,
                TrangThai = dto.TrangThai,
                MaHdn = dto.MaHdn,
                DaCanhBao = dto.DaCanhBao
            };

            var ketQua = await _service.Them(lich);

            if (ketQua != "Thêm lịch thu phí thành công.")
                return BadRequest(new { message = ketQua });

            return Ok(new { message = ketQua });
        }

        // PUT: api/LichThuPhi/1
        [HttpPut("{maLichThu:int}")]
        public async Task<IActionResult> CapNhat(
            int maLichThu, [FromBody] LichThuPhiDto dto)
        {
            var lich = new LichThuPhi
            {
                MaHd = dto.MaHd,
                KyThu = dto.KyThu,
                NgayDenHan = dto.NgayDenHan,
                SoTien = dto.SoTien,
                TrangThai = dto.TrangThai,
                MaHdn = dto.MaHdn,
                DaCanhBao = dto.DaCanhBao
            };

            var ketQua = await _service.CapNhat(maLichThu, lich);

            if (ketQua == "Không tìm thấy lịch thu phí.")
                return NotFound(new { message = ketQua });

            if (ketQua != "Cập nhật lịch thu phí thành công.")
                return BadRequest(new { message = ketQua });

            return Ok(new { message = ketQua });
        }

        // DELETE: api/LichThuPhi/1
        [HttpDelete("{maLichThu:int}")]
        public async Task<IActionResult> Xoa(int maLichThu)
        {
            var ketQua = await _service.Xoa(maLichThu);

            if (ketQua == "Không tìm thấy lịch thu phí.")
                return NotFound(new { message = ketQua });

            return Ok(new { message = ketQua });
        }
    }
}
