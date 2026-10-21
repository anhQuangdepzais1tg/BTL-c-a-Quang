using System;
using System.Collections.Generic;

namespace QuanLyBaoHiem.Models;

public partial class TaiLieuDinhKem
{
    public int MaTaiLieu { get; set; }

    public string LoaiThucThe { get; set; } = null!;

    public int MaThucThe { get; set; }

    public string TenFileGoc { get; set; } = null!;

    public string DuongDanFile { get; set; } = null!;

    public string LoaiFile { get; set; } = null!;

    public int? KichThuocByte { get; set; }

    public int NguoiUpload { get; set; }

    public DateTime NgayUpload { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? NgayXoa { get; set; }

    public virtual TaiKhoan NguoiUploadNavigation { get; set; } = null!;
}
