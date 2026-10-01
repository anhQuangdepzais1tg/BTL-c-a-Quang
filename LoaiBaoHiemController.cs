using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiBaoHiemController : ControllerBase
    {
        private readonly LoaiBaoHiemService _service;

        public LoaiBaoHiemController(LoaiBaoHiemService service)
        {
            _service = service;
        }

        // GET: api/LoaiBaoHiem
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _service.GetAll();

            return Ok(data);
        }

        // GET: api/LoaiBaoHiem/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        // POST: api/LoaiBaoHiem
        [HttpPost]
        public IActionResult Add(LoaiBaoHiem loaiBaoHiem)
        {
            _service.Add(loaiBaoHiem);

            return Ok("Thêm loại bảo hiểm thành công");
        }

        // PUT: api/LoaiBaoHiem/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, LoaiBaoHiem loaiBaoHiem)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound();
            }

            loaiBaoHiem.MaLoai = id;

            _service.Update(loaiBaoHiem);

            return Ok("Sửa loại bảo hiểm thành công");
        }

        // DELETE: api/LoaiBaoHiem/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);

            if (!result)
            {
                return NotFound("Không tìm thấy loại bảo hiểm");
            }

            return Ok("Xóa loại bảo hiểm thành công");
        }
    }
}