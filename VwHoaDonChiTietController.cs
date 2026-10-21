
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VwHoaDonChiTietController : ControllerBase
    {
        private readonly VwHoaDonChiTietService _service;

        public VwHoaDonChiTietController(VwHoaDonChiTietService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult LayDanhSach()
            => Ok(_service.LayDanhSach());

        [HttpGet("{maHdn:int}")]
        public IActionResult LayTheoMa(int maHdn)
        {
            var item = _service.LayTheoMa(maHdn);

            if (item == null)
                return NotFound(new { message = "Không tìm thấy hóa đơn." });

            return Ok(item);
        }
    }
}
