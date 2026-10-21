using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChucNangController : ControllerBase
    {
        private readonly ChucNangService _service;

        public ChucNangController(ChucNangService service)
        {
            _service = service;
        }

        // GET: api/ChucNang
        [HttpGet]
        public IActionResult LayDanhSach()
        {
            return Ok(_service.LayDanhSach());
        }

        // GET: api/ChucNang/1
        [HttpGet("{ma}")]
        public IActionResult LayTheoMa(int ma)
        {
            var data = _service.LayTheoMa(ma);

            if (data == null)
                return NotFound(new { message = "Không tìm thấy chức năng." });

            return Ok(data);
        }

        // POST: api/ChucNang
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Them([FromBody] ChucNang request)
        {
            var result = _service.Them(request);

            if (!result.ThanhCong)
                return BadRequest(new { message = result.ThongBao });

            return Ok(new
            {
                message = result.ThongBao,
                data = result.DuLieu
            });
        }

        // PUT: api/ChucNang/1
        [HttpPut("{ma}")]
        [Authorize(Roles = "Admin")]
        public IActionResult CapNhat(int ma, [FromBody] ChucNang request)
        {
            var result = _service.CapNhat(ma, request);

            if (!result.ThanhCong)
            {
                if (result.ThongBao == "Không tìm thấy chức năng.")
                    return NotFound(new { message = result.ThongBao });

                return BadRequest(new { message = result.ThongBao });
            }

            return Ok(new { message = result.ThongBao });
        }

        // DELETE: api/ChucNang/1
        // Chỉ đánh dấu đã xóa, không xóa dữ liệu vật lý
        [HttpDelete("{ma}")]
        [Authorize(Roles = "Admin")]
        public IActionResult XoaMem(int ma)
        {
            var result = _service.XoaMem(ma);

            if (!result.ThanhCong)
                return NotFound(new { message = result.ThongBao });

            return Ok(new { message = result.ThongBao });
        }
    }
}