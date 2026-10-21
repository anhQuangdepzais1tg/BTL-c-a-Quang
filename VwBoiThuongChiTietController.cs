
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VwBoiThuongChiTietController : ControllerBase
    {
        private readonly VwBoiThuongChiTietService _service;

        public VwBoiThuongChiTietController(
            VwBoiThuongChiTietService service)
        {
            _service = service;
        }

        // GET: api/VwBoiThuongChiTiet
        [HttpGet]
        public IActionResult LayDanhSach()
        {
            return Ok(_service.LayDanhSach());
        }

        // GET: api/VwBoiThuongChiTiet/1
        [HttpGet("{maBt}")]
        public IActionResult LayTheoMa(int maBt)
        {
            var item = _service.LayTheoMa(maBt);

            if (item == null)
                return NotFound(new { message = "Không tìm thấy thông tin bồi thường." });

            return Ok(item);
        }
    }
}
