
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
    public class NhanVienController : ControllerBase
    {
        private readonly NhanVienService _service;

        public NhanVienController(NhanVienService service)
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

        [HttpGet("{maNv:int}")]
        public async Task<IActionResult> LayTheoMa(int maNv)
        {
            if (maNv <= 0)
                return BadRequest(new { message = "Mã nhân viên không hợp lệ." });

            var nv = await _service.LayTheoMa(maNv);

            return nv == null
                ? NotFound(new { message = "Không tìm thấy nhân viên." })
                : Ok(nv);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Them(NhanVienDto dto)
        {
            try
            {
                var nv = await _service.Them(dto, LayMaTaiKhoan());
                return Ok(new
                {
                    message = "Thêm nhân viên thành công.",
                    maNv = nv.MaNv
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{maNv:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CapNhat(int maNv, NhanVienDto dto)
        {
            if (maNv <= 0)
                return BadRequest(new { message = "Mã nhân viên không hợp lệ." });

            try
            {
                bool ok = await _service.CapNhat(
                    maNv, dto, LayMaTaiKhoan());

                return !ok
                    ? NotFound(new { message = "Không tìm thấy nhân viên." })
                    : Ok(new { message = "Cập nhật nhân viên thành công." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{maNv:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Xoa(int maNv)
        {
            if (maNv <= 0)
                return BadRequest(new { message = "Mã nhân viên không hợp lệ." });

            bool ok = await _service.XoaMem(maNv, LayMaTaiKhoan());

            return !ok
                ? NotFound(new { message = "Không tìm thấy nhân viên." })
                : Ok(new { message = "Xóa mềm nhân viên thành công." });
        }
    }
}
