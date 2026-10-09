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
    public class ThanhToanController : ControllerBase
    {
        private readonly ThanhToanService _service;

        public ThanhToanController(ThanhToanService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound("Không tìm thấy thanh toán");

            return Ok(data);
        }

        [HttpGet("hoadon/{maHDN}")]
        public IActionResult GetByHoaDon(int maHDN)
        {
            return Ok(_service.GetByHoaDon(maHDN));
        }

        [HttpPost]
        public IActionResult Create(ThanhToan item)
        {
            if (item.SoTien <= 0)
                return BadRequest(
                    "Số tiền phải lớn hơn 0");

            item.NguoiTao =
                int.TryParse(
                    User.FindFirst(
                        ClaimTypes.NameIdentifier)?.Value,
                    out var id)
                ? id
                : null;

            if (!_service.Add(item))
                return BadRequest(
                    "Hóa đơn không tồn tại hoặc dữ liệu không hợp lệ");

            return Ok("Ghi nhận thanh toán thành công");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_service.Delete(id))
                return NotFound("Không tìm thấy thanh toán");

            return Ok("Xóa mềm thanh toán thành công");
        }
    }
}