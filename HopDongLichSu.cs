using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class HopDongLichSu
{
    public int MaLichSu { get; set; }

    public int MaHd { get; set; }

    public int SoPhienBan { get; set; }

    public string DuLieuCu { get; set; } = null!;

    public int? NguoiSua { get; set; }

    public DateTime ThoiGianSua { get; set; }

    public virtual HopDong MaHdNavigation { get; set; } = null!;

    public virtual TaiKhoan? NguoiSuaNavigation { get; set; }
}
