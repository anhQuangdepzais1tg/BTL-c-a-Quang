namespace QuanLyBaoHiem.Models
{
    public class HangDoiTacVu
    {
        public long MaJob { get; set; }

        public string LoaiJob { get; set; } = string.Empty;

        public string? DuLieuJob { get; set; }

        public string TrangThai { get; set; } = string.Empty;

        public int SoLanThu { get; set; }

        public string? LoiNeuCo { get; set; }

        public DateTime NgayTao { get; set; }

        public DateTime? NgayXuLy { get; set; }

        public DateTime? NgayHoanThanh { get; set; }
    }
}