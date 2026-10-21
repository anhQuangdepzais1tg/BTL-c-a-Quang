
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class ThanhToanService
    {
        private readonly ThanhToanData _thanhToanData;

        public ThanhToanService(ThanhToanData thanhToanData)
        {
            _thanhToanData = thanhToanData;
        }

        // Lấy danh sách thanh toán
        public async Task<List<ThanhToan>> LayDanhSach()
        {
            return await _thanhToanData.LayDanhSach();
        }

        // Tìm thanh toán theo mã
        public async Task<ThanhToan?> TimTheoMa(int maTt)
        {
            return await _thanhToanData.TimTheoMa(maTt);
        }

        // Thêm thanh toán
        public async Task<string> Them(ThanhToan thanhToan)
        {
            if (thanhToan.MaHdn <= 0)
                return "Mã hóa đơn không hợp lệ.";

            if (thanhToan.SoTien <= 0)
                return "Số tiền thanh toán phải lớn hơn 0.";

            if (string.IsNullOrWhiteSpace(thanhToan.PhuongThuc))
                return "Vui lòng nhập phương thức thanh toán.";

            thanhToan.NgayTao = DateTime.Now;
            thanhToan.IsDeleted = false;

            await _thanhToanData.Them(thanhToan);

            return "Thêm thanh toán thành công.";
        }

        // Cập nhật thanh toán
        public async Task<string> CapNhat(
            int maTt, ThanhToan thanhToan)
        {
            var thanhToanCu =
                await _thanhToanData.TimTheoMa(maTt);

            if (thanhToanCu == null)
                return "Không tìm thấy thanh toán.";

            if (thanhToan.SoTien <= 0)
                return "Số tiền thanh toán phải lớn hơn 0.";

            if (string.IsNullOrWhiteSpace(thanhToan.PhuongThuc))
                return "Vui lòng nhập phương thức thanh toán.";

            thanhToanCu.MaHdn = thanhToan.MaHdn;
            thanhToanCu.NgayThanhToan = thanhToan.NgayThanhToan;
            thanhToanCu.SoTien = thanhToan.SoTien;
            thanhToanCu.PhuongThuc = thanhToan.PhuongThuc;

            await _thanhToanData.CapNhat(thanhToanCu);

            return "Cập nhật thanh toán thành công.";
        }

        // Xóa mềm thanh toán
        public async Task<string> Xoa(int maTt)
        {
            var thanhToan =
                await _thanhToanData.TimTheoMa(maTt);

            if (thanhToan == null)
                return "Không tìm thấy thanh toán.";

            await _thanhToanData.XoaMem(thanhToan);

            return "Xóa thanh toán thành công.";
        }
    }
}
