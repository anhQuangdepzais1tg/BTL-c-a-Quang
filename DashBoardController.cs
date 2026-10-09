using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _service;

        public DashboardController(
            DashboardService service)
        {
            _service = service;
        }

        [HttpGet("tongquan")]
        public IActionResult TongQuan()
        {
            return Ok(_service.TongQuan());
        }

        [HttpGet("hopdong")]
        public IActionResult HopDong()
        {
            return Ok(
                _service.HopDongTheoTrangThai());
        }

        [HttpGet("boithuong")]
        public IActionResult BoiThuong()
        {
            return Ok(
                _service.BoiThuongTheoTrangThai());
        }

        [HttpGet("doanhthu")]
        public IActionResult DoanhThu()
        {
            return Ok(
                _service.DoanhThu());
        }
    }
}