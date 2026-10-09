using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class ThongBaoService
    {
        private readonly ThongBaoRepository _repo;

        public ThongBaoService(
            ThongBaoRepository repo)
        {
            _repo = repo;
        }

        public List<ThongBao> Get(
            int maTK,
            bool chuaDoc)
        {
            return _repo.GetByTaiKhoan(
                maTK,
                chuaDoc);
        }

        public ThongBao? GetById(
            int id,
            int maTK)
        {
            return _repo.GetById(id, maTK);
        }

        public ThongBao Add(ThongBao item)
        {
            return _repo.Add(item);
        }

        public bool Read(int id, int maTK)
        {
            return _repo.Read(id, maTK);
        }

        public bool Delete(int id, int maTK)
        {
            return _repo.Delete(id, maTK);
        }
    }
}