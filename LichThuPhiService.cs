using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class LichThuPhiService
    {
        private readonly LichThuPhiRepository _repository;

        public LichThuPhiService(LichThuPhiRepository repository)
        {
            _repository = repository;
        }

        public List<LichThuPhi> GetAll()
        {
            return _repository.GetAll();
        }

        public LichThuPhi? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public List<LichThuPhi> GetByHopDong(int maHD)
        {
            return _repository.GetByHopDong(maHD);
        }

        public bool Add(LichThuPhi lich)
        {
            return _repository.Add(lich);
        }

        public bool Update(int id, LichThuPhi lich)
        {
            return _repository.Update(id, lich);
        }

        public bool DaThanhToan(int id)
        {
            return _repository.DaThanhToan(id);
        }

        public bool QuaHan(int id)
        {
            return _repository.QuaHan(id);
        }

        public bool DanhDauCanhBao(int id)
        {
            return _repository.DanhDauCanhBao(id);
        }
    }
}