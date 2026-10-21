using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class HangDoiTacVu
{
    public long MaJob { get; set; }

    public string LoaiJob { get; set; } = null!;

    public string? DuLieuJob { get; set; }

    public string TrangThai { get; set; } = null!;

    public int SoLanThu { get; set; }

    public string? LoiNeuCo { get; set; }

    public DateTime NgayTao { get; set; }

    public DateTime? NgayXuLy { get; set; }

    public DateTime? NgayHoanThanh { get; set; }
}
