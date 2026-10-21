using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThongBaoController : ControllerBase
    {
        private readonly ThongBaoService _service;

        public ThongBaoController(ThongBaoService service)
        {
            _service = service;
        }

        // Lấy mã tài khoản từ JWT
        private int? LayMaTaiKhoan()
        {
            var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(value, out int maTk) ? maTk : null;
        }

        // Lấy danh sách thông báo của tài khoản đang đăng nhập
        [HttpGet]
        public async Task<IActionResult> LayDanhSach()
        {
            var maTk = LayMaTaiKhoan();

            if (maTk == null)
                return Unauthorized();

            return Ok(await _service.LayDanhSach(maTk.Value));
        }

        // Lấy chi tiết thông báo
        // Admin được xem mọi thông báo, tài khoản thường chỉ xem thông báo của mình
        [HttpGet("{maTb:int}")]
        public async Task<IActionResult> LayTheoMa(int maTb)
        {
            var maTk = LayMaTaiKhoan();

            if (maTk == null)
                return Unauthorized();

            var laAdmin = User.IsInRole("Admin");

            var tb = laAdmin
                ? await _service.LayTheoMaAdmin(maTb)
                : await _service.LayTheoMa(maTb, maTk.Value);

            if (tb == null)
                return NotFound(new { message = "Không tìm thấy thông báo." });

            return Ok(tb);
        }

        public class TaoThongBaoRequest
        {
            public int MaTk { get; set; }
            public string TieuDe { get; set; } = string.Empty;
            public string NoiDung { get; set; } = string.Empty;
            public string Loai { get; set; } = string.Empty;
        }

        // Admin hoặc Manager tạo thông báo
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Them(TaoThongBaoRequest request)
        {
            try
            {
                var tb = await _service.Them(
                    request.MaTk,
                    request.TieuDe,
                    request.NoiDung,
                    request.Loai);

                return Ok(new
                {
                    message = "Tạo thông báo thành công.",
                    maTb = tb.MaTb
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Đánh dấu thông báo đã đọc
        [HttpPut("{maTb:int}/dadoc")]
        public async Task<IActionResult> DanhDauDaDoc(int maTb)
        {
            var maTk = LayMaTaiKhoan();

            if (maTk == null)
                return Unauthorized();

            var laAdmin = User.IsInRole("Admin");

            var ok = await _service.DanhDauDaDoc(
                maTb, maTk.Value, laAdmin);

            if (!ok)
                return NotFound(new { message = "Không tìm thấy thông báo." });

            return Ok(new
            {
                message = "Đã đánh dấu thông báo đã đọc."
            });
        }
    }
}