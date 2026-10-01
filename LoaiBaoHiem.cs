namespace QuanLyBaoHiem.Models
{
    public class LoaiBaoHiem
    {
        public int MaLoai { get; set; }

        public string TenLoai { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        public decimal MucPhi { get; set; }

        public DateTime NgayTao { get; set; }

        public int? NguoiTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public int? NguoiCapNhat { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayXoa { get; set; }
    }
}