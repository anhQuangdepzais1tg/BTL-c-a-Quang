using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class RbacService
    {
        private readonly RbacRepository _repo;

        public RbacService(RbacRepository repo)
        {
            _repo = repo;
        }

        public List<Quyen> QuyenAll()
            => _repo.QuyenAll();

        public Quyen? QuyenGet(int id)
            => _repo.QuyenGet(id);

        public void QuyenAdd(Quyen x)
            => _repo.QuyenAdd(x);

        public bool QuyenUpdate(
            int id,
            Quyen x)
            => _repo.QuyenUpdate(id, x);

        public bool QuyenDelete(int id)
            => _repo.QuyenDelete(id);

        public List<ChucNang> ChucNangAll()
            => _repo.ChucNangAll();

        public ChucNang? ChucNangGet(int id)
            => _repo.ChucNangGet(id);

        public void ChucNangAdd(ChucNang x)
            => _repo.ChucNangAdd(x);

        public bool ChucNangUpdate(
            int id,
            ChucNang x)
            => _repo.ChucNangUpdate(id, x);

        public bool ChucNangDelete(int id)
            => _repo.ChucNangDelete(id);

        public List<PhanQuyen> PhanQuyenAll()
            => _repo.PhanQuyenAll();

        public PhanQuyen? PhanQuyenGet(int id)
            => _repo.PhanQuyenGet(id);

        public bool PhanQuyenAdd(PhanQuyen x)
            => _repo.PhanQuyenAdd(x);

        public bool PhanQuyenUpdate(
            int id,
            PhanQuyen x)
            => _repo.PhanQuyenUpdate(id, x);

        public bool PhanQuyenDelete(int id)
            => _repo.PhanQuyenDelete(id);
    }
}