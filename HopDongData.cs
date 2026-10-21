
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class HopDongData
    {
        private readonly QuanLyBaoHiemContext _context;

        public HopDongData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy danh sách hợp đồng chưa bị xóa
        public async Task<List<HopDong>> GetAll()
        {
            return await _context.HopDongs
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaHd)
                .ToListAsync();
        }

        // Lấy hợp đồng theo mã, không lấy hợp đồng đã xóa mềm
        public async Task<HopDong?> GetById(int id)
        {
            return await _context.HopDongs
                .FirstOrDefaultAsync(x => x.MaHd == id && !x.IsDeleted);
        }

        // Thêm hợp đồng
        public async Task<HopDong> Create(HopDong hopDong)
        {
            hopDong.NgayTao = DateTime.Now;
            hopDong.NgayCapNhat = null;
            hopDong.NgayXoa = null;
            hopDong.TrangThai = "Đang hoạt động";
            hopDong.SoPhienBan = 1;
            hopDong.IsDeleted = false;

            _context.HopDongs.Add(hopDong);
            await _context.SaveChangesAsync();

            return hopDong;
        }

        // Cập nhật hợp đồng
        public async Task<bool> Update(int id, HopDong duLieu)
        {
            var hopDong = await GetById(id);

            if (hopDong == null)
                return false;

            hopDong.MaKh = duLieu.MaKh;
            hopDong.MaLoai = duLieu.MaLoai;
            hopDong.MaNv = duLieu.MaNv;
            hopDong.NgayBatDau = duLieu.NgayBatDau;
            hopDong.NgayKetThuc = duLieu.NgayKetThuc;
            hopDong.SoTien = duLieu.SoTien;
            hopDong.ChuKyDongPhi = duLieu.ChuKyDongPhi;
            hopDong.NgayCapNhat = DateTime.Now;

            // Trigger trg_HopDong_Versioning tự tăng SoPhienBan
            await _context.SaveChangesAsync();

            return true;
        }

        // Xóa mềm hợp đồng
        public async Task<bool> Delete(int id)
        {
            var hopDong = await GetById(id);

            // Không tìm thấy hoặc đã xóa mềm
            if (hopDong == null)
                return false;

            hopDong.IsDeleted = true;
            hopDong.NgayXoa = DateTime.Now;
            hopDong.NgayCapNhat = DateTime.Now;

            // Không tự tăng SoPhienBan vì trigger đã xử lý
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
