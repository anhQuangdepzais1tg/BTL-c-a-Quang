namespace QuanLyBaoHiem.Models
{
    public class RefreshToken
    {
        public int MaRefreshToken { get; set; }

        public int MaTK { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime NgayTao { get; set; }

        public DateTime NgayHetHan { get; set; }

        public bool DaThuHoi { get; set; }

        public string? ThietBi { get; set; }

        public string? DiaChiIP { get; set; }
    }
}