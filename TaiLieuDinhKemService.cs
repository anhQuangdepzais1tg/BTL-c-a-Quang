using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class TaiLieuDinhKemService
    {
        private readonly TaiLieuDinhKemData _data;

        public TaiLieuDinhKemService(TaiLieuDinhKemData data)
        {
            _data = data;
        }

        public Task<List<TaiLieuDinhKem>> LayDanhSach()
        {
            return _data.LayDanhSach();
        }

        public Task<TaiLieuDinhKem?> LayTheoMa(int ma)
        {
            return _data.LayTheoMa(ma);
        }

        public Task Them(TaiLieuDinhKem taiLieu)
        {
            return _data.Them(taiLieu);
        }

        public Task<bool> XoaMem(int ma)
        {
            return _data.XoaMem(ma);
        }
    }
}