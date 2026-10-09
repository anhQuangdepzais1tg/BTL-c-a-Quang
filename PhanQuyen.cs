namespace QuanLyBaoHiem.Models
{
    public class PhanQuyen
    {
        public int MaPhanQuyen { get; set; }

        public int MaQuyen { get; set; }

        public int MaChucNang { get; set; }

        public bool DuocXem { get; set; }

        public bool DuocThem { get; set; }

        public bool DuocSua { get; set; }

        public bool DuocXoa { get; set; }

        public bool DuocXuatFile { get; set; }
    }
}