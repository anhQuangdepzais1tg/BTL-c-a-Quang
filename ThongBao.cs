namespace QuanLyBaoHiem.Models
{
    public class ThongBao
    {
        public int MaTB { get; set; }

        public int MaTK { get; set; }

        public string TieuDe { get; set; } = string.Empty;

        public string NoiDung { get; set; } = string.Empty;

        public string Loai { get; set; } = string.Empty;

        public bool DaDoc { get; set; }

        public bool DaGui { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime? NgayGui { get; set; }
    }
}