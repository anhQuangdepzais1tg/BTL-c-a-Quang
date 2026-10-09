using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : ControllerBase
    {
        private readonly AuditLogService _service;

        public AuditLogController(
            AuditLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll(
            int page = 1,
            int size = 50,
            string? bang = null)
        {
            if (page < 1)
                page = 1;

            if (size < 1 || size > 200)
                size = 50;

            return Ok(
                _service.GetAll(
                    page,
                    size,
                    bang));
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var data = _service.Get(id);

            if (data == null)
                return NotFound(
                    "Không tìm thấy log");

            return Ok(data);
        }
    }
}