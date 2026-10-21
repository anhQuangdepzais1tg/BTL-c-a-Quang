
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class AuditLogData
    {
        private readonly QuanLyBaoHiemContext _context;

        public AuditLogData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public async Task<List<AuditLog>> LayTatCa(
            int page = 1, int size = 20, string? tenBang = null)
        {
            var query = _context.AuditLogs
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(tenBang))
                query = query.Where(x => x.TenBang == tenBang);

            return await query
                .OrderByDescending(x => x.ThoiGian)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<AuditLog?> LayTheoMa(long maLog)
        {
            return await _context.AuditLogs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.MaLog == maLog);
        }

        public async Task Them(AuditLog log)
        {
            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
