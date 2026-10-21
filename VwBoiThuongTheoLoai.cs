using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class VwBoiThuongTheoLoai
{
    public string TenLoai { get; set; } = null!;

    public int? SoLuongYeuCau { get; set; }

    public decimal? TongDaBoiThuong { get; set; }
}
