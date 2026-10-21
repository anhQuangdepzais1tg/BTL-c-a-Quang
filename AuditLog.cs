using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class AuditLog
{
    public long MaLog { get; set; }

    public string TenBang { get; set; } = null!;

    public int MaBanGhi { get; set; }

    public string HanhDong { get; set; } = null!;

    public string? DuLieuCu { get; set; }

    public string? DuLieuMoi { get; set; }

    public int? MaTk { get; set; }

    public string? DiaChiIp { get; set; }

    public DateTime ThoiGian { get; set; }

    public virtual TaiKhoan? MaTkNavigation { get; set; }
}
