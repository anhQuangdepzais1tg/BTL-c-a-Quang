using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class LichThuPhi
{
    public int MaLichThu { get; set; }

    public int MaHd { get; set; }

    public int KyThu { get; set; }

    public DateOnly NgayDenHan { get; set; }

    public decimal SoTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public int? MaHdn { get; set; }

    public bool DaCanhBao { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual HopDong MaHdNavigation { get; set; } = null!;

    public virtual HoaDon? MaHdnNavigation { get; set; }
}
