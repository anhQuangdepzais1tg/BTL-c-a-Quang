namespace QuanLyBaoHiem.Models
{
    public class BoiThuong_LichSu
    {
        public int MaLichSu { get; set; }

        public int MaBT { get; set; }

        public int SoPhienBan { get; set; }

        public string DuLieuCu { get; set; } = string.Empty;

        public int? NguoiSua { get; set; }

        public DateTime ThoiGianSua { get; set; }
    }
}