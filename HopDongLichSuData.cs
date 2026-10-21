using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class HopDongLichSuData
    {
        private readonly QuanLyBaoHiemContext _context;

        public HopDongLichSuData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ lịch sử hợp đồng
        public List<HopDongLichSu> LayDanhSach()
        {
            return _context.HopDongLichSus
                .AsNoTracking()
                .OrderByDescending(x => x.ThoiGianSua)
                .ToList();
        }

        // Lấy lịch sử theo mã hợp đồng
        public List<HopDongLichSu> LayTheoMaHopDong(int maHD)
        {
            return _context.HopDongLichSus
                .AsNoTracking()
                .Where(x => x.MaHd == maHD)
                .OrderByDescending(x => x.ThoiGianSua)
                .ToList();
        }

        // Lấy một bản ghi lịch sử theo mã lịch sử
        public HopDongLichSu? LayTheoMaLichSu(int maLichSu)
        {
            return _context.HopDongLichSus
                .AsNoTracking()
                .FirstOrDefault(x => x.MaLichSu == maLichSu);
        }
    }
}