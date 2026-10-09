namespace QuanLyBaoHiem.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int MaTK { get; set; }

        public string Token { get; set; } = "";

        public DateTime NgayTao { get; set; }

        public DateTime NgayHetHan { get; set; }

        public bool DaThuHoi { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }
    }
}