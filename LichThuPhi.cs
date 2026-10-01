namespace QuanLyBaoHiem.Models
{
    public class LichThuPhi
    {
        public int MaLichThu { get; set; }

        public int MaHD { get; set; }

        public int KyThu { get; set; }

        public DateTime NgayDenHan { get; set; }

        public decimal SoTien { get; set; }

        public string TrangThai { get; set; } = string.Empty;

        public int? MaHDN { get; set; }

        public bool DaCanhBao { get; set; }

        public DateTime NgayTao { get; set; }
    }
}