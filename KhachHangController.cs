
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
    public class KhachHangController : ControllerBase
    {
        private readonly KhachHangService _service;

        public KhachHangController(KhachHangService service)
        {
            _service = service;
        }

        private int? LayMaTaiKhoan()
        {
            var value = User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(value, out int maTk) ? maTk : null;
        }

        [HttpGet]
        public async Task<IActionResult> LayTatCa(
            [FromQuery] string? tuKhoa = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            return Ok(await _service.LayTatCa(tuKhoa, page, size));
        }

        [HttpGet("{maKh:int}")]
        public async Task<IActionResult> LayTheoMa(int maKh)
        {
            if (maKh <= 0)
                return BadRequest(new { message = "Mã khách hàng không hợp lệ." });

            var kh = await _service.LayTheoMa(maKh);

            if (kh == null)
                return NotFound(new { message = "Không tìm thấy khách hàng." });

            return Ok(kh);
        }

        [HttpPost]
        public async Task<IActionResult> Them([FromBody] KhachHangDto dto)
        {
            try
            {
                var kh = await _service.Them(dto, LayMaTaiKhoan());

                return Ok(new
                {
                    message = "Thêm khách hàng thành công.",
                    maKh = kh.MaKh
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{maKh:int}")]
        public async Task<IActionResult> CapNhat(
            int maKh, [FromBody] KhachHangDto dto)
        {
            if (maKh <= 0)
                return BadRequest(new { message = "Mã khách hàng không hợp lệ." });

            try
            {
                var thanhCong = await _service.CapNhat(
                    maKh, dto, LayMaTaiKhoan());

                if (!thanhCong)
                    return NotFound(new { message = "Không tìm thấy khách hàng." });

                return Ok(new { message = "Cập nhật khách hàng thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{maKh:int}")]
        public async Task<IActionResult> Xoa(int maKh)
        {
            if (maKh <= 0)
                return BadRequest(new { message = "Mã khách hàng không hợp lệ." });

            var thanhCong = await _service.XoaMem(maKh, LayMaTaiKhoan());

            if (!thanhCong)
                return NotFound(new { message = "Không tìm thấy khách hàng." });

            return Ok(new { message = "Xóa mềm khách hàng thành công." });
        }
    }
}
