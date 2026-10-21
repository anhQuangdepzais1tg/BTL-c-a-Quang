
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models.DTO;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoaiBaoHiemController : ControllerBase
    {
        private readonly LoaiBaoHiemService _service;

        public LoaiBaoHiemController(LoaiBaoHiemService service)
        {
            _service = service;
        }

        private int? LayMaTaiKhoan()
        {
            var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(value, out int maTk) ? maTk : null;
        }

        [HttpGet]
        public async Task<IActionResult> LayTatCa(
            [FromQuery] string? tuKhoa = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
            => Ok(await _service.LayTatCa(tuKhoa, page, size));

        [HttpGet("{maLoai:int}")]
        public async Task<IActionResult> LayTheoMa(int maLoai)
        {
            if (maLoai <= 0)
                return BadRequest(new { message = "Mã loại bảo hiểm không hợp lệ." });

            var loai = await _service.LayTheoMa(maLoai);

            return loai == null
                ? NotFound(new { message = "Không tìm thấy loại bảo hiểm." })
                : Ok(loai);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Them(LoaiBaoHiemDto dto)
        {
            try
            {
                var loai = await _service.Them(dto, LayMaTaiKhoan());

                return Ok(new
                {
                    message = "Thêm loại bảo hiểm thành công.",
                    maLoai = loai.MaLoai
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{maLoai:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CapNhat(
            int maLoai, LoaiBaoHiemDto dto)
        {
            if (maLoai <= 0)
                return BadRequest(new { message = "Mã loại bảo hiểm không hợp lệ." });

            try
            {
                bool ok = await _service.CapNhat(
                    maLoai, dto, LayMaTaiKhoan());

                return !ok
                    ? NotFound(new { message = "Không tìm thấy loại bảo hiểm." })
                    : Ok(new { message = "Cập nhật loại bảo hiểm thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{maLoai:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Xoa(int maLoai)
        {
            if (maLoai <= 0)
                return BadRequest(new { message = "Mã loại bảo hiểm không hợp lệ." });

            bool ok = await _service.XoaMem(maLoai);

            return !ok
                ? NotFound(new { message = "Không tìm thấy loại bảo hiểm." })
                : Ok(new { message = "Xóa mềm loại bảo hiểm thành công." });
        }
    }
}
