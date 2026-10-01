namespace QuanLyBaoHiem.Models
{
    public class TaiKhoan
    {
        public int MaTK { get; set; }

        public string TenDangNhap { get; set; } = string.Empty;

        public string MatKhauHash { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int MaQuyen { get; set; }

        public int SoLanDangNhapSai { get; set; }

        public DateTime? KhoaDenNgay { get; set; }

        public DateTime? LanDangNhapCuoi { get; set; }

        public string? IPDangNhapCuoi { get; set; }

        public bool Bat2FA { get; set; }

        public string? MaBiMat2FA { get; set; }

        public bool KichHoat { get; set; }

        public string? MaXacMinhEmail { get; set; }

        public bool DaXacMinhEmail { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayXoa { get; set; }
    }
}