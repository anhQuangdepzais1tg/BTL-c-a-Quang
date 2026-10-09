using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class ThanhToanService
    {
        private readonly ThanhToanRepository _repo;

        public ThanhToanService(ThanhToanRepository repo)
        {
            _repo = repo;
        }

        public List<ThanhToan> GetAll()
        {
            return _repo.GetAll();
        }

        public ThanhToan? GetById(int id)
        {
            return _repo.GetById(id);
        }

        public List<ThanhToan> GetByHoaDon(int id)
        {
            return _repo.GetByHoaDon(id);
        }

        public bool Add(ThanhToan item)
        {
            return _repo.Add(item);
        }

        public bool Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}