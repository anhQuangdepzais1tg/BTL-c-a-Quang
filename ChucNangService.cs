using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class ChucNangService
    {
        private readonly ChucNangData _data;

        public ChucNangService(ChucNangData data)
        {
            _data = data;
        }

        public List<ChucNang> LayDanhSach()
        {
            return _data.LayDanhSach();
        }

        public ChucNang? LayTheoMa(int ma)
        {
            return _data.LayTheoMa(ma);
        }

        public (bool ThanhCong, string ThongBao, ChucNang? DuLieu)
            Them(ChucNang request)
        {
            if (string.IsNullOrWhiteSpace(request.TenChucNang))
                return (false, "Tên chức năng không được để trống.", null);

            string ten = request.TenChucNang.Trim();

            if (_data.TenDaTonTai(ten))
                return (false, "Tên chức năng đã tồn tại.", null);

            if (request.MaChucNangCha.HasValue &&
                !_data.ChucNangChaHopLe(request.MaChucNangCha.Value))
                return (false, "Chức năng cha không tồn tại.", null);

            var chucNang = new ChucNang
            {
                TenChucNang = ten,
                DuongDan = string.IsNullOrWhiteSpace(request.DuongDan)
                    ? null : request.DuongDan.Trim(),
                MaChucNangCha = request.MaChucNangCha,
                IsDeleted = false
            };

            _data.Them(chucNang);
            return (true, "Thêm chức năng thành công.", chucNang);
        }

        public (bool ThanhCong, string ThongBao) CapNhat(
            int ma, ChucNang request)
        {
            var chucNang = _data.LayTheoMa(ma);

            if (chucNang == null)
                return (false, "Không tìm thấy chức năng.");

            if (string.IsNullOrWhiteSpace(request.TenChucNang))
                return (false, "Tên chức năng không được để trống.");

            string ten = request.TenChucNang.Trim();

            if (_data.TenDaTonTai(ten, ma))
                return (false, "Tên chức năng đã tồn tại.");

            if (request.MaChucNangCha.HasValue)
            {
                int maCha = request.MaChucNangCha.Value;

                if (maCha == ma)
                    return (false, "Chức năng không thể là cha của chính nó.");

                if (!_data.ChucNangChaHopLe(maCha, ma))
                    return (false, "Chức năng cha không tồn tại.");
            }

            chucNang.TenChucNang = ten;
            chucNang.DuongDan = string.IsNullOrWhiteSpace(request.DuongDan)
                ? null : request.DuongDan.Trim();
            chucNang.MaChucNangCha = request.MaChucNangCha;

            _data.LuuThayDoi();
            return (true, "Cập nhật chức năng thành công.");
        }

        public (bool ThanhCong, string ThongBao) XoaMem(int ma)
        {
            var chucNang = _data.LayTheoMa(ma);

            if (chucNang == null)
                return (false, "Không tìm thấy chức năng.");

            chucNang.IsDeleted = true;
            _data.LuuThayDoi();

            return (true, "Xóa chức năng thành công.");
        }
    }
}