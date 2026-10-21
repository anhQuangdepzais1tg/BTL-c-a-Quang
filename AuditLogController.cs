
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : ControllerBase
    {
        private readonly AuditLogService _service;

        public AuditLogController(AuditLogService service)
        {
            _service = service;
        }

        // GET: api/AuditLog?page=1&size=20&tenBang=HopDong
        [HttpGet]
        public async Task<IActionResult> LayTatCa(
            [FromQuery] int page = 1,
            [FromQuery] int size = 20,
            [FromQuery] string? tenBang = null)
        {
            var data = await _service.LayTatCa(page, size, tenBang);
            return Ok(data);
        }

        // GET: api/AuditLog/1
        [HttpGet("{maLog:long}")]
        public async Task<IActionResult> LayTheoMa(long maLog)
        {
            if (maLog <= 0)
                return BadRequest(new { message = "Mã log không hợp lệ." });

            var data = await _service.LayTheoMa(maLog);

            if (data == null)
                return NotFound(new { message = "Không tìm thấy nhật ký." });

            return Ok(data);
        }
    }
}
