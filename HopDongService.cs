
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class HopDongService
    {
        private readonly HopDongData _data;

        public HopDongService(HopDongData data)
        {
            _data = data;
        }

        public Task<List<HopDong>> GetAll()
            => _data.GetAll();

        public Task<HopDong?> GetById(int id)
            => _data.GetById(id);

        public async Task<(bool ThanhCong, string ThongBao, HopDong? DuLieu)>
            Create(HopDong hopDong)
        {
            if (hopDong.MaKh <= 0 || hopDong.MaLoai <= 0 || hopDong.MaNv <= 0)
                return (false, "Mã khách hàng, loại bảo hiểm và nhân viên phải lớn hơn 0.", null);

            if (hopDong.NgayKetThuc <= hopDong.NgayBatDau)
                return (false, "Ngày kết thúc phải sau ngày bắt đầu.", null);

            if (hopDong.SoTien < 0)
                return (false, "Số tiền không được âm.", null);

            string[] chuKy =
            {
                "Hàng năm", "Hàng quý", "Hàng tháng", "Một lần"
            };

            if (!chuKy.Contains(hopDong.ChuKyDongPhi))
                return (false, "Chu kỳ đóng phí không hợp lệ.", null);

            var duLieu = await _data.Create(hopDong);
            return (true, "Thêm hợp đồng thành công.", duLieu);
        }

        public async Task<(bool ThanhCong, string ThongBao)>
            Update(int id, HopDong hopDong)
        {
            if (hopDong.MaKh <= 0 || hopDong.MaLoai <= 0 || hopDong.MaNv <= 0)
                return (false, "Mã khách hàng, loại bảo hiểm và nhân viên phải lớn hơn 0.");

            if (hopDong.NgayKetThuc <= hopDong.NgayBatDau)
                return (false, "Ngày kết thúc phải sau ngày bắt đầu.");

            if (hopDong.SoTien < 0)
                return (false, "Số tiền không được âm.");

            string[] chuKy =
            {
                "Hàng năm", "Hàng quý", "Hàng tháng", "Một lần"
            };

            if (!chuKy.Contains(hopDong.ChuKyDongPhi))
                return (false, "Chu kỳ đóng phí không hợp lệ.");

            var thanhCong = await _data.Update(id, hopDong);

            return thanhCong
                ? (true, "Cập nhật hợp đồng thành công.")
                : (false, "Không tìm thấy hợp đồng.");
        }

        public async Task<(bool ThanhCong, string ThongBao)> Delete(int id)
        {
            var thanhCong = await _data.Delete(id);

            return thanhCong
                ? (true, "Xóa mềm hợp đồng thành công.")
                : (false, "Không tìm thấy hợp đồng.");
        }
    }
}
