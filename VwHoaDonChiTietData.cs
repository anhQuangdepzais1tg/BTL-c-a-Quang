
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class VwHoaDonChiTietData
    {
        private readonly QuanLyBaoHiemContext _context;

        public VwHoaDonChiTietData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<VwHoaDonChiTiet> LayDanhSach()
        {
            return _context.VwHoaDonChiTiets
                .AsNoTracking()
                .ToList();
        }

        public VwHoaDonChiTiet? LayTheoMa(int maHdn)
        {
            return _context.VwHoaDonChiTiets
                .AsNoTracking()
                .FirstOrDefault(x => x.MaHdn == maHdn);
        }
    }
}
