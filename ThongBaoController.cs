using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;
using System.Security.Claims;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThongBaoController : ControllerBase
    {
        private readonly ThongBaoService _service;

        public ThongBaoController(
            ThongBaoService service)
        {
            _service = service;
        }

        private int UserId
        {
            get
            {
                return int.Parse(
                    User.FindFirstValue(
                        ClaimTypes.NameIdentifier)!);
            }
        }

        [HttpGet]
        public IActionResult Get(
            [FromQuery] bool chuaDoc = false)
        {
            return Ok(
                _service.Get(UserId, chuaDoc));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data =
                _service.GetById(id, UserId);

            if (data == null)
                return NotFound(
                    "Không tìm thấy thông báo");

            return Ok(data);
        }

        [HttpPost]
        public IActionResult Create(ThongBao item)
        {
            if (string.IsNullOrWhiteSpace(item.TieuDe) ||
                string.IsNullOrWhiteSpace(item.NoiDung))
            {
                return BadRequest(
                    "Thiếu tiêu đề hoặc nội dung");
            }

            item.MaTK = UserId;

            return Ok(_service.Add(item));
        }

        [HttpPut("dadoc/{id}")]
        public IActionResult Read(int id)
        {
            if (!_service.Read(id, UserId))
                return NotFound(
                    "Không tìm thấy thông báo");

            return Ok("Đã đánh dấu đã đọc");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_service.Delete(id, UserId))
                return NotFound(
                    "Không tìm thấy thông báo");

            return Ok("Đã xóa thông báo");
        }
    }
}