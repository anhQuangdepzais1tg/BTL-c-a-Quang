namespace QuanLyBaoHiem.Models
{
    public class Quyen
    {
        public int MaQuyen { get; set; }

        public string TenQuyen { get; set; } = string.Empty;

        public string? MoTa { get; set; }

        public bool IsDeleted { get; set; }
    }
}