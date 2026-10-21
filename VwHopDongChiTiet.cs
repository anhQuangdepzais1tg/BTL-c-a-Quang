using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class VwHopDongChiTiet
{
    public int MaHd { get; set; }

    public DateOnly NgayBatDau { get; set; }

    public DateOnly NgayKetThuc { get; set; }

    public decimal SoTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public int MaKh { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public string SdtKhachHang { get; set; } = null!;

    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public int MaNv { get; set; }

    public string TenNhanVien { get; set; } = null!;
}
