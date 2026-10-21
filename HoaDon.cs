
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuanLyBaoHiem.Models
{
    public partial class HoaDon
    {
        public int MaHdn { get; set; }

        public int MaHd { get; set; }

        public DateOnly NgayLap { get; set; }

        public decimal SoTien { get; set; }

        public string TrangThai { get; set; } = null!;

        public DateTime NgayTao { get; set; }

        public int? NguoiTao { get; set; }

        public DateTime? NgayCapNhat { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? NgayXoa { get; set; }

        [JsonIgnore]
        public virtual ICollection<LichThuPhi> LichThuPhis { get; set; }
            = new List<LichThuPhi>();

        [JsonIgnore]
        public virtual HopDong MaHdNavigation { get; set; } = null!;

        [JsonIgnore]
        public virtual TaiKhoan? NguoiTaoNavigation { get; set; }

        [JsonIgnore]
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
            = new List<ThanhToan>();
    }
}
