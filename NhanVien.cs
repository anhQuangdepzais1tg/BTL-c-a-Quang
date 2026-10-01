namespace QuanLyBaoHiem.Models
{
    public class NhanVien
    {
        public int MaNV { get; set; }

        public int? MaTK { get; set; }

        public string HoTen { get; set; } = string.Empty;

        public string SoDienThoai { get; set; } = string.Empty;

        public string? DiaChi { get; set; }

        public string ChucVu { get; set; } = string.Empty;

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