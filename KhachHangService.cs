using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class KhachHangService
    {
        private readonly KhachHangRepository _repository;

        public KhachHangService(KhachHangRepository repository)
        {
            _repository = repository;
        }

        // Lấy tất cả khách hàng
        public List<KhachHang> GetAll()
        {
            return _repository.GetAll();
        }

        // Lấy theo mã
        public KhachHang? GetById(int id)
        {
            return _repository.GetById(id);
        }

        // Tìm kiếm + phân trang
        public (List<KhachHang> Data, int TotalCount) GetPaged(
            string? keyword,
            int pageIndex,
            int pageSize)
        {
            if (pageIndex < 1)
                pageIndex = 1;

            if (pageSize < 1)
                pageSize = 10;

            return _repository.GetPaged(
                keyword,
                pageIndex,
                pageSize);
        }

        // Thêm khách hàng
        public bool Add(KhachHang khachHang)
        {
            // Kiểm tra CCCD đã tồn tại chưa
            var trungCCCD = _repository.GetAll()
                .Any(x => x.CCCD == khachHang.CCCD && !x.IsDeleted);

            if (trungCCCD)
            {
                return false;
            }

            // Thêm khách hàng
            _repository.Add(khachHang);

            return true;
        }

        // Sửa
        public void Update(KhachHang khachHang)
        {
            _repository.Update(khachHang);
        }

        // Xóa mềm
        public bool DeleteSoft(int id, int? nguoiXoa)
        {
            var data = _repository.GetById(id);

            if (data == null)
                return false;

            if (_repository.HasActiveContract(id))
            {
                throw new Exception(
                    "Không thể xóa khách hàng vì còn hợp đồng đang hoạt động.");
            }

            _repository.DeleteSoft(id, nguoiXoa);

            return true;
        }

        // Khôi phục
        public bool Restore(int id)
        {
            _repository.Restore(id);
            return true;
        }
    }
}