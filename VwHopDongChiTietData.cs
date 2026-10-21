
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class VwHopDongChiTietData
    {
        private readonly QuanLyBaoHiemContext _context;

        public VwHopDongChiTietData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<VwHopDongChiTiet> LayDanhSach()
        {
            return _context.VwHopDongChiTiets
                .AsNoTracking()
                .ToList();
        }

        public VwHopDongChiTiet? LayTheoMa(int maHd)
        {
            return _context.VwHopDongChiTiets
                .AsNoTracking()
                .FirstOrDefault(x => x.MaHd == maHd);
        }
    }
}
