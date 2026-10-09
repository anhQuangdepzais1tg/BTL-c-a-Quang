namespace QuanLyBaoHiem.Models
{
    public class LoginResponse
    {
        public string Token { get; set; } = "";

        public string RefreshToken { get; set; } = "";

        public int MaTK { get; set; }

        public string TenDangNhap { get; set; } = "";

        public string Email { get; set; } = "";

        public int MaQuyen { get; set; }

        public string TenQuyen { get; set; } = "";
    }
}