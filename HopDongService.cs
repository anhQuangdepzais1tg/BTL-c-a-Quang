using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class HopDongService
    {
        private readonly HopDongRepository _repository;

        public HopDongService(HopDongRepository repository)
        {
            _repository = repository;
        }

        public List<HopDong> GetAll()
        {
            return _repository.GetAll();
        }

        public HopDong? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void Add(HopDong hopDong)
        {
            _repository.Add(hopDong);
        }

        public void Update(HopDong hopDong)
        {
            _repository.Update(hopDong);
        }

        public bool DeleteSoft(int id, int? nguoiXoa)
        {
            return _repository.DeleteSoft(id, nguoiXoa);
        }

        public bool GiaHan(
            int id,
            DateTime ngayKetThucMoi,
            int? nguoiCapNhat)
        {
            return _repository.GiaHan(
                id,
                ngayKetThucMoi,
                nguoiCapNhat);
        }

        public bool Huy(
            int id,
            int? nguoiCapNhat)
        {
            return _repository.Huy(
                id,
                nguoiCapNhat);
        }
    }
}