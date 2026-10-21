namespace QuanLyBaoHiem.Models.DTO
{
    public class HopDongCreateDto
    {
        public int MaKh { get; set; }
        public int MaLoai { get; set; }
        public int MaNv { get; set; }

        public DateOnly NgayBatDau { get; set; }
        public DateOnly NgayKetThuc { get; set; }

        public decimal SoTien { get; set; }
        public string ChuKyDongPhi { get; set; } = "Một lần";

        public int? NguoiTao { get; set; }
    }
}
