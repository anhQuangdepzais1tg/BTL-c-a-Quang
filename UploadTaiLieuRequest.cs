using Microsoft.AspNetCore.Http;

namespace QuanLyBaoHiem.Models
{
    public class UploadTaiLieuRequest
    {
        public int MaThucThe { get; set; }

        public string LoaiThucThe { get; set; } = string.Empty;

        public IFormFile File { get; set; } = default!;
    }
}