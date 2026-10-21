using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.DAL
{
    public class ChucNangData
    {
        private readonly QuanLyBaoHiemContext _context;

        public ChucNangData(QuanLyBaoHiemContext context)
        {
            _context = context;
        }

        // Lấy danh sách chức năng chưa bị xóa
        public List<ChucNang> LayDanhSach()
        {
            return _context.ChucNangs
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.MaChucNang)
                .ToList();
        }

        // Lấy chức năng theo mã
        public ChucNang? LayTheoMa(int ma)
        {
            return _context.ChucNangs
                .FirstOrDefault(x => x.MaChucNang == ma && !x.IsDeleted);
        }

        // Kiểm tra tên chức năng đã tồn tại chưa
        public bool TenDaTonTai(string ten, int? maBoQua = null)
        {
            return _context.ChucNangs.Any(x =>
                !x.IsDeleted &&
                x.TenChucNang == ten &&
                (!maBoQua.HasValue || x.MaChucNang != maBoQua.Value));
        }

        // Kiểm tra chức năng cha có tồn tại không
        public bool ChucNangChaHopLe(int maCha, int? maBoQua = null)
        {
            return _context.ChucNangs.Any(x =>
                x.MaChucNang == maCha &&
                !x.IsDeleted &&
                (!maBoQua.HasValue || x.MaChucNang != maBoQua.Value));
        }

        // Thêm chức năng
        public void Them(ChucNang chucNang)
        {
            _context.ChucNangs.Add(chucNang);
            _context.SaveChanges();
        }

        // Lưu thay đổi
        public void LuuThayDoi()
        {
            _context.SaveChanges();
        }
    }
}