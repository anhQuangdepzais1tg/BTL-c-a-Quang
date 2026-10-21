using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class TaiKhoan
{
    public int MaTk { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int MaQuyen { get; set; }

    public int SoLanDangNhapSai { get; set; }

    public DateTime? KhoaDenNgay { get; set; }

    public DateTime? LanDangNhapCuoi { get; set; }

    public string? IpdangNhapCuoi { get; set; }

    public bool Bat2Fa { get; set; }

    public string? MaBiMat2Fa { get; set; }

    public bool KichHoat { get; set; }

    public string? MaXacMinhEmail { get; set; }

    public bool DaXacMinhEmail { get; set; }

    public DateTime NgayTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? NgayXoa { get; set; }

    public virtual ICollection<BoiThuongLichSu> BoiThuongLichSus { get; set; } = new List<BoiThuongLichSu>();

    public virtual ICollection<BoiThuong> BoiThuongNguoiCapNhatNavigations { get; set; } = new List<BoiThuong>();

    public virtual ICollection<BoiThuong> BoiThuongNguoiTaoNavigations { get; set; } = new List<BoiThuong>();

    public virtual ICollection<BoiThuong> BoiThuongNguoiXoaNavigations { get; set; } = new List<BoiThuong>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<HopDongLichSu> HopDongLichSus { get; set; } = new List<HopDongLichSu>();

    public virtual ICollection<HopDong> HopDongNguoiCapNhatNavigations { get; set; } = new List<HopDong>();

    public virtual ICollection<HopDong> HopDongNguoiTaoNavigations { get; set; } = new List<HopDong>();

    public virtual ICollection<HopDong> HopDongNguoiXoaNavigations { get; set; } = new List<HopDong>();

    public virtual KhachHang? KhachHangMaTkNavigation { get; set; }

    public virtual ICollection<KhachHang> KhachHangNguoiCapNhatNavigations { get; set; } = new List<KhachHang>();

    public virtual ICollection<KhachHang> KhachHangNguoiTaoNavigations { get; set; } = new List<KhachHang>();

    public virtual ICollection<LoaiBaoHiem> LoaiBaoHiemNguoiCapNhatNavigations { get; set; } = new List<LoaiBaoHiem>();

    public virtual ICollection<LoaiBaoHiem> LoaiBaoHiemNguoiTaoNavigations { get; set; } = new List<LoaiBaoHiem>();

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;

    public virtual NhanVien? NhanVienMaTkNavigation { get; set; }

    public virtual ICollection<NhanVien> NhanVienNguoiCapNhatNavigations { get; set; } = new List<NhanVien>();

    public virtual ICollection<NhanVien> NhanVienNguoiTaoNavigations { get; set; } = new List<NhanVien>();

    public virtual ICollection<NhanVien> NhanVienNguoiXoaNavigations { get; set; } = new List<NhanVien>();

    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual ICollection<TaiLieuDinhKem> TaiLieuDinhKems { get; set; } = new List<TaiLieuDinhKem>();

    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();

    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();
}
