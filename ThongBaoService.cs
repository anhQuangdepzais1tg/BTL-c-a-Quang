using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class ThongBaoService
    {
        private readonly ThongBaoData _data;

        public ThongBaoService(ThongBaoData data)
        {
            _data = data;
        }

        // Lấy danh sách thông báo của một tài khoản
        public Task<List<ThongBao>> LayDanhSach(int maTk)
        {
            return _data.LayDanhSach(maTk);
        }

        // Lấy thông báo theo mã và tài khoản sở hữu
        public Task<ThongBao?> LayTheoMa(int maTb, int maTk)
        {
            return _data.LayTheoMa(maTb, maTk);
        }

        // Lấy thông báo theo mã dành cho Admin
        public Task<ThongBao?> LayTheoMaAdmin(int maTb)
        {
            return _data.LayTheoMaAdmin(maTb);
        }

        // Tạo thông báo mới
        public async Task<ThongBao> Them(
            int maTk,
            string tieuDe,
            string noiDung,
            string loai)
        {
            if (maTk <= 0)
                throw new ArgumentException("Mã tài khoản không hợp lệ.");

            if (string.IsNullOrWhiteSpace(tieuDe))
                throw new ArgumentException("Tiêu đề không được để trống.");

            if (string.IsNullOrWhiteSpace(noiDung))
                throw new ArgumentException("Nội dung không được để trống.");

            if (string.IsNullOrWhiteSpace(loai))
                throw new ArgumentException("Loại thông báo không được để trống.");

            // Kiểm tra loại thông báo hợp lệ
            string[] loaiHopLe = { "CA_HAI", "EMAIL", "INAPP" };

            if (!loaiHopLe.Contains(loai.Trim().ToUpperInvariant()))
                throw new ArgumentException(
                    "Loại thông báo chỉ được là CA_HAI, EMAIL hoặc INAPP.");

            var thongBao = new ThongBao
            {
                MaTk = maTk,
                TieuDe = tieuDe.Trim(),
                NoiDung = noiDung.Trim(),
                Loai = loai.Trim().ToUpperInvariant(),
                DaDoc = false,
                DaGui = false,
                NgayTao = DateTime.Now
            };

            await _data.Them(thongBao);

            return thongBao;
        }

        // Đánh dấu thông báo đã đọc
        public async Task<bool> DanhDauDaDoc(
    int maTb, int maTk, bool laAdmin = false)
        {
            var thongBao = laAdmin
                ? await _data.LayTheoMaAdmin(maTb)
                : await _data.LayTheoMa(maTb, maTk);

            if (thongBao == null)
                return false;

            if (!thongBao.DaDoc)
            {
                thongBao.DaDoc = true;
                await _data.LuuThayDoi();
            }

            return true;
        }
    }
}