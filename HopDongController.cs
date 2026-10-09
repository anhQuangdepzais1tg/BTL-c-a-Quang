using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HopDongController : ControllerBase
    {
        private readonly HopDongService _service;

        public HopDongController(HopDongService service)
        {
            _service = service;
        }

        // ==========================================
        // GET: api/HopDong
        // ==========================================
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _service.GetAll();

            return Ok(data);
        }

        // ==========================================
        // GET: api/HopDong/1
        // ==========================================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            return Ok(data);
        }

        // ==========================================
        // POST: api/HopDong
        // ==========================================
        [HttpPost]
        public IActionResult Add(HopDong hopDong)
        {
            try
            {
                _service.Add(hopDong);

                return Ok("Thêm hợp đồng thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ==========================================
        // PUT: api/HopDong/1
        // ==========================================
        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            HopDong hopDong)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            hopDong.MaHD = id;

            _service.Update(hopDong);

            return Ok("Sửa hợp đồng thành công");
        }

        // ==========================================
        // DELETE: api/HopDong/1?nguoiXoa=4
        // ==========================================
        [HttpDelete("{id}")]
        public IActionResult Delete(
            int id,
            int? nguoiXoa = null)
        {
            var result =
                _service.DeleteSoft(id, nguoiXoa);

            if (!result)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            return Ok("Xóa mềm hợp đồng thành công");
        }

        // ==========================================
        // PUT: api/HopDong/giahan/1
        // ==========================================
        [HttpPut("giahan/{id}")]
        public IActionResult GiaHan(
            int id,
            DateTime ngayKetThucMoi,
            int? nguoiCapNhat = null)
        {
            var result =
                _service.GiaHan(
                    id,
                    ngayKetThucMoi,
                    nguoiCapNhat);

            if (!result)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            return Ok("Gia hạn hợp đồng thành công");
        }

        // ==========================================
        // PUT: api/HopDong/huy/1
        // ==========================================
        [HttpPut("huy/{id}")]
        public IActionResult Huy(
            int id,
            int? nguoiCapNhat = null)
        {
            var result =
                _service.Huy(
                    id,
                    nguoiCapNhat);

            if (!result)
            {
                return NotFound("Không tìm thấy hợp đồng");
            }

            return Ok("Hủy hợp đồng thành công");
        }
    }
}