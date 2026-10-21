
namespace QuanLyBaoHiem.Models.DTO
{
    // Dữ liệu nhận khi tạo hoặc cập nhật khách hàng
    public class KhachHangDto
    {
        public int? MaTk { get; set; }
        public string HoTen { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string? DiaChi { get; set; }
        public string Cccd { get; set; } = string.Empty;
        public DateOnly? NgaySinh { get; set; }
    }
}
