using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class VwHoaDonChiTiet
{
    public int MaHdn { get; set; }

    public DateOnly NgayLap { get; set; }

    public decimal SoTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public int MaHd { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public string TenLoai { get; set; } = null!;
}
