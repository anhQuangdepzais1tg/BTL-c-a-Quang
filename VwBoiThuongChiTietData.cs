
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class VwBoiThuongChiTietData
    {
        private readonly QuanLyBaoHiemContext _context;

        public VwBoiThuongChiTietData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<VwBoiThuongChiTiet> LayDanhSach()
        {
            return _context.VwBoiThuongChiTiets
                .AsNoTracking()
                .ToList();
        }

        public VwBoiThuongChiTiet? LayTheoMa(int maBt)
        {
            return _context.VwBoiThuongChiTiets
                .AsNoTracking()
                .FirstOrDefault(x => x.MaBt == maBt);
        }
    }
}
