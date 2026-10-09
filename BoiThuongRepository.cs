using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class BoiThuongRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public BoiThuongRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy danh sách bồi thường
        public List<BoiThuong> GetAll()
        {
            return _context.BoiThuong
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.MaBT)
                .ToList();
        }

        // Lấy theo mã
        public BoiThuong? GetById(int id)
        {
            return _context.BoiThuong
                .FirstOrDefault(x => x.MaBT == id && !x.IsDeleted);
        }

        // Thêm yêu cầu bồi thường
        public bool Add(BoiThuong boiThuong)
        {
            // Kiểm tra hợp đồng còn hiệu lực
            var hopDong = _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == boiThuong.MaHD &&
                    x.TrangThai == "Đang hoạt động" &&
                    !x.IsDeleted);

            if (hopDong == null)
                return false;

            boiThuong.NgayTao = DateTime.Now;
            boiThuong.SoPhienBan = 1;
            boiThuong.IsDeleted = false;

            // Khi khách hàng mới tạo yêu cầu
            boiThuong.MaNV = null;
            boiThuong.TrangThai = "Đang xử lý";

            _context.BoiThuong.Add(boiThuong);
            _context.SaveChanges();

            return true;
        }

        // Duyệt bồi thường
        public bool Duyet(
            int id,
            int maNVXuLy,
            int? nguoiCapNhat)
        {
            var data = _context.BoiThuong
                .FirstOrDefault(x =>
                    x.MaBT == id &&
                    x.TrangThai == "Đang xử lý" &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.TrangThai = "Đã duyệt";
            data.MaNV = maNVXuLy;
            data.NguoiCapNhat = nguoiCapNhat;
            data.NgayCapNhat = DateTime.Now;
            data.SoPhienBan++;

            _context.SaveChanges();

            return true;
        }

        // Từ chối bồi thường
        public bool TuChoi(
            int id,
            int maNVXuLy,
            int? nguoiCapNhat)
        {
            var data = _context.BoiThuong
                .FirstOrDefault(x =>
                    x.MaBT == id &&
                    x.TrangThai == "Đang xử lý" &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.TrangThai = "Từ chối";
            data.MaNV = maNVXuLy;
            data.NguoiCapNhat = nguoiCapNhat;
            data.NgayCapNhat = DateTime.Now;
            data.SoPhienBan++;

            _context.SaveChanges();

            return true;
        }

        // Xóa mềm
        public bool DeleteSoft(int id, int? nguoiXoa)
        {
            var data = _context.BoiThuong
                .FirstOrDefault(x =>
                    x.MaBT == id &&
                    !x.IsDeleted);

            if (data == null)
                return false;

            data.IsDeleted = true;
            data.NgayXoa = DateTime.Now;
            data.NguoiXoa = nguoiXoa;

            _context.SaveChanges();

            return true;
        }
    }
}