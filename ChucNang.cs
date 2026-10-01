namespace QuanLyBaoHiem.Models
{
    public class ChucNang
    {
        public int MaChucNang { get; set; }

        public string TenChucNang { get; set; } = string.Empty;

        public int? MaChucNangCha { get; set; }

        public string? DuongDan { get; set; }

        public bool IsDeleted { get; set; }
    }
}