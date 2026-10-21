using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class TaiLieuDinhKemData
    {
        private readonly QuanLyBaoHiemContext _context;

        public TaiLieuDinhKemData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy danh sách tài liệu chưa bị xóa
        public async Task<List<TaiLieuDinhKem>> LayDanhSach()
        {
            return await _context.TaiLieuDinhKems
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.NgayUpload)
                .ToListAsync();
        }

        // Lấy tài liệu theo mã
        public async Task<TaiLieuDinhKem?> LayTheoMa(int ma)
        {
            return await _context.TaiLieuDinhKems
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.MaTaiLieu == ma && !x.IsDeleted);
        }

        // Thêm thông tin tài liệu
        public async Task Them(TaiLieuDinhKem taiLieu)
        {
            _context.TaiLieuDinhKems.Add(taiLieu);
            await _context.SaveChangesAsync();
        }

        // Xóa mềm tài liệu
        public async Task<bool> XoaMem(int ma)
        {
            var taiLieu = await _context.TaiLieuDinhKems
                .FirstOrDefaultAsync(x =>
                    x.MaTaiLieu == ma && !x.IsDeleted);

            if (taiLieu == null)
                return false;

            taiLieu.IsDeleted = true;
            taiLieu.NgayXoa = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}