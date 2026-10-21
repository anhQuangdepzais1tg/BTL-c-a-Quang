
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class AuditLogService
    {
        private readonly AuditLogData _data;

        public AuditLogService(AuditLogData data)
        {
            _data = data;
        }

        public Task<List<AuditLog>> LayTatCa(
            int page = 1, int size = 20, string? tenBang = null)
        {
            if (page < 1) page = 1;
            if (size < 1) size = 20;
            if (size > 100) size = 100;

            return _data.LayTatCa(page, size, tenBang);
        }

        public Task<AuditLog?> LayTheoMa(long maLog)
        {
            return _data.LayTheoMa(maLog);
        }

        public async Task GhiLog(AuditLog log)
        {
            if (string.IsNullOrWhiteSpace(log.TenBang))
                throw new ArgumentException("Tên bảng không được để trống.");

            if (string.IsNullOrWhiteSpace(log.HanhDong))
                throw new ArgumentException("Hành động không được để trống.");

            if (log.ThoiGian == default)
                log.ThoiGian = DateTime.Now;

            await _data.Them(log);
        }
    }
}
