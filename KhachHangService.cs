
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Models.DTO;

namespace QuanLyBaoHiem.BLL
{
    public class KhachHangService
    {
        private readonly KhachHangData _data;

        public KhachHangService(KhachHangData data)
        {
            _data = data;
        }

        public async Task<object> LayTatCa(
            string? tuKhoa, int page, int size)
        {
            page = Math.Max(page, 1);
            size = Math.Clamp(size, 1, 100);

            var data = await _data.LayTatCa(tuKhoa, page, size);
            var tongSo = await _data.DemTatCa(tuKhoa);

            return new
            {
                page,
                size,
                tongSo,
                tongSoTrang = (int)Math.Ceiling(tongSo / (double)size),
                danhSach = data
            };
        }

        public Task<KhachHang?> LayTheoMa(int maKh)
        {
            return _data.LayTheoMa(maKh);
        }

        public async Task<KhachHang> Them(
            KhachHangDto dto, int? maNguoiTao)
        {
            KiemTraDuLieu(dto);

            if (await _data.TrungSoDienThoai(dto.SoDienThoai.Trim()))
                throw new ArgumentException("Số điện thoại đã tồn tại.");

            if (await _data.TrungCccd(dto.Cccd.Trim()))
                throw new ArgumentException("CCCD đã tồn tại.");

            var kh = new KhachHang
            {
                MaTk = dto.MaTk,
                HoTen = dto.HoTen.Trim(),
                SoDienThoai = dto.SoDienThoai.Trim(),
                DiaChi = dto.DiaChi?.Trim(),
                Cccd = dto.Cccd.Trim(),
                NgaySinh = dto.NgaySinh,
                NgayTao = DateTime.Now,
                NguoiTao = maNguoiTao,
                SoPhienBan = 1,
                IsDeleted = false
            };

            await _data.Them(kh);
            return kh;
        }

        public async Task<bool> CapNhat(
            int maKh, KhachHangDto dto, int? maNguoiCapNhat)
        {
            KiemTraDuLieu(dto);

            var kh = await _data.LayTheoMa(maKh);
            if (kh == null) return false;

            if (await _data.TrungSoDienThoai(
                dto.SoDienThoai.Trim(), maKh))
                throw new ArgumentException("Số điện thoại đã tồn tại.");

            if (await _data.TrungCccd(dto.Cccd.Trim(), maKh))
                throw new ArgumentException("CCCD đã tồn tại.");

            kh.MaTk = dto.MaTk;
            kh.HoTen = dto.HoTen.Trim();
            kh.SoDienThoai = dto.SoDienThoai.Trim();
            kh.DiaChi = dto.DiaChi?.Trim();
            kh.Cccd = dto.Cccd.Trim();
            kh.NgaySinh = dto.NgaySinh;
            kh.NgayCapNhat = DateTime.Now;
            kh.NguoiCapNhat = maNguoiCapNhat;
            kh.SoPhienBan++;

            await _data.LuuThayDoi();
            return true;
        }

        public async Task<bool> XoaMem(int maKh, int? maNguoiXoa)
        {
            var kh = await _data.LayTheoMa(maKh);
            if (kh == null) return false;

            kh.IsDeleted = true;
            kh.NgayXoa = DateTime.Now;
            kh.NguoiXoa = maNguoiXoa;
            kh.NgayCapNhat = DateTime.Now;

            await _data.LuuThayDoi();
            return true;
        }

        private static void KiemTraDuLieu(KhachHangDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.HoTen))
                throw new ArgumentException("Họ tên không được để trống.");

            if (dto.HoTen.Trim().Length > 100)
                throw new ArgumentException("Họ tên không được quá 100 ký tự.");

            if (string.IsNullOrWhiteSpace(dto.SoDienThoai) ||
                dto.SoDienThoai.Trim().Length > 20)
                throw new ArgumentException("Số điện thoại không hợp lệ.");

            if (string.IsNullOrWhiteSpace(dto.Cccd) ||
                dto.Cccd.Trim().Length > 20)
                throw new ArgumentException("CCCD không hợp lệ.");

            if (dto.DiaChi?.Length > 200)
                throw new ArgumentException("Địa chỉ không được quá 200 ký tự.");

            if (dto.NgaySinh.HasValue &&
                dto.NgaySinh.Value > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Ngày sinh không được ở tương lai.");
        }
    }
}
