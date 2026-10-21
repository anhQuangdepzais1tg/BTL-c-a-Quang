using QuanLyBaoHiem.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HopDongController : ControllerBase
    {
        private readonly HopDongService _service;

        public HopDongController(HopDongService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hopDong = await _service.GetById(id);

            if (hopDong == null)
                return NotFound(new { message = "Không tìm thấy hợp đồng." });

            return Ok(hopDong);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HopDongCreateDto dto)
        {
            var hopDong = new HopDong
            {
                MaKh = dto.MaKh,
                MaLoai = dto.MaLoai,
                MaNv = dto.MaNv,
                NgayBatDau = dto.NgayBatDau,
                NgayKetThuc = dto.NgayKetThuc,
                SoTien = dto.SoTien,
                ChuKyDongPhi = dto.ChuKyDongPhi,
                NguoiTao = dto.NguoiTao
            };

            var ketQua = await _service.Create(hopDong);

            if (!ketQua.ThanhCong)
            {
                return BadRequest(new { message = ketQua.ThongBao });
            }

            return Ok(new
            {
                message = ketQua.ThongBao,
                data = ketQua.DuLieu
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
    int id, [FromBody] HopDongCreateDto dto)
        {
            var hopDong = new HopDong
            {
                MaKh = dto.MaKh,
                MaLoai = dto.MaLoai,
                MaNv = dto.MaNv,
                NgayBatDau = dto.NgayBatDau,
                NgayKetThuc = dto.NgayKetThuc,
                SoTien = dto.SoTien,
                ChuKyDongPhi = dto.ChuKyDongPhi
            };

            var ketQua = await _service.Update(id, hopDong);

            if (!ketQua.ThanhCong)
            {
                return BadRequest(new { message = ketQua.ThongBao });
            }

            return Ok(new
            {
                message = ketQua.ThongBao
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ketQua = await _service.Delete(id);

            if (!ketQua.ThanhCong)
                return NotFound(new { message = ketQua.ThongBao });

            return Ok(new { message = ketQua.ThongBao });
        }
    }
}
