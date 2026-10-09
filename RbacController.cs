using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RbacController : ControllerBase
    {
        private readonly RbacService _service;

        public RbacController(RbacService service)
        {
            _service = service;
        }

        // ================= QUYỀN =================

        [HttpGet("quyen")]
        public IActionResult GetQuyen()
        {
            return Ok(_service.QuyenAll());
        }

        [HttpGet("quyen/{id}")]
        public IActionResult GetQuyen(int id)
        {
            var data = _service.QuyenGet(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost("quyen")]
        public IActionResult AddQuyen(Quyen item)
        {
            _service.QuyenAdd(item);

            return Ok(item);
        }

        [HttpPut("quyen/{id}")]
        public IActionResult UpdateQuyen(
            int id,
            Quyen item)
        {
            if (!_service.QuyenUpdate(id, item))
                return NotFound();

            return Ok("Cập nhật quyền thành công");
        }

        [HttpDelete("quyen/{id}")]
        public IActionResult DeleteQuyen(int id)
        {
            if (!_service.QuyenDelete(id))
                return NotFound();

            return Ok("Xóa quyền thành công");
        }

        // ================= CHỨC NĂNG =================

        [HttpGet("chucnang")]
        public IActionResult GetChucNang()
        {
            return Ok(_service.ChucNangAll());
        }

        [HttpGet("chucnang/{id}")]
        public IActionResult GetChucNang(int id)
        {
            var data = _service.ChucNangGet(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost("chucnang")]
        public IActionResult AddChucNang(
            ChucNang item)
        {
            _service.ChucNangAdd(item);

            return Ok(item);
        }

        [HttpPut("chucnang/{id}")]
        public IActionResult UpdateChucNang(
            int id,
            ChucNang item)
        {
            if (!_service.ChucNangUpdate(id, item))
                return NotFound();

            return Ok(
                "Cập nhật chức năng thành công");
        }

        [HttpDelete("chucnang/{id}")]
        public IActionResult DeleteChucNang(int id)
        {
            if (!_service.ChucNangDelete(id))
                return NotFound();

            return Ok(
                "Xóa chức năng thành công");
        }

        // ================= PHÂN QUYỀN =================

        [HttpGet("phanquyen")]
        public IActionResult GetPhanQuyen()
        {
            return Ok(_service.PhanQuyenAll());
        }

        [HttpGet("phanquyen/{id}")]
        public IActionResult GetPhanQuyen(int id)
        {
            var data = _service.PhanQuyenGet(id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpPost("phanquyen")]
        public IActionResult AddPhanQuyen(
            PhanQuyen item)
        {
            if (!_service.PhanQuyenAdd(item))
            {
                return BadRequest(
                    "Quyền hoặc chức năng không tồn tại");
            }

            return Ok(
                "Thêm phân quyền thành công");
        }

        [HttpPut("phanquyen/{id}")]
        public IActionResult UpdatePhanQuyen(
            int id,
            PhanQuyen item)
        {
            if (!_service.PhanQuyenUpdate(id, item))
                return NotFound();

            return Ok(
                "Cập nhật phân quyền thành công");
        }

        [HttpDelete("phanquyen/{id}")]
        public IActionResult DeletePhanQuyen(int id)
        {
            if (!_service.PhanQuyenDelete(id))
                return NotFound();

            return Ok(
                "Xóa phân quyền thành công");
        }
    }
}