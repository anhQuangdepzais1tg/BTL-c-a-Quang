
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class BoiThuongLichSuService
    {
        private readonly BoiThuongLichSuData _data;

        public BoiThuongLichSuService(BoiThuongLichSuData data)
        {
            _data = data;
        }

        // Lấy toàn bộ lịch sử
        public async Task<List<BoiThuongLichSu>> GetAll()
        {
            return await _data.GetAll();
        }

        // Lấy lịch sử theo mã bồi thường
        public async Task<List<BoiThuongLichSu>> GetByMaBoiThuong(int maBt)
        {
            return await _data.GetByMaBoiThuong(maBt);
        }
    }
}
