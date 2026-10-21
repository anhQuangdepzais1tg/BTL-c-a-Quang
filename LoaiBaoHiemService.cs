
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Models.DTO;

namespace QuanLyBaoHiem.BLL
{
    public class LoaiBaoHiemService
    {
        private readonly LoaiBaoHiemData _data;

        public LoaiBaoHiemService(LoaiBaoHiemData data)
        {
            _data = data;
        }

        public async Task<object> LayTatCa(
            string? tuKhoa, int page, int size)
        {
            page = Math.Max(1, page);
            size = Math.Clamp(size, 1, 100);

            var danhSach = await _data.LayTatCa(tuKhoa, page, size);
            var tongSo = await _data.DemTatCa(tuKhoa);

            return new
            {
                page,
                size,
                tongSo,
                tongSoTrang = (int)Math.Ceiling(tongSo / (double)size),
                danhSach
            };
        }

        public Task<LoaiBaoHiem?> LayTheoMa(int maLoai)
            => _data.LayTheoMa(maLoai);

        public async Task<LoaiBaoHiem> Them(
            LoaiBaoHiemDto dto, int? maNguoiTao)
        {
            KiemTra(dto);

            string ten = dto.TenLoai.Trim();

            if (await _data.TrungTen(ten))
                throw new ArgumentException("Tên loại bảo hiểm đã tồn tại.");

            var loai = new LoaiBaoHiem
            {
                TenLoai = ten,
                MoTa = dto.MoTa?.Trim(),
                MucPhi = dto.MucPhi,
                NgayTao = DateTime.Now,
                NguoiTao = maNguoiTao,
                IsDeleted = false
            };

            await _data.Them(loai);
            return loai;
        }

        public async Task<bool> CapNhat(
            int maLoai, LoaiBaoHiemDto dto, int? maNguoiCapNhat)
        {
            KiemTra(dto);

            var loai = await _data.LayTheoMa(maLoai);
            if (loai == null) return false;

            string ten = dto.TenLoai.Trim();

            if (await _data.TrungTen(ten, maLoai))
                throw new ArgumentException("Tên loại bảo hiểm đã tồn tại.");

            loai.TenLoai = ten;
            loai.MoTa = dto.MoTa?.Trim();
            loai.MucPhi = dto.MucPhi;
            loai.NgayCapNhat = DateTime.Now;
            loai.NguoiCapNhat = maNguoiCapNhat;

            await _data.LuuThayDoi();
            return true;
        }

        public async Task<bool> XoaMem(int maLoai)
        {
            var loai = await _data.LayTheoMa(maLoai);
            if (loai == null) return false;

            loai.IsDeleted = true;
            loai.NgayXoa = DateTime.Now;
            loai.NgayCapNhat = DateTime.Now;

            await _data.LuuThayDoi();
            return true;
        }

        private static void KiemTra(LoaiBaoHiemDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenLoai))
                throw new ArgumentException("Tên loại bảo hiểm không được để trống.");

            if (dto.MucPhi < 0)
                throw new ArgumentException("Mức phí không được âm.");

            if (dto.MoTa?.Length > 1000)
                throw new ArgumentException("Mô tả không được quá 1000 ký tự.");
        }
    }
}
