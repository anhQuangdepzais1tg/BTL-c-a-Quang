
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class KhachHangData
    {
        private readonly QuanLyBaoHiemContext _db;

        public KhachHangData(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        // Lấy danh sách khách hàng
        public async Task<List<KhachHang>> LayTatCa(
            string? tuKhoa, int page, int size)
        {
            var query = _db.KhachHangs
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();

                query = query.Where(x =>
                    x.HoTen.Contains(tuKhoa) ||
                    x.SoDienThoai.Contains(tuKhoa) ||
                    x.Cccd.Contains(tuKhoa));
            }

            return await query
                .OrderByDescending(x => x.MaKh)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        // Đếm số khách hàng phù hợp tìm kiếm
        public async Task<int> DemTatCa(string? tuKhoa)
        {
            var query = _db.KhachHangs
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                tuKhoa = tuKhoa.Trim();

                query = query.Where(x =>
                    x.HoTen.Contains(tuKhoa) ||
                    x.SoDienThoai.Contains(tuKhoa) ||
                    x.Cccd.Contains(tuKhoa));
            }

            return await query.CountAsync();
        }

        // Tìm khách hàng theo mã
        public async Task<KhachHang?> LayTheoMa(int maKh)
        {
            return await _db.KhachHangs
                .FirstOrDefaultAsync(x =>
                    x.MaKh == maKh && !x.IsDeleted);
        }

        // Kiểm tra số điện thoại đã tồn tại
        public async Task<bool> TrungSoDienThoai(
            string soDienThoai, int? boQuaMaKh = null)
        {
            return await _db.KhachHangs.AnyAsync(x =>
                !x.IsDeleted &&
                x.SoDienThoai == soDienThoai &&
                (!boQuaMaKh.HasValue || x.MaKh != boQuaMaKh.Value));
        }

        // Kiểm tra CCCD đã tồn tại
        public async Task<bool> TrungCccd(
            string cccd, int? boQuaMaKh = null)
        {
            return await _db.KhachHangs.AnyAsync(x =>
                x.Cccd == cccd &&
                (!boQuaMaKh.HasValue || x.MaKh != boQuaMaKh.Value));
        }

        // Thêm khách hàng
        public async Task Them(KhachHang khachHang)
        {
            _db.KhachHangs.Add(khachHang);
            await _db.SaveChangesAsync();
        }

        // Lưu cập nhật
        public async Task LuuThayDoi()
        {
            await _db.SaveChangesAsync();
        }
    }
}
