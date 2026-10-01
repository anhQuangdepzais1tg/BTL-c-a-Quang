namespace QuanLyBaoHiem.Models
{
    public class HoaDon
    {
        public int MaHDN { get; set; }

        public int MaHD { get; set; }

        public DateTime NgayLap { get; set; }

        public decimal SoTien { get; set; }

        public string TrangThai { get; set; } = string.Empty;

        public DateTime NgayTao { get; set; }

        public int? NguoiTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayXoa { get; set; }
    }
}