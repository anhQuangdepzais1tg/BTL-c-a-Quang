
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.BLL
{
    public class PhanQuyenService
    {
        private readonly PhanQuyenData _data;

        public PhanQuyenService(PhanQuyenData data)
        {
            _data = data;
        }

        // Lấy danh sách vai trò
        public async Task<List<Quyen>> LayDanhSachQuyen()
        {
            return await _data.LayDanhSachQuyen();
        }

        // Lấy danh sách chức năng
        public async Task<List<ChucNang>> LayDanhSachChucNang()
        {
            return await _data.LayDanhSachChucNang();
        }

        // Lấy quyền theo vai trò, chỉ trả về dữ liệu cần thiết
        public async Task<object> LayQuyenTheoVaiTro(int maQuyen)
        {
            var danhSach = await _data.LayQuyenTheoVaiTro(maQuyen);

            return danhSach.Select(x => new
            {
                x.MaPhanQuyen,
                x.MaQuyen,
                TenQuyen = x.MaQuyenNavigation.TenQuyen,
                x.MaChucNang,
                TenChucNang = x.MaChucNangNavigation.TenChucNang,
                x.DuocXem,
                x.DuocThem,
                x.DuocSua,
                x.DuocXoa,
                x.DuocXuatFile
            }).ToList();
        }

        // Cập nhật quyền
        public async Task<string> CapNhatQuyen(
            int maQuyen, int maChucNang,
            bool duocXem, bool duocThem,
            bool duocSua, bool duocXoa,
            bool duocXuatFile)
        {
            var quyen = await _data.TimPhanQuyen(maQuyen, maChucNang);

            if (quyen == null)
                return "Không tìm thấy quyền của vai trò và chức năng này.";

            quyen.DuocXem = duocXem;
            quyen.DuocThem = duocThem;
            quyen.DuocSua = duocSua;
            quyen.DuocXoa = duocXoa;
            quyen.DuocXuatFile = duocXuatFile;

            await _data.LuuThayDoi();

            return "Cập nhật phân quyền thành công.";
        }
    }
}
