using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichThuPhiController : ControllerBase
    {
        private readonly LichThuPhiService _service;

        public LichThuPhiController(LichThuPhiService service)
        {
            _service = service;
        }

        // GET: api/LichThuPhi
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/LichThuPhi/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
                return NotFound("Không tìm thấy lịch thu phí");

            return Ok(data);
        }

        // GET: api/LichThuPhi/hopdong/1
        [HttpGet("hopdong/{maHD}")]
        public IActionResult GetByHopDong(int maHD)
        {
            var data = _service.GetByHopDong(maHD);

            return Ok(data);
        }

        // POST: api/LichThuPhi
        [HttpPost]
        public IActionResult Create([FromBody] LichThuPhi lich)
        {
            if (lich == null)
                return BadRequest("Dữ liệu không hợp lệ");

            if (lich.SoTien < 0)
                return BadRequest("Số tiền không được âm");

            var result = _service.Add(lich);

            if (!result)
                return BadRequest("Hợp đồng không tồn tại");

            return Ok("Thêm lịch thu phí thành công");
        }

        // PUT: api/LichThuPhi/1
        [HttpPut("{id}")]
        public IActionResult Update(
            int id,
            [FromBody] LichThuPhi lich)
        {
            if (lich == null)
                return BadRequest("Dữ liệu không hợp lệ");

            if (lich.SoTien < 0)
                return BadRequest("Số tiền không được âm");

            var result = _service.Update(id, lich);

            if (!result)
                return NotFound("Không tìm thấy lịch thu phí");

            return Ok("Cập nhật lịch thu phí thành công");
        }

        // PUT: api/LichThuPhi/dathanhtoan/1
        [HttpPut("dathanhtoan/{id}")]
        public IActionResult DaThanhToan(int id)
        {
            var result = _service.DaThanhToan(id);

            if (!result)
                return NotFound("Không tìm thấy lịch thu phí");

            return Ok("Đã cập nhật trạng thái Đã thanh toán");
        }

        // PUT: api/LichThuPhi/quahan/1
        [HttpPut("quahan/{id}")]
        public IActionResult QuaHan(int id)
        {
            var result = _service.QuaHan(id);

            if (!result)
                return BadRequest(
                    "Không tìm thấy lịch thu phí hoặc lịch đã thanh toán");

            return Ok("Đã chuyển lịch thu phí sang trạng thái Quá hạn");
        }

        // PUT: api/LichThuPhi/canhbao/1
        [HttpPut("canhbao/{id}")]
        public IActionResult DanhDauCanhBao(int id)
        {
            var result = _service.DanhDauCanhBao(id);

            if (!result)
                return NotFound("Không tìm thấy lịch thu phí");

            return Ok("Đã đánh dấu cảnh báo");
        }
    }
}