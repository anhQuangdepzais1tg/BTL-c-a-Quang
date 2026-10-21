
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class HangDoiTacVuService
    {
        private readonly HangDoiTacVuData _data;

        public HangDoiTacVuService(HangDoiTacVuData data)
        {
            _data = data;
        }

        public List<HangDoiTacVu> LayDanhSach()
        {
            return _data.LayDanhSach();
        }

        public HangDoiTacVu? LayTheoMa(long maJob)
        {
            return _data.LayTheoMa(maJob);
        }
    }
}
