
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class VwHopDongChiTietService
    {
        private readonly VwHopDongChiTietData _data;

        public VwHopDongChiTietService(VwHopDongChiTietData data)
        {
            _data = data;
        }

        public List<VwHopDongChiTiet> LayDanhSach()
            => _data.LayDanhSach();

        public VwHopDongChiTiet? LayTheoMa(int maHd)
            => _data.LayTheoMa(maHd);
    }
}
