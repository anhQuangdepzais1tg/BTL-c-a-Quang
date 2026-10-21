using System.ComponentModel.DataAnnotations;

namespace QuanLyBaoHiem.Models.DTO
{
    public class HoaDonCreateDto
    {
        [Required]
        public int MaHd { get; set; }

        public DateOnly NgayLap { get; set; }

        [Range(0.01, double.MaxValue,
            ErrorMessage = "Số tiền phải lớn hơn 0.")]
        public decimal SoTien { get; set; }

        [Required]
        public string TrangThai { get; set; } = string.Empty;

        public int? NguoiTao { get; set; }
    }
}
