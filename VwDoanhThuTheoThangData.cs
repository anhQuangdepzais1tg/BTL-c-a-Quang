
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class VwDoanhThuTheoThangData
    {
        private readonly QuanLyBaoHiemContext _context;

        public VwDoanhThuTheoThangData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<VwDoanhThuTheoThang> LayDanhSach()
        {
            return _context.VwDoanhThuTheoThangs
                .AsNoTracking()
                .ToList();
        }

        public VwDoanhThuTheoThang? LayTheoThang(int nam, int thang)
        {
            return _context.VwDoanhThuTheoThangs
                .AsNoTracking()
                .FirstOrDefault(x => x.Nam == nam && x.Thang == thang);
        }
    }
}
