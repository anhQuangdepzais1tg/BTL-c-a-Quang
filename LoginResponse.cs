namespace QuanLyBaoHiem.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;

        public int MaTK { get; set; }

        public string TenDangNhap { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int MaQuyen { get; set; }

        public string TenQuyen { get; set; } = string.Empty;
    }
}