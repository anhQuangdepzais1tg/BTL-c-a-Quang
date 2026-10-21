
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoiThuongLichSuController : ControllerBase
    {
        private readonly BoiThuongLichSuService _service;

        public BoiThuongLichSuController(BoiThuongLichSuService service)
        {
            _service = service;
        }

        // GET: api/BoiThuongLichSu
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var danhSach = await _service.GetAll();
            return Ok(danhSach);
        }

        // GET: api/BoiThuongLichSu/3
        [HttpGet("{maBt}")]
        public async Task<IActionResult> GetByMaBoiThuong(int maBt)
        {
            if (maBt <= 0)
                return BadRequest(new { message = "Mã bồi thường không hợp lệ." });

            var danhSach = await _service.GetByMaBoiThuong(maBt);
            return Ok(danhSach);
        }
    }
}
