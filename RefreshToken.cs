using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class RefreshToken
{
    public int MaRefreshToken { get; set; }

    public int MaTk { get; set; }

    public string Token { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public DateTime NgayHetHan { get; set; }

    public bool DaThuHoi { get; set; }

    public string? ThietBi { get; set; }

    public string? DiaChiIp { get; set; }

    public virtual TaiKhoan MaTkNavigation { get; set; } = null!;
}
