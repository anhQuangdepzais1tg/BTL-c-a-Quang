
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhanQuyenController : ControllerBase
    {
        private readonly PhanQuyenService _service;

        public PhanQuyenController(PhanQuyenService service)
        {
            _service = service;
        }

        // GET: api/PhanQuyen/quyen
        [HttpGet("quyen")]
        public async Task<IActionResult> LayDanhSachQuyen()
        {
            var data = await _service.LayDanhSachQuyen();
            return Ok(data);
        }

        // GET: api/PhanQuyen/chucnang
        [HttpGet("chucnang")]
        public async Task<IActionResult> LayDanhSachChucNang()
        {
            var data = await _service.LayDanhSachChucNang();
            return Ok(data);
        }

        // GET: api/PhanQuyen/vaitro/1
        [HttpGet("vaitro/{maQuyen}")]
        public async Task<IActionResult> LayQuyenTheoVaiTro(int maQuyen)
        {
            if (maQuyen <= 0)
                return BadRequest(new
                {
                    message = "Mã quyền không hợp lệ."
                });

            var data = await _service.LayQuyenTheoVaiTro(maQuyen);
            return Ok(data);
        }

        // PUT: api/PhanQuyen/capnhat
        // Chỉ Admin được cập nhật phân quyền
        [HttpPut("capnhat")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CapNhatQuyen(
            [FromBody] CapNhatQuyenRequest request)
        {
            if (request == null ||
                request.MaQuyen <= 0 ||
                request.MaChucNang <= 0)
            {
                return BadRequest(new
                {
                    message = "Mã quyền và mã chức năng phải lớn hơn 0."
                });
            }

            var ketQua = await _service.CapNhatQuyen(
                request.MaQuyen,
                request.MaChucNang,
                request.DuocXem,
                request.DuocThem,
                request.DuocSua,
                request.DuocXoa,
                request.DuocXuatFile);

            if (ketQua.StartsWith("Không tìm thấy"))
                return NotFound(new { message = ketQua });

            if (ketQua.Contains("không hợp lệ",
                StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { message = ketQua });

            return Ok(new
            {
                thanhCong = true,
                message = ketQua
            });
        }
    }

    // Dữ liệu nhận khi cập nhật phân quyền
    public class CapNhatQuyenRequest
    {
        public int MaQuyen { get; set; }
        public int MaChucNang { get; set; }
        public bool DuocXem { get; set; }
        public bool DuocThem { get; set; }
        public bool DuocSua { get; set; }
        public bool DuocXoa { get; set; }
        public bool DuocXuatFile { get; set; }
    }
}
