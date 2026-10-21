using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class NhanVien
{
    public int MaNv { get; set; }

    public int? MaTk { get; set; }

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string ChucVu { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public int? NguoiTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? NguoiCapNhat { get; set; }

    public int SoPhienBan { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? NgayXoa { get; set; }

    public int? NguoiXoa { get; set; }

    public virtual ICollection<BoiThuong> BoiThuongs { get; set; } = new List<BoiThuong>();

    public virtual ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();

    public virtual TaiKhoan? MaTkNavigation { get; set; }

    public virtual TaiKhoan? NguoiCapNhatNavigation { get; set; }

    public virtual TaiKhoan? NguoiTaoNavigation { get; set; }

    public virtual TaiKhoan? NguoiXoaNavigation { get; set; }
}
