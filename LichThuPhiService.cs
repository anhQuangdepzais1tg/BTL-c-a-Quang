
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class LichThuPhiService
    {
        private readonly LichThuPhiData _data;

        public LichThuPhiService(LichThuPhiData data)
        {
            _data = data;
        }

        // Lấy danh sách lịch thu phí
        public async Task<List<LichThuPhi>> LayDanhSach()
        {
            return await _data.LayDanhSach();
        }

        // Tìm lịch thu phí theo mã
        public async Task<LichThuPhi?> TimTheoMa(int maLichThu)
        {
            return await _data.TimTheoMa(maLichThu);
        }

        // Thêm lịch thu phí
        public async Task<string> Them(LichThuPhi lichThuPhi)
        {
            if (lichThuPhi.MaHd <= 0)
                return "Mã hợp đồng không hợp lệ.";

            if (lichThuPhi.KyThu <= 0)
                return "Kỳ thu phải lớn hơn 0.";

            if (lichThuPhi.SoTien <= 0)
                return "Số tiền phải lớn hơn 0.";

            if (string.IsNullOrWhiteSpace(lichThuPhi.TrangThai))
                return "Vui lòng nhập trạng thái.";

            lichThuPhi.NgayTao = DateTime.Now;
            lichThuPhi.DaCanhBao = false;

            await _data.Them(lichThuPhi);

            return "Thêm lịch thu phí thành công.";
        }

        // Cập nhật lịch thu phí
        public async Task<string> CapNhat(
            int maLichThu, LichThuPhi lichThuPhi)
        {
            var lichCu = await _data.TimTheoMa(maLichThu);

            if (lichCu == null)
                return "Không tìm thấy lịch thu phí.";

            if (lichThuPhi.MaHd <= 0)
                return "Mã hợp đồng không hợp lệ.";

            if (lichThuPhi.KyThu <= 0)
                return "Kỳ thu phải lớn hơn 0.";

            if (lichThuPhi.SoTien <= 0)
                return "Số tiền phải lớn hơn 0.";

            if (string.IsNullOrWhiteSpace(lichThuPhi.TrangThai))
                return "Vui lòng nhập trạng thái.";

            lichCu.MaHd = lichThuPhi.MaHd;
            lichCu.KyThu = lichThuPhi.KyThu;
            lichCu.NgayDenHan = lichThuPhi.NgayDenHan;
            lichCu.SoTien = lichThuPhi.SoTien;
            lichCu.TrangThai = lichThuPhi.TrangThai;
            lichCu.MaHdn = lichThuPhi.MaHdn;
            lichCu.DaCanhBao = lichThuPhi.DaCanhBao;

            await _data.CapNhat(lichCu);

            return "Cập nhật lịch thu phí thành công.";
        }

        // Xóa lịch thu phí
        public async Task<string> Xoa(int maLichThu)
        {
            var lichThuPhi = await _data.TimTheoMa(maLichThu);

            if (lichThuPhi == null)
                return "Không tìm thấy lịch thu phí.";

            await _data.Xoa(lichThuPhi);

            return "Xóa lịch thu phí thành công.";
        }
    }
}
