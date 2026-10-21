using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class VwBoiThuongChiTiet
{
    public int MaBt { get; set; }

    public DateOnly NgayYeuCau { get; set; }

    public string LyDo { get; set; } = null!;

    public decimal SoTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public int MaHd { get; set; }

    public string TenKhachHang { get; set; } = null!;

    public string TenLoai { get; set; } = null!;

    public string? NhanVienXuLy { get; set; }
}
