using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class ThanhToan
{
    public int MaTt { get; set; }

    public int MaHdn { get; set; }

    public DateOnly NgayThanhToan { get; set; }

    public decimal SoTien { get; set; }

    public string PhuongThuc { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public int? NguoiTao { get; set; }

    public bool IsDeleted { get; set; }

    public virtual HoaDon MaHdnNavigation { get; set; } = null!;

    public virtual TaiKhoan? NguoiTaoNavigation { get; set; }
}
