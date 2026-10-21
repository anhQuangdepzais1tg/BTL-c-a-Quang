
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VwHopDongChiTietController : ControllerBase
    {
        private readonly VwHopDongChiTietService _service;

        public VwHopDongChiTietController(VwHopDongChiTietService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult LayDanhSach()
            => Ok(_service.LayDanhSach());

        [HttpGet("{maHd:int}")]
        public IActionResult LayTheoMa(int maHd)
        {
            var item = _service.LayTheoMa(maHd);

            if (item == null)
                return NotFound(new { message = "Không tìm thấy hợp đồng." });

            return Ok(item);
        }
    }
}
