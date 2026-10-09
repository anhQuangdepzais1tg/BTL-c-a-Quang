using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class AuditLogService
    {
        private readonly AuditLogRepository _repo;

        public AuditLogService(
            AuditLogRepository repo)
        {
            _repo = repo;
        }

        public List<AuditLog> GetAll(
            int page,
            int size,
            string? bang)
        {
            return _repo.GetAll(
                page,
                size,
                bang);
        }

        public AuditLog? Get(long id)
        {
            return _repo.Get(id);
        }
    }
}