
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class HoaDonService
    {
        private readonly HoaDonData _hoaDonData;

        public HoaDonService(HoaDonData hoaDonData)
        {
            _hoaDonData = hoaDonData;
        }

        // Lấy danh sách hóa đơn
        public async Task<List<HoaDon>> LayDanhSach()
        {
            return await _hoaDonData.LayDanhSach();
        }

        // Tìm hóa đơn theo mã
        public async Task<HoaDon?> TimTheoMa(int maHdn)
        {
            return await _hoaDonData.TimTheoMa(maHdn);
        }

        // Thêm hóa đơn
        public async Task<string> Them(HoaDon hoaDon)
        {
            if (hoaDon.MaHd <= 0)
                return "Mã hợp đồng không hợp lệ.";

            if (hoaDon.SoTien <= 0)
                return "Số tiền hóa đơn phải lớn hơn 0.";

            if (string.IsNullOrWhiteSpace(hoaDon.TrangThai))
                return "Vui lòng nhập trạng thái hóa đơn.";

            hoaDon.NgayTao = DateTime.Now;
            hoaDon.NgayCapNhat = null;
            hoaDon.IsDeleted = false;
            hoaDon.NgayXoa = null;

            await _hoaDonData.Them(hoaDon);

            return "Thêm hóa đơn thành công.";
        }

        // Cập nhật hóa đơn
        public async Task<string> CapNhat(int maHdn, HoaDon hoaDon)
        {
            var hoaDonCu = await _hoaDonData.TimTheoMa(maHdn);

            if (hoaDonCu == null)
                return "Không tìm thấy hóa đơn.";

            if (hoaDon.MaHd <= 0)
                return "Mã hợp đồng không hợp lệ.";

            if (hoaDon.SoTien <= 0)
                return "Số tiền hóa đơn phải lớn hơn 0.";

            if (string.IsNullOrWhiteSpace(hoaDon.TrangThai))
                return "Vui lòng nhập trạng thái hóa đơn.";

            hoaDonCu.MaHd = hoaDon.MaHd;
            hoaDonCu.NgayLap = hoaDon.NgayLap;
            hoaDonCu.SoTien = hoaDon.SoTien;
            hoaDonCu.TrangThai = hoaDon.TrangThai;
            hoaDonCu.NgayCapNhat = DateTime.Now;

            await _hoaDonData.CapNhat(hoaDonCu);

            return "Cập nhật hóa đơn thành công.";
        }

        // Xóa mềm hóa đơn
        public async Task<string> Xoa(int maHdn)
        {
            var hoaDon = await _hoaDonData.TimTheoMa(maHdn);

            if (hoaDon == null)
                return "Không tìm thấy hóa đơn.";

            await _hoaDonData.XoaMem(hoaDon);

            return "Xóa hóa đơn thành công.";
        }
    }
}
