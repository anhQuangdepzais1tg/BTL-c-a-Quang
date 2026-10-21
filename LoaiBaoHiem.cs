using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class LoaiBaoHiem
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal MucPhi { get; set; }

    public DateTime NgayTao { get; set; }

    public int? NguoiTao { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public int? NguoiCapNhat { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? NgayXoa { get; set; }

    public virtual ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();

    public virtual TaiKhoan? NguoiCapNhatNavigation { get; set; }

    public virtual TaiKhoan? NguoiTaoNavigation { get; set; }
}
