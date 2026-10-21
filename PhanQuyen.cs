using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class PhanQuyen
{
    public int MaPhanQuyen { get; set; }

    public int MaQuyen { get; set; }

    public int MaChucNang { get; set; }

    public bool DuocXem { get; set; }

    public bool DuocThem { get; set; }

    public bool DuocSua { get; set; }

    public bool DuocXoa { get; set; }

    public bool DuocXuatFile { get; set; }

    public virtual ChucNang MaChucNangNavigation { get; set; } = null!;

    public virtual Quyen MaQuyenNavigation { get; set; } = null!;
}
