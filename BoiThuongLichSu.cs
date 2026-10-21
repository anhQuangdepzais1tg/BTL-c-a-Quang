using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class BoiThuongLichSu
{
    public int MaLichSu { get; set; }

    public int MaBt { get; set; }

    public int SoPhienBan { get; set; }

    public string DuLieuCu { get; set; } = null!;

    public int? NguoiSua { get; set; }

    public DateTime ThoiGianSua { get; set; }

    public virtual BoiThuong MaBtNavigation { get; set; } = null!;

    public virtual TaiKhoan? NguoiSuaNavigation { get; set; }
}
