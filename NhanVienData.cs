
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class NhanVienData
    {
        private readonly QuanLyBaoHiemContext _db;

        public NhanVienData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        public async Task<List<NhanVien>> LayTatCa(
            string? tuKhoa, int page, int size)
        {
            var query = _db.NhanViens
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string k = tuKhoa.Trim();
                query = query.Where(x =>
                    x.HoTen.Contains(k) ||
                    x.SoDienThoai.Contains(k) ||
                    x.ChucVu.Contains(k));
            }

            return await query.OrderByDescending(x => x.MaNv)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> DemTatCa(string? tuKhoa)
        {
            var query = _db.NhanViens.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string k = tuKhoa.Trim();
                query = query.Where(x =>
                    x.HoTen.Contains(k) ||
                    x.SoDienThoai.Contains(k) ||
                    x.ChucVu.Contains(k));
            }

            return await query.CountAsync();
        }

        public Task<NhanVien?> LayTheoMa(int maNv)
        {
            return _db.NhanViens
                .FirstOrDefaultAsync(x =>
                    x.MaNv == maNv && !x.IsDeleted);
        }

        public Task<bool> TrungSoDienThoai(
            string soDienThoai, int? boQuaMaNv = null)
        {
            return _db.NhanViens.AnyAsync(x =>
                !x.IsDeleted &&
                x.SoDienThoai == soDienThoai &&
                (!boQuaMaNv.HasValue || x.MaNv != boQuaMaNv.Value));
        }

        public async Task Them(NhanVien nv)
        {
            _db.NhanViens.Add(nv);
            await _db.SaveChangesAsync();
        }

        public async Task LuuThayDoi()
        {
            await _db.SaveChangesAsync();
        }
    }
}
