using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class ThongBaoData
    {
        private readonly QuanLyBaoHiemContext _db;

        public ThongBaoData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // Lấy danh sách thông báo của một tài khoản
        public async Task<List<ThongBao>> LayDanhSach(int maTk)
        {
            return await _db.ThongBaos
                .AsNoTracking()
                .Where(x => x.MaTk == maTk)
                .OrderByDescending(x => x.NgayTao)
                .ToListAsync();
        }

        // Lấy thông báo theo mã và tài khoản sở hữu
        public async Task<ThongBao?> LayTheoMa(int maTb, int maTk)
        {
            return await _db.ThongBaos
                .FirstOrDefaultAsync(x =>
                    x.MaTb == maTb && x.MaTk == maTk);
        }

        // Admin lấy thông báo theo mã, không giới hạn tài khoản sở hữu
        public async Task<ThongBao?> LayTheoMaAdmin(int maTb)
        {
            return await _db.ThongBaos
                .FirstOrDefaultAsync(x => x.MaTb == maTb);
        }

        // Thêm thông báo mới
        public async Task Them(ThongBao thongBao)
        {
            _db.ThongBaos.Add(thongBao);
            await _db.SaveChangesAsync();
        }

        // Lưu thay đổi, ví dụ đánh dấu thông báo đã đọc
        public async Task LuuThayDoi()
        {
            await _db.SaveChangesAsync();
        }
    }

}