using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class HoaDonService
    {
        private readonly HoaDonRepository _repository;

        public HoaDonService(HoaDonRepository repository)
        {
            _repository = repository;
        }

        public List<HoaDon> GetAll()
        {
            return _repository.GetAll();
        }

        public HoaDon? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public bool Add(HoaDon hoaDon)
        {
            return _repository.Add(hoaDon);
        }

        public bool Update(int id, HoaDon hoaDon)
        {
            return _repository.Update(id, hoaDon);
        }

        public bool QuaHan(int id)
        {
            return _repository.QuaHan(id);
        }

        public bool DeleteSoft(int id)
        {
            return _repository.DeleteSoft(id);
        }
    }
}