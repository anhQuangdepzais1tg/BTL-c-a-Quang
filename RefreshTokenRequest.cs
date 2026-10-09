namespace QuanLyBaoHiem.Models
{
    // Dùng khi lấy JWT mới bằng Refresh Token
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}