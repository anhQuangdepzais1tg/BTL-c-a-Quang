using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Repositories
{
    public class LichThuPhiRepository
    {
        private readonly QuanLyBaoHiemContext _context;

        public LichThuPhiRepository(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy tất cả lịch thu phí
        public List<LichThuPhi> GetAll()
        {
            return _context.LichThuPhi
                .OrderBy(x => x.NgayDenHan)
                .ToList();
        }

        // Lấy theo mã lịch thu
        public LichThuPhi? GetById(int id)
        {
            return _context.LichThuPhi
                .FirstOrDefault(x => x.MaLichThu == id);
        }

        // Lấy lịch thu phí theo hợp đồng
        public List<LichThuPhi> GetByHopDong(int maHD)
        {
            return _context.LichThuPhi
                .Where(x => x.MaHD == maHD)
                .OrderBy(x => x.KyThu)
                .ToList();
        }

        // Thêm lịch thu phí
        public bool Add(LichThuPhi lich)
        {
            var hopDong = _context.HopDong
                .FirstOrDefault(x =>
                    x.MaHD == lich.MaHD &&
                    !x.IsDeleted);

            if (hopDong == null)
                return false;

            lich.TrangThai = "Chưa đến hạn";
            lich.DaCanhBao = false;
            lich.NgayTao = DateTime.Now;

            _context.LichThuPhi.Add(lich);
            _context.SaveChanges();

            return true;
        }

        // Cập nhật lịch thu phí
        public bool Update(int id, LichThuPhi lichMoi)
        {
            var data = _context.LichThuPhi
                .FirstOrDefault(x => x.MaLichThu == id);

            if (data == null)
                return false;

            data.KyThu = lichMoi.KyThu;
            data.NgayDenHan = lichMoi.NgayDenHan;
            data.SoTien = lichMoi.SoTien;
            data.TrangThai = lichMoi.TrangThai;

            _context.SaveChanges();

            return true;
        }

        // Đánh dấu đã thanh toán
        public bool DaThanhToan(int id)
        {
            var data = _context.LichThuPhi
                .FirstOrDefault(x => x.MaLichThu == id);

            if (data == null)
                return false;

            data.TrangThai = "Đã thanh toán";

            _context.SaveChanges();

            return true;
        }

        // Đánh dấu quá hạn
        public bool QuaHan(int id)
        {
            var data = _context.LichThuPhi
                .FirstOrDefault(x => x.MaLichThu == id);

            if (data == null)
                return false;

            if (data.TrangThai == "Đã thanh toán")
                return false;

            data.TrangThai = "Quá hạn";

            _context.SaveChanges();

            return true;
        }

        // Đánh dấu đã cảnh báo
        public bool DanhDauCanhBao(int id)
        {
            var data = _context.LichThuPhi
                .FirstOrDefault(x => x.MaLichThu == id);

            if (data == null)
                return false;

            data.DaCanhBao = true;

            _context.SaveChanges();

            return true;
        }
    }
}