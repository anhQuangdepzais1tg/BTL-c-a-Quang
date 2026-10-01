namespace QuanLyBaoHiem.Models
{
    public class BoiThuong
    {
        public int MaBT { get; set; }

        public int MaHD { get; set; }

        public int? MaNV { get; set; }

        public DateTime NgayYeuCau { get; set; }

        public string LyDo { get; set; } = string.Empty;

        public decimal SoTien { get; set; }

        public string TrangThai { get; set; } = string.Empty;

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