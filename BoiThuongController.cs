using QuanLyBaoHiem.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoiThuongController : ControllerBase
    {
        private readonly BoiThuongService _service;

        public BoiThuongController(BoiThuongService service)
        {
            _service = service;
        }

        // 1. Lấy danh sách bồi thường
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var danhSach = await _service.GetAll();
            return Ok(danhSach);
        }

        // 2. Lấy bồi thường theo mã
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var duLieu = await _service.GetById(id);

            if (duLieu == null)
                return NotFound(new { message = "Không tìm thấy yêu cầu bồi thường." });

            return Ok(duLieu);
        }

        // 3. Thêm yêu cầu bồi thường

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] BoiThuongCreateDto duLieu)
        {
            var boiThuong = new BoiThuong
            {
                MaHd = duLieu.MaHd,
                MaNv = duLieu.MaNv,
                NgayYeuCau = duLieu.NgayYeuCau,
                LyDo = duLieu.LyDo,
                SoTien = duLieu.SoTien,
                TrangThai = duLieu.TrangThai,
                NguoiTao = duLieu.NguoiTao
            };

            var ketQua = await _service.Create(boiThuong);

            if (!ketQua.ThanhCong)
                return BadRequest(new { message = ketQua.ThongBao });

            return Ok(new { message = ketQua.ThongBao });
        }


        // 4. Cập nhật yêu cầu bồi thường

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, [FromBody] BoiThuongCreateDto duLieu)
        {
            var boiThuong = new BoiThuong
            {
                MaHd = duLieu.MaHd,
                MaNv = duLieu.MaNv,
                NgayYeuCau = duLieu.NgayYeuCau,
                LyDo = duLieu.LyDo,
                SoTien = duLieu.SoTien,
                TrangThai = duLieu.TrangThai
            };

            var ketQua = await _service.Update(id, boiThuong);

            if (!ketQua.ThanhCong)
            {
                if (ketQua.ThongBao == "Không tìm thấy yêu cầu bồi thường.")
                    return NotFound(new { message = ketQua.ThongBao });

                return BadRequest(new { message = ketQua.ThongBao });
            }

            return Ok(new { message = ketQua.ThongBao });
        }


        // 5. Xóa mềm yêu cầu bồi thường
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ketQua = await _service.Delete(id);

            if (!ketQua.ThanhCong)
                return NotFound(new { message = ketQua.ThongBao });

            return Ok(new { message = ketQua.ThongBao });
        }
    }
}
