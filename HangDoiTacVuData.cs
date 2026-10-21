
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class HangDoiTacVuData
    {
        private readonly QuanLyBaoHiemContext _context;

        public HangDoiTacVuData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<HangDoiTacVu> LayDanhSach()
        {
            return _context.HangDoiTacVus
                .AsNoTracking()
                .OrderByDescending(x => x.NgayTao)
                .ToList();
        }

        public HangDoiTacVu? LayTheoMa(long maJob)
        {
            return _context.HangDoiTacVus
                .AsNoTracking()
                .FirstOrDefault(x => x.MaJob == maJob);
        }
    }
}
