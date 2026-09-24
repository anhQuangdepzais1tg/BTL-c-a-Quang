namespace QuanLyBaoHiem.Models
{
    public class HopDong
    {
        public int MaHD { get; set; }

        public int MaKH { get; set; }

        public int MaLoai { get; set; }

        public int MaNV { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public decimal SoTien { get; set; }

        public string TrangThai { get; set; }
    }
}