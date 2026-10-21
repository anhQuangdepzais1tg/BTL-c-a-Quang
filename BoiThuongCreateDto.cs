
namespace QuanLyBaoHiem.Models.DTO
{
    public class BoiThuongCreateDto
    {
        public int MaHd { get; set; }

        public int? MaNv { get; set; }

        public DateOnly NgayYeuCau { get; set; }

        public string LyDo { get; set; } = string.Empty;

        public decimal SoTien { get; set; }

        public string TrangThai { get; set; } = "Đang xử lý";

        public int? NguoiTao { get; set; }
    }
}
