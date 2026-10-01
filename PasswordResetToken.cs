namespace QuanLyBaoHiem.Models
{
    public class PasswordResetToken
    {
        public int MaToken { get; set; }

        public int MaTK { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime NgayTao { get; set; }

        public DateTime NgayHetHan { get; set; }

        public bool DaSuDung { get; set; }
    }
}