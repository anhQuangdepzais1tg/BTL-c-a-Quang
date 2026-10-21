
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class VwDoanhThuTheoThangService
    {
        private readonly VwDoanhThuTheoThangData _data;

        public VwDoanhThuTheoThangService(VwDoanhThuTheoThangData data)
        {
            _data = data;
        }

        public List<VwDoanhThuTheoThang> LayDanhSach()
            => _data.LayDanhSach();

        public VwDoanhThuTheoThang? LayTheoThang(int nam, int thang)
            => _data.LayTheoThang(nam, thang);
    }
}
