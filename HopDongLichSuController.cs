using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Nhân viên")]
    public class HopDongLichSuController : ControllerBase
    {
        private readonly HopDongLichSuService _service;

        public HopDongLichSuController(HopDongLichSuService service)
        {
            _service = service;
        }

        // GET: api/HopDongLichSu
        [HttpGet]
        public IActionResult LayDanhSach()
        {
            return Ok(_service.LayDanhSach());
        }

        // GET: api/HopDongLichSu/hopdong/1
        [HttpGet("hopdong/{maHD}")]
        public IActionResult LayTheoMaHopDong(int maHD)
        {
            if (maHD <= 0)
                return BadRequest(new { message = "Mã hợp đồng không hợp lệ." });

            var data = _service.LayTheoMaHopDong(maHD);

            return Ok(data);
        }

        // GET: api/HopDongLichSu/1
        [HttpGet("{maLichSu:int}")]
        public IActionResult LayTheoMaLichSu(int maLichSu)
        {
            if (maLichSu <= 0)
                return BadRequest(new { message = "Mã lịch sử không hợp lệ." });

            var data = _service.LayTheoMaLichSu(maLichSu);

            if (data == null)
                return NotFound(new { message = "Không tìm thấy lịch sử hợp đồng." });

            return Ok(data);
        }
    }
}