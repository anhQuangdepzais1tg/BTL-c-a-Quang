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

        // Lấy tất cả loại bảo hiểm
        public List<LoaiBaoHiem> GetAll()
        {
            return _context.LoaiBaoHiem.ToList();
        }

        // Lấy loại bảo hiểm theo mã
        public LoaiBaoHiem GetById(int id)
        {
            return _context.LoaiBaoHiem.Find(id);
        }

        // Thêm loại bảo hiểm
        public void Add(LoaiBaoHiem loaiBaoHiem)
        {
            _context.LoaiBaoHiem.Add(loaiBaoHiem);
            _context.SaveChanges();
        }

        // Sửa loại bảo hiểm
        public void Update(LoaiBaoHiem loaiBaoHiem)
        {
            _context.LoaiBaoHiem.Update(loaiBaoHiem);
            _context.SaveChanges();
        }

        // Xóa loại bảo hiểm
        public void Delete(int id)
        {
            var loaiBaoHiem = _context.LoaiBaoHiem.Find(id);

            if (loaiBaoHiem != null)
            {
                _context.LoaiBaoHiem.Remove(loaiBaoHiem);
                _context.SaveChanges();
            }
        }
    }
}