using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class NhanVienRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public NhanVienRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy tất cả nhân viên chưa bị xóa
        public List<NhanVien> GetAll()
        {
            return _context.NhanVien
                .Where(x => !x.IsDeleted)
                .ToList();
        }

        // Lấy nhân viên theo mã
        public NhanVien? GetById(int id)
        {
            return _context.NhanVien
                .FirstOrDefault(x => x.MaNV == id && !x.IsDeleted);
        }

        // Kiểm tra số điện thoại đã tồn tại chưa
        public bool CheckSoDienThoai(string soDienThoai, int? maNV = null)
        {
            return _context.NhanVien.Any(x =>
                x.SoDienThoai == soDienThoai
                && !x.IsDeleted
                && (maNV == null || x.MaNV != maNV));
        }

        // Thêm nhân viên
        public bool Add(NhanVien nhanVien)
        {
            // Kiểm tra số điện thoại trùng
            if (CheckSoDienThoai(nhanVien.SoDienThoai))
            {
                return false;
            }

            // Ngày tạo
            nhanVien.NgayTao = DateTime.Now;

            // Phiên bản đầu tiên
            nhanVien.SoPhienBan = 1;

            // Chưa xóa
            nhanVien.IsDeleted = false;

            // Thêm
            _context.NhanVien.Add(nhanVien);

            // Lưu
            _context.SaveChanges();

            return true;
        }

        // Sửa nhân viên
        public bool Update(NhanVien nhanVien)
        {
            // Kiểm tra số điện thoại trùng với nhân viên khác
            if (CheckSoDienThoai(nhanVien.SoDienThoai, nhanVien.MaNV))
            {
                return false;
            }

            var data = _context.NhanVien
                .FirstOrDefault(x => x.MaNV == nhanVien.MaNV && !x.IsDeleted);

            if (data == null)
            {
                return false;
            }

            // Cập nhật thông tin
            data.HoTen = nhanVien.HoTen;
            data.SoDienThoai = nhanVien.SoDienThoai;
            data.DiaChi = nhanVien.DiaChi;
            data.ChucVu = nhanVien.ChucVu;
            data.MaTK = nhanVien.MaTK;

            // Thông tin cập nhật
            data.NgayCapNhat = DateTime.Now;
            data.NguoiCapNhat = nhanVien.NguoiCapNhat;

            // Tăng phiên bản
            data.SoPhienBan++;

            _context.SaveChanges();

            return true;
        }

        // Xóa mềm nhân viên
        public bool DeleteSoft(int id, int? nguoiXoa)
        {
            var nhanVien = _context.NhanVien
                .FirstOrDefault(x => x.MaNV == id && !x.IsDeleted);

            if (nhanVien == null)
            {
                return false;
            }

            nhanVien.IsDeleted = true;
            nhanVien.NgayXoa = DateTime.Now;
            nhanVien.NguoiXoa = nguoiXoa;

            _context.SaveChanges();

            return true;
        }
    }
}