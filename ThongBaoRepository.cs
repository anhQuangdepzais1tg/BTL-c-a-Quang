using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class ThongBaoRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public ThongBaoRepository(
            QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<ThongBao> GetByTaiKhoan(
            int maTK,
            bool chuaDoc)
        {
            return _context.ThongBao
                .Where(x =>
                    x.MaTK == maTK &&
                    (!chuaDoc || !x.DaDoc))
                .OrderByDescending(x => x.NgayTao)
                .ToList();
        }

        public ThongBao? GetById(int id, int maTK)
        {
            return _context.ThongBao
                .FirstOrDefault(x =>
                    x.MaTB == id &&
                    x.MaTK == maTK);
        }

        public ThongBao Add(ThongBao item)
        {
            item.NgayTao = DateTime.Now;

            _context.ThongBao.Add(item);
            _context.SaveChanges();

            return item;
        }

        public bool Read(int id, int maTK)
        {
            var item = GetById(id, maTK);

            if (item == null)
                return false;

            item.DaDoc = true;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id, int maTK)
        {
            var item = GetById(id, maTK);

            if (item == null)
                return false;

            _context.ThongBao.Remove(item);

            _context.SaveChanges();

            return true;
        }
    }
}