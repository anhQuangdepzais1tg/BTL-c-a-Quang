using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class ThanhToanRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public ThanhToanRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<ThanhToan> GetAll()
        {
            return _context.ThanhToan
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaTT)
                .ToList();
        }

        public ThanhToan? GetById(int id)
        {
            return _context.ThanhToan
                .FirstOrDefault(x =>
                    x.MaTT == id &&
                    !x.IsDeleted);
        }

        public List<ThanhToan> GetByHoaDon(int maHDN)
        {
            return _context.ThanhToan
                .Where(x =>
                    x.MaHDN == maHDN &&
                    !x.IsDeleted)
                .OrderByDescending(x => x.NgayThanhToan)
                .ToList();
        }

        public bool Add(ThanhToan item)
        {
            var hoaDon = _context.HoaDon
                .FirstOrDefault(x =>
                    x.MaHDN == item.MaHDN &&
                    !x.IsDeleted);

            if (hoaDon == null)
                return false;

            if (item.SoTien <= 0)
                return false;

            item.NgayTao = DateTime.Now;
            item.IsDeleted = false;

            _context.ThanhToan.Add(item);
            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var item = GetById(id);

            if (item == null)
                return false;

            item.IsDeleted = true;

            _context.SaveChanges();

            return true;
        }
    }
}