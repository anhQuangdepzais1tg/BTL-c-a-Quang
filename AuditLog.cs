namespace QuanLyBaoHiem.Models
{
    public class AuditLog
    {
        public long MaLog { get; set; }

        public string TenBang { get; set; } = string.Empty;

        public int MaBanGhi { get; set; }

        public string HanhDong { get; set; } = string.Empty;

        public string? DuLieuCu { get; set; }

        public string? DuLieuMoi { get; set; }

        public int? MaTK { get; set; }

        public string? DiaChiIP { get; set; }

        public DateTime ThoiGian { get; set; }
    }
}