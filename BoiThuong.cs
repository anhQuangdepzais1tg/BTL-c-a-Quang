using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class BoiThuong
{
    public int MaBt { get; set; }

    public int MaHd { get; set; }

    public int? MaNv { get; set; }

    public DateOnly NgayYeuCau { get; set; }

    public string LyDo { get; set; } = null!;

    public decimal SoTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public int? NguoiTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? NguoiCapNhat { get; set; }

    public int SoPhienBan { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? NgayXoa { get; set; }

    public int? NguoiXoa { get; set; }

    public virtual ICollection<BoiThuongLichSu> BoiThuongLichSus { get; set; } = new List<BoiThuongLichSu>();

    public virtual HopDong MaHdNavigation { get; set; } = null!;

    public virtual NhanVien? MaNvNavigation { get; set; }

    public virtual TaiKhoan? NguoiCapNhatNavigation { get; set; }

    public virtual TaiKhoan? NguoiTaoNavigation { get; set; }

    public virtual TaiKhoan? NguoiXoaNavigation { get; set; }
}
