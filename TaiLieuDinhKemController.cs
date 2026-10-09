using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;
using System.Security.Claims;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaiLieuDinhKemController : ControllerBase
    {
        private readonly TaiLieuDinhKemService _service;

        public TaiLieuDinhKemController(
            TaiLieuDinhKemService service)
        {
            _service = service;
        }

        // =====================================================
        // 1. LẤY DANH SÁCH TÀI LIỆU
        // =====================================================
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAll());
        }

        // =====================================================
        // 2. LẤY TÀI LIỆU THEO ID
        // =====================================================
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var data = _service.GetById(id);

            if (data == null)
            {
                return NotFound("Không tìm thấy tài liệu");
            }

            return Ok(data);
        }

        // =====================================================
        // 3. UPLOAD FILE
        // =====================================================
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            [FromForm] UploadTaiLieuRequest request)
        {
            try
            {
                // Kiểm tra file
                if (request.File == null ||
                    request.File.Length == 0)
                {
                    return BadRequest(
                        "Vui lòng chọn file cần upload");
                }

                // Lấy mã tài khoản từ JWT
                var claim = User.FindFirst(
                    ClaimTypes.NameIdentifier);

                if (claim == null)
                {
                    return Unauthorized(
                        "Không xác định được tài khoản");
                }

                // Chuyển mã tài khoản từ JWT sang int
                int nguoiUpload;

                if (!int.TryParse(
                    claim.Value,
                    out nguoiUpload))
                {
                    return Unauthorized(
                        "Mã tài khoản không hợp lệ");
                }

                // Gọi Service upload
                var result = await _service.Upload(
                    request.MaThucThe,
                    request.LoaiThucThe,
                    request.File,
                    nguoiUpload);

                return Ok(new
                {
                    message = "Upload tài liệu thành công",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =====================================================
        // 4. XÓA MỀM TÀI LIỆU
        // =====================================================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);

            if (!result)
            {
                return NotFound(
                    "Không tìm thấy tài liệu");
            }

            return Ok(
                "Xóa tài liệu thành công");
        }
    }
}