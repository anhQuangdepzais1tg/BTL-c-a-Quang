
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class BoiThuongData
    {
        private readonly QuanLyBaoHiemContext _context;

        public BoiThuongData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách bồi thường chưa bị xóa
        public async Task<List<BoiThuong>> GetAll()
        {
            return await _context.BoiThuongs
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaBt)
                .ToListAsync();
        }

        // 2. Tìm bồi thường theo mã
        public async Task<BoiThuong?> GetById(int id)
        {
            return await _context.BoiThuongs
                .FirstOrDefaultAsync(x =>
                    x.MaBt == id && !x.IsDeleted);
        }

        // 3. Thêm yêu cầu bồi thường
        public async Task Create(BoiThuong duLieu)
        {
            duLieu.IsDeleted = false;
            duLieu.SoPhienBan = 1;
            duLieu.NgayTao = DateTime.Now;

            _context.BoiThuongs.Add(duLieu);
            await _context.SaveChangesAsync();
        }

        // 4. Cập nhật yêu cầu bồi thường
        public async Task Update(BoiThuong duLieu)
        {
            _context.BoiThuongs.Update(duLieu);
            await _context.SaveChangesAsync();
        }

        // 5. Xóa mềm yêu cầu bồi thường
        public async Task Delete(int id, int? nguoiXoa = null)
        {
            var duLieu = await GetById(id);

            if (duLieu == null)
                return;

            duLieu.IsDeleted = true;
            duLieu.NgayXoa = DateTime.Now;
            duLieu.NguoiXoa = nguoiXoa;

            await _context.SaveChangesAsync();
        }
    }
}
