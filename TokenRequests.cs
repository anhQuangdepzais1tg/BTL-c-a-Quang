
namespace QuanLyBaoHiem.Models.DTO
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; } = "";
    }

    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = "";
    }

    public class ResetPasswordRequest
    {
        public string Token { get; set; } = "";
        public string MatKhauMoi { get; set; } = "";
    }
}
