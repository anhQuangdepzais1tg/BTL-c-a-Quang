using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BoiThuongController : ControllerBase
    {
        private readonly BoiThuongService _service;

        public BoiThuongController(BoiThuongService service)
        {
            _service = service;
        }

        // GET: api/BoiThuong
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/BoiThuong/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound("Không tìm thấy yêu cầu bồi thường");

            return Ok(data);
        }

        // POST: api/BoiThuong
        [HttpPost]
        public IActionResult Create([FromBody] BoiThuong boiThuong)
        {
            if (boiThuong == null)
                return BadRequest("Dữ liệu không hợp lệ");

            var result = _service.Add(boiThuong);

            if (!result)
                return BadRequest(
                    "Hợp đồng không tồn tại hoặc không còn hiệu lực");

            return Ok("Tạo yêu cầu bồi thường thành công");
        }

        // PUT: api/BoiThuong/duyet/1
        [HttpPut("duyet/{id}")]
        public IActionResult Duyet(
            int id,
            int maNVXuLy,
            int? nguoiCapNhat = null)
        {
            var result = _service.Duyet(
                id,
                maNVXuLy,
                nguoiCapNhat);

            if (!result)
                return BadRequest(
                    "Không tìm thấy yêu cầu hoặc yêu cầu không ở trạng thái Đang xử lý");

            return Ok("Duyệt bồi thường thành công");
        }

        // PUT: api/BoiThuong/tuchoi/1
        [HttpPut("tuchoi/{id}")]
        public IActionResult TuChoi(
            int id,
            int maNVXuLy,
            int? nguoiCapNhat = null)
        {
            var result = _service.TuChoi(
                id,
                maNVXuLy,
                nguoiCapNhat);

            if (!result)
                return BadRequest(
                    "Không tìm thấy yêu cầu hoặc yêu cầu không ở trạng thái Đang xử lý");

            return Ok("Từ chối bồi thường thành công");
        }

        // DELETE: api/BoiThuong/1
        [HttpDelete("{id}")]
        public IActionResult Delete(
            int id,
            int? nguoiXoa = null)
        {
            var result = _service.DeleteSoft(id, nguoiXoa);

            if (!result)
                return NotFound(
                    "Không tìm thấy yêu cầu bồi thường");

            return Ok("Xóa mềm yêu cầu bồi thường thành công");
        }
    }
}