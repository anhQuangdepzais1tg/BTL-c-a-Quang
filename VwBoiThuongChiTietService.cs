
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class VwBoiThuongChiTietService
    {
        private readonly VwBoiThuongChiTietData _data;

        public VwBoiThuongChiTietService(VwBoiThuongChiTietData data)
        {
            _data = data;
        }

        public List<VwBoiThuongChiTiet> LayDanhSach()
        {
            return _data.LayDanhSach();
        }

        public VwBoiThuongChiTiet? LayTheoMa(int maBt)
        {
            return _data.LayTheoMa(maBt);
        }
    }
}
