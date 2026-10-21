
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class HangDoiTacVuController : ControllerBase
    {
        private readonly HangDoiTacVuService _service;

        public HangDoiTacVuController(HangDoiTacVuService service)
        {
            _service = service;
        }

        // GET: api/HangDoiTacVu
        [HttpGet]
        public IActionResult LayDanhSach()
        {
            return Ok(_service.LayDanhSach());
        }

        // GET: api/HangDoiTacVu/1
        [HttpGet("{maJob:long}")]
        public IActionResult LayTheoMa(long maJob)
        {
            var job = _service.LayTheoMa(maJob);

            if (job == null)
                return NotFound(new { message = "Không tìm thấy tác vụ." });

            return Ok(job);
        }
    }
}
