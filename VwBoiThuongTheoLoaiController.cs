
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VwBoiThuongTheoLoaiController : ControllerBase
    {
        private readonly VwBoiThuongTheoLoaiService _service;

        public VwBoiThuongTheoLoaiController(VwBoiThuongTheoLoaiService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult LayDanhSach()
            => Ok(_service.LayDanhSach());

        [HttpGet("theo-loai")]
        public IActionResult LayTheoLoai([FromQuery] string tenLoai)
        {
            if (string.IsNullOrWhiteSpace(tenLoai))
                return BadRequest(new { message = "Vui lòng nhập tên loại bảo hiểm." });

            var item = _service.LayTheoLoai(tenLoai);

            if (item == null)
                return NotFound(new { message = "Không tìm thấy loại bảo hiểm." });

            return Ok(item);
        }
    }
}
