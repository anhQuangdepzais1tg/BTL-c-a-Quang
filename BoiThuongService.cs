
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class BoiThuongService
    {
        private readonly BoiThuongData _data;

        public BoiThuongService(BoiThuongData data)
        {
            _data = data;
        }

        // 1. Lấy danh sách bồi thường
        public async Task<List<BoiThuong>> GetAll()
        {
            return await _data.GetAll();
        }

        // 2. Lấy bồi thường theo mã
        public async Task<BoiThuong?> GetById(int id)
        {
            return await _data.GetById(id);
        }

        // 3. Thêm yêu cầu bồi thường
        public async Task<(bool ThanhCong, string ThongBao)> Create(
            BoiThuong duLieu)
        {
            if (duLieu.MaHd <= 0)
                return (false, "Mã hợp đồng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(duLieu.LyDo))
                return (false, "Vui lòng nhập lý do bồi thường.");

            if (duLieu.SoTien <= 0)
                return (false, "Số tiền bồi thường phải lớn hơn 0.");

            if (duLieu.NgayYeuCau == default)
                return (false, "Vui lòng nhập ngày yêu cầu.");

            duLieu.LyDo = duLieu.LyDo.Trim();

            await _data.Create(duLieu);

            return (true, "Thêm yêu cầu bồi thường thành công.");
        }

        // 4. Cập nhật yêu cầu bồi thường

        public async Task<(bool ThanhCong, string ThongBao)> Update(
            int id, BoiThuong duLieu)
        {
            var hienTai = await _data.GetById(id);

            if (hienTai == null)
                return (false, "Không tìm thấy yêu cầu bồi thường.");

            if (duLieu.MaHd <= 0)
                return (false, "Mã hợp đồng không hợp lệ.");

            if (string.IsNullOrWhiteSpace(duLieu.LyDo))
                return (false, "Vui lòng nhập lý do bồi thường.");

            if (duLieu.SoTien <= 0)
                return (false, "Số tiền bồi thường phải lớn hơn 0.");

            if (duLieu.TrangThai != "Đang xử lý"
                && duLieu.TrangThai != "Đã duyệt"
                && duLieu.TrangThai != "Từ chối")
            {
                return (false, "Trạng thái không hợp lệ.");
            }

            hienTai.MaHd = duLieu.MaHd;
            hienTai.MaNv = duLieu.MaNv;
            hienTai.NgayYeuCau = duLieu.NgayYeuCau;
            hienTai.LyDo = duLieu.LyDo.Trim();
            hienTai.SoTien = duLieu.SoTien;
            hienTai.NgayCapNhat = DateTime.Now;
            hienTai.SoPhienBan++;

            // Không cập nhật TrangThai tại đây
            await _data.Update(hienTai);

            return (true, "Cập nhật yêu cầu bồi thường thành công.");
        }


        // 5. Xóa mềm yêu cầu bồi thường
        public async Task<(bool ThanhCong, string ThongBao)> Delete(
            int id, int? nguoiXoa = null)
        {
            var duLieu = await _data.GetById(id);

            if (duLieu == null)
                return (false, "Không tìm thấy yêu cầu bồi thường.");

            await _data.Delete(id, nguoiXoa);

            return (true, "Xóa yêu cầu bồi thường thành công.");
        }
    }
}
