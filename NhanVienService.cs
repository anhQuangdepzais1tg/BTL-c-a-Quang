
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Models.DTO;

namespace QuanLyBaoHiem.BLL
{
    public class NhanVienService
    {
        private readonly NhanVienData _data;

        public NhanVienService(NhanVienData data)
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

        public Task<NhanVien?> LayTheoMa(int maNv)
            => _data.LayTheoMa(maNv);

        public async Task<NhanVien> Them(
            NhanVienDto dto, int? maNguoiTao)
        {
            KiemTra(dto);

            string sdt = dto.SoDienThoai.Trim();

            if (await _data.TrungSoDienThoai(sdt))
                throw new ArgumentException("Số điện thoại đã tồn tại.");

            var nv = new NhanVien
            {
                MaTk = dto.MaTk,
                HoTen = dto.HoTen.Trim(),
                SoDienThoai = sdt,
                DiaChi = dto.DiaChi?.Trim(),
                ChucVu = dto.ChucVu.Trim(),
                NgayTao = DateTime.Now,
                NguoiTao = maNguoiTao,
                SoPhienBan = 1,
                IsDeleted = false
            };

            await _data.Them(nv);
            return nv;
        }

        public async Task<bool> CapNhat(
            int maNv, NhanVienDto dto, int? maNguoiCapNhat)
        {
            KiemTra(dto);

            var nv = await _data.LayTheoMa(maNv);
            if (nv == null) return false;

            string sdt = dto.SoDienThoai.Trim();

            if (await _data.TrungSoDienThoai(sdt, maNv))
                throw new ArgumentException("Số điện thoại đã tồn tại.");

            nv.MaTk = dto.MaTk;
            nv.HoTen = dto.HoTen.Trim();
            nv.SoDienThoai = sdt;
            nv.DiaChi = dto.DiaChi?.Trim();
            nv.ChucVu = dto.ChucVu.Trim();
            nv.NgayCapNhat = DateTime.Now;
            nv.NguoiCapNhat = maNguoiCapNhat;
            nv.SoPhienBan++;

            await _data.LuuThayDoi();
            return true;
        }

        public async Task<bool> XoaMem(int maNv, int? maNguoiXoa)
        {
            var nv = await _data.LayTheoMa(maNv);
            if (nv == null) return false;

            nv.IsDeleted = true;
            nv.NgayXoa = DateTime.Now;
            nv.NguoiXoa = maNguoiXoa;
            nv.NgayCapNhat = DateTime.Now;

            await _data.LuuThayDoi();
            return true;
        }

        private static void KiemTra(NhanVienDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.HoTen) ||
                dto.HoTen.Trim().Length > 100)
                throw new ArgumentException("Họ tên không hợp lệ.");

            if (string.IsNullOrWhiteSpace(dto.SoDienThoai) ||
                dto.SoDienThoai.Trim().Length > 20)
                throw new ArgumentException("Số điện thoại không hợp lệ.");

            if (string.IsNullOrWhiteSpace(dto.ChucVu) ||
                dto.ChucVu.Trim().Length > 100)
                throw new ArgumentException("Chức vụ không hợp lệ.");

            if (dto.DiaChi?.Length > 200)
                throw new ArgumentException("Địa chỉ không được quá 200 ký tự.");
        }
    }
}
