using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class ThongBao
{
    public int MaTb { get; set; }

    public int MaTk { get; set; }

    public string TieuDe { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public string Loai { get; set; } = null!;

    public bool DaDoc { get; set; }

    public bool DaGui { get; set; }

    public DateTime NgayTao { get; set; }

    public DateTime? NgayGui { get; set; }

    public virtual TaiKhoan MaTkNavigation { get; set; } = null!;
}
