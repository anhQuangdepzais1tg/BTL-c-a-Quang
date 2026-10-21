
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models;
using System.Security.Claims;
using System.IO;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaiLieuDinhKemController : ControllerBase
    {
        private readonly TaiLieuDinhKemService _service;
        private readonly IWebHostEnvironment _env;

        public TaiLieuDinhKemController(
            TaiLieuDinhKemService service,
            IWebHostEnvironment env)
        {
            _service = service;
            _env = env;
        }

        // GET: api/TaiLieuDinhKem
        [HttpGet]
        public async Task<IActionResult> LayDanhSach()
        {
            var data = await _service.LayDanhSach();
            return Ok(data);
        }

        // GET: api/TaiLieuDinhKem/1
        [HttpGet("{ma:int}")]
        public async Task<IActionResult> LayTheoMa(int ma)
        {
            var data = await _service.LayTheoMa(ma);

            if (data == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy tài liệu."
                });
            }

            return Ok(data);
        }

        // POST: api/TaiLieuDinhKem/upload
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> Upload([FromForm] TaiLieuUploadRequest request)
        {
            if (request == null)
                return BadRequest(new { message = "Dữ liệu gửi lên không hợp lệ." });

            if (request.File == null || request.File.Length == 0)
                return BadRequest(new { message = "Bạn chưa chọn file." });

            if (request.File.Length > 10 * 1024 * 1024)
                return BadRequest(new { message = "File tối đa 10 MB." });

            // Lấy mã tài khoản từ JWT
            var maTK = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(maTK, out int nguoiUpload))
                return Unauthorized(new { message = "Token không có mã tài khoản hợp lệ." });

            string loaiThucThe = (request.LoaiThucThe ?? "").Trim();
            string loaiFile = (request.LoaiFile ?? "").Trim().ToLowerInvariant();

            string tenFileGoc = Path.GetFileName(request.File.FileName);
            string ext = Path.GetExtension(tenFileGoc).ToLowerInvariant();

            // Kiểm tra loại thực thể
            if (loaiThucThe != "HopDong" &&
                loaiThucThe != "BoiThuong" &&
                loaiThucThe != "KhachHang")
            {
                return BadRequest(new
                {
                    message = "Loại thực thể không hợp lệ. Dùng HopDong, BoiThuong hoặc KhachHang."
                });
            }

            // Kiểm tra loại file
            if (loaiFile != "image" &&
                loaiFile != "pdf" &&
                loaiFile != "other")
            {
                return BadRequest(new
                {
                    message = "Loại file không hợp lệ.",
                    loaiFileNhanDuoc = request.LoaiFile,
                    goiY = "LoaiFile phải là image, pdf hoặc other."
                });
            }

            // Đối chiếu loại file với phần mở rộng
            string[] imageExt = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            string[] otherExt = { ".doc", ".docx", ".xls", ".xlsx", ".txt" };

            bool hopLe = loaiFile switch
            {
                "image" => imageExt.Contains(ext),
                "pdf" => ext == ".pdf",
                "other" => otherExt.Contains(ext),
                _ => false
            };

            if (!hopLe)
            {
                return BadRequest(new
                {
                    message = "Phần mở rộng file không phù hợp với loại file.",
                    loaiFileNhanDuoc = loaiFile,
                    duoiFileNhanDuoc = ext
                });
            }

            // Tạo thư mục lưu file
            string webRoot = _env.WebRootPath
                ?? Path.Combine(_env.ContentRootPath, "wwwroot");

            string folder = Path.Combine(webRoot, "uploads", "tailieu");
            Directory.CreateDirectory(folder);

            // Đổi tên file để tránh trùng tên
            string tenFileLuu = $"{Guid.NewGuid():N}{ext}";
            string duongDanVatLy = Path.Combine(folder, tenFileLuu);

            try
            {
                // Lưu file vào máy chủ
                await using (var stream = new FileStream(
                    duongDanVatLy,
                    FileMode.CreateNew,
                    FileAccess.Write))
                {
                    await request.File.CopyToAsync(stream);
                }

                // Lưu thông tin vào SQL Server
                var taiLieu = new TaiLieuDinhKem
                {
                    LoaiThucThe = loaiThucThe,
                    MaThucThe = request.MaThucThe,
                    TenFileGoc = tenFileGoc,
                    DuongDanFile = $"/uploads/tailieu/{tenFileLuu}",
                    LoaiFile = loaiFile,
                    KichThuocByte = (int)request.File.Length,
                    NguoiUpload = nguoiUpload,
                    NgayUpload = DateTime.Now,
                    IsDeleted = false
                };

                await _service.Them(taiLieu);

                return Ok(new
                {
                    message = "Tải tài liệu thành công.",
                    taiLieu
                });
            }
            catch
            {
                // Nếu lưu database thất bại thì xóa file vừa tải lên
                if (System.IO.File.Exists(duongDanVatLy))
                    System.IO.File.Delete(duongDanVatLy);

                throw;
            }
        }

        // DELETE: api/TaiLieuDinhKem/1
        [HttpDelete("{ma:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> XoaMem(int ma)
        {
            var ketQua = await _service.XoaMem(ma);

            if (!ketQua)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy tài liệu."
                });
            }

            return Ok(new
            {
                message = "Xóa tài liệu thành công."
            });
        }
    }

    // Dữ liệu nhận từ multipart/form-data
    public class TaiLieuUploadRequest
    {
        [FromForm(Name = "LoaiThucThe")]
        public string LoaiThucThe { get; set; } = string.Empty;

        [FromForm(Name = "MaThucThe")]
        public int MaThucThe { get; set; }

        [FromForm(Name = "LoaiFile")]
        public string LoaiFile { get; set; } = string.Empty;

        [FromForm(Name = "File")]
        public IFormFile? File { get; set; }
    }
}
