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

        public string TrangThai { get; set; } = string.Empty;

        public string ChuKyDongPhi { get; set; } = string.Empty;

        public DateTime NgayTao { get; set; }

        public int? NguoiTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public int? NguoiCapNhat { get; set; }

        public int SoPhienBan { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayXoa { get; set; }

        public int? NguoiXoa { get; set; }
    }
}