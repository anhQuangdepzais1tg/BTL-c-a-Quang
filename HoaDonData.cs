
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class HoaDonData
    {
        private readonly QuanLyBaoHiemContext _db;

        public HoaDonData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // Lấy danh sách hóa đơn chưa bị xóa
        public async Task<List<HoaDon>> LayDanhSach()
        {
            return await _db.HoaDons
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.NgayLap)
                .ToListAsync();
        }

        // Tìm hóa đơn theo mã
        public async Task<HoaDon?> TimTheoMa(int maHdn)
        {
            return await _db.HoaDons
                .FirstOrDefaultAsync(x =>
                    x.MaHdn == maHdn && !x.IsDeleted);
        }

        // Thêm hóa đơn
        public async Task Them(HoaDon hoaDon)
        {
            await _db.HoaDons.AddAsync(hoaDon);
            await _db.SaveChangesAsync();
        }

        // Cập nhật hóa đơn
        public async Task CapNhat(HoaDon hoaDon)
        {
            _db.HoaDons.Update(hoaDon);
            await _db.SaveChangesAsync();
        }

        // Xóa mềm hóa đơn
        public async Task XoaMem(HoaDon hoaDon)
        {
            hoaDon.IsDeleted = true;
            hoaDon.NgayXoa = DateTime.Now;
            hoaDon.NgayCapNhat = DateTime.Now;

            await _db.SaveChangesAsync();
        }
    }
}
