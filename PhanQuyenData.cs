
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class PhanQuyenData
    {
        private readonly QuanLyBaoHiemContext _db;

        public PhanQuyenData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // Lấy danh sách vai trò chưa bị xóa
        public async Task<List<Quyen>> LayDanhSachQuyen()
        {
            return await _db.Quyens
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        // Lấy danh sách chức năng chưa bị xóa
        public async Task<List<ChucNang>> LayDanhSachChucNang()
        {
            return await _db.ChucNangs
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        // Lấy danh sách quyền của một vai trò

        // Lấy quyền theo vai trò và nạp thông tin vai trò, chức năng
        public async Task<List<PhanQuyen>> LayQuyenTheoVaiTro(int maQuyen)
        {
            return await _db.PhanQuyens
                .Include(x => x.MaQuyenNavigation)
                .Include(x => x.MaChucNangNavigation)
                .Where(x => x.MaQuyen == maQuyen)
                .ToListAsync();
        }


        // Tìm quyền theo vai trò và chức năng
        public async Task<PhanQuyen?> TimPhanQuyen(
            int maQuyen, int maChucNang)
        {
            return await _db.PhanQuyens
                .FirstOrDefaultAsync(x =>
                    x.MaQuyen == maQuyen &&
                    x.MaChucNang == maChucNang);
        }

        // Lưu thay đổi xuống cơ sở dữ liệu
        public async Task LuuThayDoi()
        {
            await _db.SaveChangesAsync();
        }
    }
}
