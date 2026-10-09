using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class NhanVienService
    {
        private readonly NhanVienRepository _repository;

        public NhanVienService(NhanVienRepository repository)
        {
            _repository = repository;
        }

        // Lấy danh sách nhân viên
        public List<NhanVien> GetAll()
        {
            return _repository.GetAll();
        }

        // Lấy nhân viên theo mã
        public NhanVien? GetById(int id)
        {
            return _repository.GetById(id);
        }

        // Thêm nhân viên
        public bool Add(NhanVien nhanVien)
        {
            return _repository.Add(nhanVien);
        }

        // Sửa nhân viên
        public bool Update(NhanVien nhanVien)
        {
            return _repository.Update(nhanVien);
        }

        // Xóa mềm
        public bool DeleteSoft(int id, int? nguoiXoa)
        {
            return _repository.DeleteSoft(id, nguoiXoa);
        }
    }
}