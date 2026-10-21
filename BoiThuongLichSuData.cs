
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class BoiThuongLichSuData
    {
        private readonly QuanLyBaoHiemContext _context;

        public BoiThuongLichSuData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy toàn bộ lịch sử bồi thường
        public async Task<List<BoiThuongLichSu>> GetAll()
        {
            return await _context.BoiThuongLichSus
                .AsNoTracking()
                .OrderByDescending(x => x.ThoiGianSua)
                .ToListAsync();
        }

        // Lấy lịch sử theo mã bồi thường
        public async Task<List<BoiThuongLichSu>> GetByMaBoiThuong(int maBt)
        {
            return await _context.BoiThuongLichSus
                .AsNoTracking()
                .Where(x => x.MaBt == maBt)
                .OrderByDescending(x => x.ThoiGianSua)
                .ToListAsync();
        }

        // Thêm một bản ghi lịch sử
        public async Task Create(BoiThuongLichSu lichSu)
        {
            _context.BoiThuongLichSus.Add(lichSu);
            await _context.SaveChangesAsync();
        }
    }
}
