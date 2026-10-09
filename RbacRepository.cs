using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class RbacRepository
    {
        private readonly QuanLyBaoHiemContext _db;

        public RbacRepository(
            QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // ================= QUYỀN =================

        public List<Quyen> QuyenAll()
        {
            return _db.Quyen
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.MaQuyen)
                .ToList();
        }

        public Quyen? QuyenGet(int id)
        {
            return _db.Quyen.FirstOrDefault(
                x => x.MaQuyen == id &&
                     !x.IsDeleted);
        }

        public void QuyenAdd(Quyen x)
        {
            x.IsDeleted = false;

            _db.Quyen.Add(x);
            _db.SaveChanges();
        }

        public bool QuyenUpdate(
            int id,
            Quyen x)
        {
            var data = QuyenGet(id);

            if (data == null)
                return false;

            data.TenQuyen = x.TenQuyen;
            data.MoTa = x.MoTa;

            _db.SaveChanges();

            return true;
        }

        public bool QuyenDelete(int id)
        {
            var data = QuyenGet(id);

            if (data == null)
                return false;

            data.IsDeleted = true;

            _db.SaveChanges();

            return true;
        }

        // ================= CHỨC NĂNG =================

        public List<ChucNang> ChucNangAll()
        {
            return _db.ChucNang
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.MaChucNang)
                .ToList();
        }

        public ChucNang? ChucNangGet(int id)
        {
            return _db.ChucNang.FirstOrDefault(
                x => x.MaChucNang == id &&
                     !x.IsDeleted);
        }

        public void ChucNangAdd(ChucNang x)
        {
            x.IsDeleted = false;

            _db.ChucNang.Add(x);
            _db.SaveChanges();
        }

        public bool ChucNangUpdate(
            int id,
            ChucNang x)
        {
            var data = ChucNangGet(id);

            if (data == null)
                return false;

            data.TenChucNang = x.TenChucNang;
            data.MaChucNangCha = x.MaChucNangCha;
            data.DuongDan = x.DuongDan;

            _db.SaveChanges();

            return true;
        }

        public bool ChucNangDelete(int id)
        {
            var data = ChucNangGet(id);

            if (data == null)
                return false;

            data.IsDeleted = true;

            _db.SaveChanges();

            return true;
        }

        // ================= PHÂN QUYỀN =================

        public List<PhanQuyen> PhanQuyenAll()
        {
            return _db.PhanQuyen.ToList();
        }

        public PhanQuyen? PhanQuyenGet(int id)
        {
            return _db.PhanQuyen.Find(id);
        }

        public bool PhanQuyenAdd(
            PhanQuyen x)
        {
            var quyen = _db.Quyen.Any(
                q => q.MaQuyen == x.MaQuyen &&
                     !q.IsDeleted);

            var chucNang = _db.ChucNang.Any(
                c => c.MaChucNang == x.MaChucNang &&
                     !c.IsDeleted);

            if (!quyen || !chucNang)
                return false;

            _db.PhanQuyen.Add(x);
            _db.SaveChanges();

            return true;
        }

        public bool PhanQuyenUpdate(
            int id,
            PhanQuyen x)
        {
            var data = PhanQuyenGet(id);

            if (data == null)
                return false;

            data.MaQuyen = x.MaQuyen;
            data.MaChucNang = x.MaChucNang;
            data.DuocXem = x.DuocXem;
            data.DuocThem = x.DuocThem;
            data.DuocSua = x.DuocSua;
            data.DuocXoa = x.DuocXoa;
            data.DuocXuatFile = x.DuocXuatFile;

            _db.SaveChanges();

            return true;
        }

        public bool PhanQuyenDelete(int id)
        {
            var data = PhanQuyenGet(id);

            if (data == null)
                return false;

            _db.PhanQuyen.Remove(data);
            _db.SaveChanges();

            return true;
        }
    }
}