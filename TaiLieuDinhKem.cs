namespace QuanLyBaoHiem.Models
{
    public class TaiLieuDinhKem
    {
        public int MaTaiLieu { get; set; }

        public string LoaiThucThe { get; set; } = string.Empty;

        public int MaThucThe { get; set; }

        public string TenFileGoc { get; set; } = string.Empty;

        public string DuongDanFile { get; set; } = string.Empty;

        public string LoaiFile { get; set; } = string.Empty;

        public int? KichThuocByte { get; set; }

        public int NguoiUpload { get; set; }

        public DateTime NgayUpload { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayXoa { get; set; }
    }
}