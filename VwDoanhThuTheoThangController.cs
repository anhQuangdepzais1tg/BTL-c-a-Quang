
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VwDoanhThuTheoThangController : ControllerBase
    {
        private readonly VwDoanhThuTheoThangService _service;

        public VwDoanhThuTheoThangController(VwDoanhThuTheoThangService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult LayDanhSach()
            => Ok(_service.LayDanhSach());

        [HttpGet("theo-thang")]
        public IActionResult LayTheoThang([FromQuery] int nam, [FromQuery] int thang)
        {
            if (nam < 2000 || thang < 1 || thang > 12)
                return BadRequest(new { message = "Năm hoặc tháng không hợp lệ." });

            var item = _service.LayTheoThang(nam, thang);

            if (item == null)
                return NotFound(new { message = "Không có dữ liệu doanh thu tháng này." });

            return Ok(item);
        }
    }
}
