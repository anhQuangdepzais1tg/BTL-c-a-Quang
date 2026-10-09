using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly NhanVienService _service;

        public NhanVienController(NhanVienService service)
        {
            _service = service;
        }

        // GET: api/NhanVien
        [HttpGet]
        public IActionResult GetAll()
        {
            var data = _service.GetAll();

            return Ok(data);
        }

        // GET: api/NhanVien/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound("Không tìm thấy nhân viên");
            }

            return Ok(data);
        }

        // POST: api/NhanVien
        [HttpPost]
        public IActionResult Add(NhanVien nhanVien)
        {
            // Kiểm tra họ tên
            if (string.IsNullOrWhiteSpace(nhanVien.HoTen))
            {
                return BadRequest("Họ tên không được để trống");
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrWhiteSpace(nhanVien.SoDienThoai))
            {
                return BadRequest("Số điện thoại không được để trống");
            }

            // Kiểm tra chức vụ
            if (string.IsNullOrWhiteSpace(nhanVien.ChucVu))
            {
                return BadRequest("Chức vụ không được để trống");
            }

            var result = _service.Add(nhanVien);

            if (!result)
            {
                return BadRequest("Số điện thoại đã tồn tại");
            }

            return Ok("Thêm nhân viên thành công");
        }

        // PUT: api/NhanVien/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, NhanVien nhanVien)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound("Không tìm thấy nhân viên");
            }

            // Gán mã lấy từ URL
            nhanVien.MaNV = id;

            // Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(nhanVien.HoTen))
            {
                return BadRequest("Họ tên không được để trống");
            }

            if (string.IsNullOrWhiteSpace(nhanVien.SoDienThoai))
            {
                return BadRequest("Số điện thoại không được để trống");
            }

            if (string.IsNullOrWhiteSpace(nhanVien.ChucVu))
            {
                return BadRequest("Chức vụ không được để trống");
            }

            var result = _service.Update(nhanVien);

            if (!result)
            {
                return BadRequest("Số điện thoại đã tồn tại");
            }

            return Ok("Sửa nhân viên thành công");
        }

        // DELETE: api/NhanVien/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id, int? nguoiXoa = null)
        {
            var result = _service.DeleteSoft(id, nguoiXoa);

            if (!result)
            {
                return NotFound("Không tìm thấy nhân viên");
            }

            return Ok("Xóa mềm nhân viên thành công");
        }
    }
}