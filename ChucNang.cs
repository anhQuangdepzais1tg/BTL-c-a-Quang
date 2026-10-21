using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class ChucNang
{
    public int MaChucNang { get; set; }

    public string TenChucNang { get; set; } = null!;

    public int? MaChucNangCha { get; set; }

    public string? DuongDan { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<PhanQuyen> PhanQuyens { get; set; } = new List<PhanQuyen>();
}
