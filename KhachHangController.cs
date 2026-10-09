using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly KhachHangService _service;

        public KhachHangController(KhachHangService service)
        {
            _service = service;
        }

        // GET: api/KhachHang
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _service.GetAll();

            return Ok(data);
        }

        // GET: api/KhachHang/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound("Không tìm thấy khách hàng");
            }

            return Ok(data);
        }

        // GET: api/KhachHang/paged
        [HttpGet("paged")]
        public IActionResult GetPaged(
            string? keyword = null,
            int pageIndex = 1,
            int pageSize = 10)
        {
            var result = _service.GetPaged(
                keyword,
                pageIndex,
                pageSize);

            return Ok(new
            {
                pageIndex = pageIndex,
                pageSize = pageSize,
                totalCount = result.TotalCount,
                data = result.Data
            });
        }

        // POST: api/KhachHang
        [HttpPost]
        public IActionResult Add(KhachHang khachHang)
        {
            // Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(khachHang.HoTen))
            {
                return BadRequest("Họ tên không được để trống.");
            }

            // Kiểm tra CCCD
            if (string.IsNullOrWhiteSpace(khachHang.CCCD))
            {
                return BadRequest("CCCD không được để trống.");
            }

            // Thêm khách hàng
            var result = _service.Add(khachHang);

            if (!result)
            {
                return BadRequest("CCCD đã tồn tại.");
            }

            return Ok("Thêm khách hàng thành công.");
        }

        // PUT: api/KhachHang/1
        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            KhachHang khachHang)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound("Không tìm thấy khách hàng");
            }

            khachHang.MaKH = id;

            _service.Update(khachHang);

            return Ok("Sửa khách hàng thành công");
        }

        // DELETE: api/KhachHang/1
        [HttpDelete("{id}")]
        public IActionResult Delete(
            int id,
            int? nguoiXoa = null)
        {
            try
            {
                var result = _service.DeleteSoft(
                    id,
                    nguoiXoa);

                if (!result)
                {
                    return NotFound("Không tìm thấy khách hàng");
                }

                return Ok("Xóa mềm khách hàng thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/KhachHang/restore/1
        [HttpPut("restore/{id}")]
        public IActionResult Restore(int id)
        {
            var result = _service.Restore(id);

            if (!result)
            {
                return NotFound("Không tìm thấy khách hàng");
            }

            return Ok("Khôi phục khách hàng thành công");
        }
    }
}