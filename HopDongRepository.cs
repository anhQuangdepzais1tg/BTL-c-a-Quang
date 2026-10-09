using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class HopDongRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public HopDongRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // ===============================
        // LẤY DANH SÁCH HỢP ĐỒNG
        // ===============================
        public List<HopDong> GetAll()
        {
            return _context.HopDong
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaHD)
                .ToList();
        }

        // ===============================
        // LẤY HỢP ĐỒNG THEO ID
        // ===============================
        public HopDong? GetById(int id)
        {
            return _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == id &&
                    !x.IsDeleted);
        }

        // ===============================
        // THÊM HỢP ĐỒNG
        // ===============================
        public void Add(HopDong hopDong)
        {
            // Ngày tạo
            hopDong.NgayTao = DateTime.Now;

            // Phiên bản đầu tiên
            hopDong.SoPhienBan = 1;

            // Chưa xóa
            hopDong.IsDeleted = false;

            // Mặc định trạng thái
            if (string.IsNullOrWhiteSpace(hopDong.TrangThai))
            {
                hopDong.TrangThai = "Đang hoạt động";
            }

            // Mặc định chu kỳ đóng phí
            if (string.IsNullOrWhiteSpace(hopDong.ChuKyDongPhi))
            {
                hopDong.ChuKyDongPhi = "Một lần";
            }

            _context.HopDong.Add(hopDong);

            _context.SaveChanges();
        }

        // ===============================
        // CẬP NHẬT HỢP ĐỒNG
        // ===============================
        public void Update(HopDong hopDong)
        {
            var data = _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == hopDong.MaHD &&
                    !x.IsDeleted);

            if (data == null)
                return;

            // Chỉ cập nhật các trường cho phép sửa
            data.NgayKetThuc = hopDong.NgayKetThuc;
            data.SoTien = hopDong.SoTien;
            data.NguoiCapNhat = hopDong.NguoiCapNhat;
            data.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();
        }

        // ===============================
        // XÓA MỀM HỢP ĐỒNG
        // ===============================
        public bool DeleteSoft(int id, int? nguoiXoa)
        {
            var data = _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.IsDeleted = true;
            data.NgayXoa = DateTime.Now;
            data.NguoiXoa = nguoiXoa;

            _context.SaveChanges();

            return true;
        }

        // ===============================
        // GIA HẠN HỢP ĐỒNG
        // ===============================
        public bool GiaHan(
            int id,
            DateTime ngayKetThucMoi,
            int? nguoiCapNhat)
        {
            var data = _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.NgayKetThuc = ngayKetThucMoi;
            data.TrangThai = "Đang hoạt động";
            data.NguoiCapNhat = nguoiCapNhat;
            data.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();

            return true;
        }

        // ===============================
        // HỦY HỢP ĐỒNG
        // ===============================
        public bool Huy(
            int id,
            int? nguoiCapNhat)
        {
            var data = _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.TrangThai = "Đã hủy";
            data.NguoiCapNhat = nguoiCapNhat;
            data.NgayCapNhat = DateTime.Now;

            _context.SaveChanges();

            return true;
        }
    }
}