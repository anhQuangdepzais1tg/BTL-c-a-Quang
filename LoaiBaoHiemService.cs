using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class LoaiBaoHiemService
    {
        private readonly LoaiBaoHiemRepository _repository;

        public LoaiBaoHiemService(
            LoaiBaoHiemRepository repository)
        {
            _repository = repository;
        }

        public List<LoaiBaoHiem> GetAll()
        {
            return _repository.GetAll();
        }

        public LoaiBaoHiem? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(LoaiBaoHiem loaiBaoHiem)
        {
            _repository.Add(loaiBaoHiem);
        }

        public void Update(LoaiBaoHiem loaiBaoHiem)
        {
            _repository.Update(loaiBaoHiem);
        }

        // Xóa mềm
        public bool Delete(int id)
        {
            return _repository.DeleteSoft(id);
        }
    }
}