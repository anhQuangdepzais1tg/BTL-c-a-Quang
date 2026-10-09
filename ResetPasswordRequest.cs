namespace QuanLyBaoHiem.Models
{
    public class ResetPasswordRequest
    {
        public string Token { get; set; } = "";

        public string MatKhauMoi { get; set; } = "";
    }
}