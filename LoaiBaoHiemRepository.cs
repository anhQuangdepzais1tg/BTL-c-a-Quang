using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class LoaiBaoHiemRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public LoaiBaoHiemRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy danh sách loại bảo hiểm chưa bị xóa
        public List<LoaiBaoHiem> GetAll()
        {
            return _context.LoaiBaoHiem
                .Where(x => !x.IsDeleted)
                .ToList();
        }

        // Lấy theo mã loại
        public LoaiBaoHiem? GetById(int id)
        {
            return _context.LoaiBaoHiem
                .FirstOrDefault(x =>
                    x.MaLoai == id &&
                    !x.IsDeleted);
        }

        // Thêm loại bảo hiểm
        public void Add(LoaiBaoHiem loaiBaoHiem)
        {
            loaiBaoHiem.NgayTao = DateTime.Now;
            loaiBaoHiem.IsDeleted = false;

            _context.LoaiBaoHiem.Add(loaiBaoHiem);
            _context.SaveChanges();
        }

        // Cập nhật loại bảo hiểm
        public void Update(LoaiBaoHiem loaiBaoHiem)
        {
            // Tìm bản ghi thật trong database
            var data = _context.LoaiBaoHiem
                .FirstOrDefault(x =>
                    x.MaLoai == loaiBaoHiem.MaLoai &&
                    !x.IsDeleted);

            if (data == null)
                return;

            // Chỉ sửa các trường cần thiết
            data.TenLoai = loaiBaoHiem.TenLoai;
            data.MoTa = loaiBaoHiem.MoTa;
            data.MucPhi = loaiBaoHiem.MucPhi;

            data.NguoiCapNhat = loaiBaoHiem.NguoiCapNhat;
            data.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();
        }

        // Xóa mềm loại bảo hiểm
        public bool DeleteSoft(int id)
        {
            var data = _context.LoaiBaoHiem
                .FirstOrDefault(x =>
                    x.MaLoai == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            // Đánh dấu đã xóa
            data.IsDeleted = true;

            // Lưu thời gian xóa
            data.NgayXoa = DateTime.Now;

            _context.SaveChanges();

            return true;
        }
    }
}