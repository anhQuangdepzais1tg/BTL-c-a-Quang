
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class ThanhToanData
    {
        private readonly QuanLyBaoHiemContext _db;

        public ThanhToanData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // Lấy danh sách thanh toán chưa bị xóa
        public async Task<List<ThanhToan>> LayDanhSach()
        {
            return await _db.ThanhToans
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.NgayThanhToan)
                .ToListAsync();
        }

        // Tìm thanh toán theo mã
        public async Task<ThanhToan?> TimTheoMa(int maTt)
        {
            return await _db.ThanhToans
                .FirstOrDefaultAsync(x =>
                    x.MaTt == maTt && !x.IsDeleted);
        }

        // Thêm thanh toán
        public async Task Them(ThanhToan thanhToan)
        {
            await _db.ThanhToans.AddAsync(thanhToan);
            await _db.SaveChangesAsync();
        }

        // Cập nhật thanh toán
        public async Task CapNhat(ThanhToan thanhToan)
        {
            _db.ThanhToans.Update(thanhToan);
            await _db.SaveChangesAsync();
        }

        // Xóa mềm thanh toán
        public async Task XoaMem(ThanhToan thanhToan)
        {
            thanhToan.IsDeleted = true;
            await _db.SaveChangesAsync();
        }
    }
}
