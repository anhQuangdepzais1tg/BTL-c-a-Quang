
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class LoaiBaoHiemData
    {
        private readonly QuanLyBaoHiemContext _db;

        public LoaiBaoHiemData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        public async Task<List<LoaiBaoHiem>> LayTatCa(
            string? tuKhoa, int page, int size)
        {
            var query = _db.LoaiBaoHiems
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string k = tuKhoa.Trim();
                query = query.Where(x =>
                    x.TenLoai.Contains(k) ||
                    (x.MoTa != null && x.MoTa.Contains(k)));
            }

            return await query.OrderByDescending(x => x.MaLoai)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<int> DemTatCa(string? tuKhoa)
        {
            var query = _db.LoaiBaoHiems.Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string k = tuKhoa.Trim();
                query = query.Where(x =>
                    x.TenLoai.Contains(k) ||
                    (x.MoTa != null && x.MoTa.Contains(k)));
            }

            return await query.CountAsync();
        }

        public Task<LoaiBaoHiem?> LayTheoMa(int maLoai)
        {
            return _db.LoaiBaoHiems.FirstOrDefaultAsync(x =>
                x.MaLoai == maLoai && !x.IsDeleted);
        }

        public Task<bool> TrungTen(
            string tenLoai, int? boQuaMaLoai = null)
        {
            return _db.LoaiBaoHiems.AnyAsync(x =>
                !x.IsDeleted &&
                x.TenLoai == tenLoai &&
                (!boQuaMaLoai.HasValue ||
                 x.MaLoai != boQuaMaLoai.Value));
        }

        public async Task Them(LoaiBaoHiem loai)
        {
            _db.LoaiBaoHiems.Add(loai);
            await _db.SaveChangesAsync();
        }

        public async Task LuuThayDoi()
        {
            await _db.SaveChangesAsync();
        }
    }
}
