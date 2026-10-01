namespace QuanLyBaoHiem.Models
{
    public class RegisterRequest
    {
        public string TenDangNhap { get; set; } = string.Empty;

        public string MatKhau { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}