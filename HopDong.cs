using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class HopDong
{
    public int MaHd { get; set; }

    public int MaKh { get; set; }

    public int MaLoai { get; set; }

    public int MaNv { get; set; }

    public DateOnly NgayBatDau { get; set; }

    public DateOnly NgayKetThuc { get; set; }

    public decimal SoTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public string ChuKyDongPhi { get; set; } = null!;

    public DateTime NgayTao { get; set; }

    public int? NguoiTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? NguoiCapNhat { get; set; }

    public int SoPhienBan { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? NgayXoa { get; set; }

    public int? NguoiXoa { get; set; }

    public virtual ICollection<BoiThuong> BoiThuongs { get; set; } = new List<BoiThuong>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<HopDongLichSu> HopDongLichSus { get; set; } = new List<HopDongLichSu>();

    public virtual ICollection<LichThuPhi> LichThuPhis { get; set; } = new List<LichThuPhi>();

    public virtual KhachHang MaKhNavigation { get; set; } = null!;

    public virtual LoaiBaoHiem MaLoaiNavigation { get; set; } = null!;

    public virtual NhanVien MaNvNavigation { get; set; } = null!;

    public virtual TaiKhoan? NguoiCapNhatNavigation { get; set; }

    public virtual TaiKhoan? NguoiTaoNavigation { get; set; }

    public virtual TaiKhoan? NguoiXoaNavigation { get; set; }
}
