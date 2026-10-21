using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class PasswordResetToken
{
    public int MaToken { get; set; }

    public int MaTk { get; set; }

    public string Token { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public DateTime NgayHetHan { get; set; }

    public bool DaSuDung { get; set; }

    public virtual TaiKhoan MaTkNavigation { get; set; } = null!;
}
