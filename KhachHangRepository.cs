using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class KhachHangRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public KhachHangRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy danh sách khách hàng chưa bị xóa
        public List<KhachHang> GetAll()
        {
            return _context.KhachHang
                .Where(x => !x.IsDeleted)
                .ToList();
        }

        // Lấy khách hàng theo mã
        public KhachHang? GetById(int id)
        {
            return _context.KhachHang
                .FirstOrDefault(x => x.MaKH == id && !x.IsDeleted);
        }

        // Tìm kiếm + phân trang
        public (List<KhachHang> Data, int TotalCount) GetPaged(
            string? keyword,
            int pageIndex,
            int pageSize)
        {
            var query = _context.KhachHang
                .Where(x => !x.IsDeleted)
                .AsQueryable();

            // Tìm theo họ tên, số điện thoại hoặc CCCD
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.HoTen.Contains(keyword) ||
                    x.SoDienThoai.Contains(keyword) ||
                    x.CCCD.Contains(keyword));
            }

            int totalCount = query.Count();

            var data = query
                .OrderBy(x => x.MaKH)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (data, totalCount);
        }

        // Thêm khách hàng
        public void Add(KhachHang khachHang)
        {
            // Gán các giá trị mặc định trước khi lưu
            khachHang.NgayTao = DateTime.Now;
            khachHang.SoPhienBan = 1;
            khachHang.IsDeleted = false;

            _context.KhachHang.Add(khachHang);
                _context.SaveChanges();
        }

        // Kiểm tra khách hàng còn hợp đồng đang hoạt động hay không
        public bool HasActiveContract(int maKH)
        {
            return _context.HopDong.Any(x =>
                x.MaKH == maKH &&
                !x.IsDeleted &&
                x.TrangThai == "Đang hoạt động");
        }

        // Sửa khách hàng
        public void Update(KhachHang khachHang)
        {
            var data = _context.KhachHang
                .FirstOrDefault(x => x.MaKH == khachHang.MaKH);

            if (data == null)
                return;

            // Chỉ sửa các thông tin cho phép sửa
            data.HoTen = khachHang.HoTen;
            data.SoDienThoai = khachHang.SoDienThoai;
            data.DiaChi = khachHang.DiaChi;
            data.NgaySinh = khachHang.NgaySinh;

            data.NguoiCapNhat = khachHang.NguoiCapNhat;
            data.NgayCapNhat = DateTime.Now;

            data.SoPhienBan = data.SoPhienBan + 1;

            _context.SaveChanges();
        }

        // Xóa mềm
        public void DeleteSoft(int id, int? nguoiXoa)
        {
            var khachHang = _context.KhachHang
                .FirstOrDefault(x => x.MaKH == id && !x.IsDeleted);

            if (khachHang == null)
                return;

            khachHang.IsDeleted = true;
            khachHang.NgayXoa = DateTime.Now;
            khachHang.NguoiXoa = nguoiXoa;

            _context.SaveChanges();
        }

        // Khôi phục khách hàng
        public void Restore(int id)
        {
            var khachHang = _context.KhachHang
                .FirstOrDefault(x => x.MaKH == id);

            if (khachHang == null)
                return;

            khachHang.IsDeleted = false;
            khachHang.NgayXoa = null;
            khachHang.NguoiXoa = null;

            _context.SaveChanges();
        }
    }
}