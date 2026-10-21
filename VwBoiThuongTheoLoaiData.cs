
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class VwBoiThuongTheoLoaiData
    {
        private readonly QuanLyBaoHiemContext _context;

        public VwBoiThuongTheoLoaiData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<VwBoiThuongTheoLoai> LayDanhSach()
        {
            return _context.VwBoiThuongTheoLoais
                .AsNoTracking()
                .ToList();
        }

        public VwBoiThuongTheoLoai? LayTheoLoai(string tenLoai)
        {
            return _context.VwBoiThuongTheoLoais
                .AsNoTracking()
                .FirstOrDefault(x => x.TenLoai == tenLoai);
        }
    }
}
