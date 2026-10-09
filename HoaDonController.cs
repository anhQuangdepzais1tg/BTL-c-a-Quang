using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HoaDonController : ControllerBase
    {
        private readonly HoaDonService _service;

        public HoaDonController(HoaDonService service)
        {
            _service = service;
        }

        // GET: api/HoaDon
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/HoaDon/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound("Không tìm thấy hóa đơn");

            return Ok(data);
        }

        // POST: api/HoaDon
        [HttpPost]
        public IActionResult Create([FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null)
                return BadRequest("Dữ liệu không hợp lệ");

            if (hoaDon.SoTien < 0)
                return BadRequest("Số tiền không được âm");

            var result = _service.Add(hoaDon);

            if (!result)
                return BadRequest("Hợp đồng không tồn tại");

            return Ok("Tạo hóa đơn thành công");
        }

        // PUT: api/HoaDon/1
        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            [FromBody] HoaDon hoaDon)
        {
            if (hoaDon == null)
                return BadRequest("Dữ liệu không hợp lệ");

            if (hoaDon.SoTien < 0)
                return BadRequest("Số tiền không được âm");

            var result = _service.Update(id, hoaDon);

            if (!result)
                return NotFound("Không tìm thấy hóa đơn");

            return Ok("Cập nhật hóa đơn thành công");
        }

        // PUT: api/HoaDon/quahan/1
        [HttpPut("quahan/{id}")]
        public IActionResult QuaHan(int id)
        {
            var result = _service.QuaHan(id);

            if (!result)
                return BadRequest(
                    "Không tìm thấy hóa đơn hoặc hóa đơn đã thanh toán");

            return Ok("Đã chuyển hóa đơn sang trạng thái Quá hạn");
        }

        // DELETE: api/HoaDon/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.DeleteSoft(id);

            if (!result)
                return NotFound("Không tìm thấy hóa đơn");

            return Ok("Xóa mềm hóa đơn thành công");
        }
    }
}