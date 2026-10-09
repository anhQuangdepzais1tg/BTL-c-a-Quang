using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class TaiLieuDinhKemRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public TaiLieuDinhKemRepository(
            QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy tất cả tài liệu chưa bị xóa
        public List<TaiLieuDinhKem> GetAll()
        {
            return _context.TaiLieuDinhKem
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaTaiLieu)
                .ToList();
        }

        // Lấy tài liệu theo mã
        public TaiLieuDinhKem? GetById(int id)
        {
            return _context.TaiLieuDinhKem
                .FirstOrDefault(x =>
                    x.MaTaiLieu == id &&
                    !x.IsDeleted);
        }

        // Thêm tài liệu vào database
        public void Add(TaiLieuDinhKem taiLieu)
        {
            _context.TaiLieuDinhKem.Add(taiLieu);
            _context.SaveChanges();
        }

        // Xóa mềm tài liệu
        public bool Delete(int id)
        {
            var taiLieu = _context.TaiLieuDinhKem
                .FirstOrDefault(x =>
                    x.MaTaiLieu == id &&
                    !x.IsDeleted);

            if (taiLieu == null)
            {
                return false;
            }

            taiLieu.IsDeleted = true;
            taiLieu.NgayXoa = DateTime.Now;

            _context.SaveChanges();

            return true;
        }
    }
}