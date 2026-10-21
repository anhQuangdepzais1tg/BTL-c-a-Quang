using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class HopDongLichSuService
    {
        private readonly HopDongLichSuData _data;

        public HopDongLichSuService(HopDongLichSuData data)
        {
            _data = data;
        }

        public List<HopDongLichSu> LayDanhSach()
        {
            return _data.LayDanhSach();
        }

        public List<HopDongLichSu> LayTheoMaHopDong(int maHD)
        {
            return _data.LayTheoMaHopDong(maHD);
        }

        public HopDongLichSu? LayTheoMaLichSu(int maLichSu)
        {
            return _data.LayTheoMaLichSu(maLichSu);
        }
    }
}