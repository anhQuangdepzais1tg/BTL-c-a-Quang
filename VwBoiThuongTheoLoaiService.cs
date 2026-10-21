
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class VwBoiThuongTheoLoaiService
    {
        private readonly VwBoiThuongTheoLoaiData _data;

        public VwBoiThuongTheoLoaiService(VwBoiThuongTheoLoaiData data)
        {
            _data = data;
        }

        public List<VwBoiThuongTheoLoai> LayDanhSach()
            => _data.LayDanhSach();

        public VwBoiThuongTheoLoai? LayTheoLoai(string tenLoai)
            => _data.LayTheoLoai(tenLoai);
    }
}
