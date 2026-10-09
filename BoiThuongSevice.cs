using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class BoiThuongService
    {
        private readonly BoiThuongRepository _repository;

        public BoiThuongService(BoiThuongRepository repository)
        {
            _repository = repository;
        }

        public List<BoiThuong> GetAll()
        {
            return _repository.GetAll();
        }

        public BoiThuong? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public bool Add(BoiThuong boiThuong)
        {
            return _repository.Add(boiThuong);
        }

        public bool Duyet(
            int id,
            int maNVXuLy,
            int? nguoiCapNhat)
        {
            return _repository.Duyet(
                id,
                maNVXuLy,
                nguoiCapNhat);
        }

        public bool TuChoi(
            int id,
            int maNVXuLy,
            int? nguoiCapNhat)
        {
            return _repository.TuChoi(
                id,
                maNVXuLy,
                nguoiCapNhat);
        }

        public bool DeleteSoft(int id, int? nguoiXoa)
        {
            return _repository.DeleteSoft(id, nguoiXoa);
        }
    }
}