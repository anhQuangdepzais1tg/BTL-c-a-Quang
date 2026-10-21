
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class VwHoaDonChiTietService
    {
        private readonly VwHoaDonChiTietData _data;

        public VwHoaDonChiTietService(VwHoaDonChiTietData data)
        {
            _data = data;
        }

        public List<VwHoaDonChiTiet> LayDanhSach()
            => _data.LayDanhSach();

        public VwHoaDonChiTiet? LayTheoMa(int maHdn)
            => _data.LayTheoMa(maHdn);
    }
}
