
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class LichThuPhiData
    {
        private readonly QuanLyBaoHiemContext _db;

        public LichThuPhiData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // Lấy danh sách lịch thu phí
        public async Task<List<LichThuPhi>> LayDanhSach()
        {
            return await _db.LichThuPhis
                .AsNoTracking()
                .OrderBy(x => x.NgayDenHan)
                .ToListAsync();
        }

        // Tìm lịch thu phí theo mã
        public async Task<LichThuPhi?> TimTheoMa(int maLichThu)
        {
            return await _db.LichThuPhis
                .FirstOrDefaultAsync(x => x.MaLichThu == maLichThu);
        }

        // Thêm lịch thu phí
        public async Task Them(LichThuPhi lichThuPhi)
        {
            await _db.LichThuPhis.AddAsync(lichThuPhi);
            await _db.SaveChangesAsync();
        }

        // Cập nhật lịch thu phí
        public async Task CapNhat(LichThuPhi lichThuPhi)
        {
            _db.LichThuPhis.Update(lichThuPhi);
            await _db.SaveChangesAsync();
        }

        // Xóa lịch thu phí
        public async Task Xoa(LichThuPhi lichThuPhi)
        {
            _db.LichThuPhis.Remove(lichThuPhi);
            await _db.SaveChangesAsync();
        }
    }
}
