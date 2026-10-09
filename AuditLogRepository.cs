using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class AuditLogRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public AuditLogRepository(
            QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        public List<AuditLog> GetAll(
            int page,
            int size,
            string? bang)
        {
            var query = _context.AuditLog.AsQueryable();

            if (!string.IsNullOrWhiteSpace(bang))
            {
                query = query.Where(
                    x => x.TenBang == bang);
            }

            return query
                .OrderByDescending(x => x.ThoiGian)
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }

        public AuditLog? Get(long id)
        {
            return _context.AuditLog
                .FirstOrDefault(x => x.MaLog == id);
        }
    }
}