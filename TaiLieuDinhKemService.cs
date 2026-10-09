using Microsoft.AspNetCore.Http;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Repositories;

namespace QuanLyBaoHiem.Services
{
    public class TaiLieuDinhKemService
    {
        private readonly TaiLieuDinhKemRepository _repo;

        public TaiLieuDinhKemService(
            TaiLieuDinhKemRepository repo)
        {
            _repo = repo;
        }

        // =====================================================
        // 1. LẤY TẤT CẢ TÀI LIỆU
        // =====================================================
        public List<TaiLieuDinhKem> GetAll()
        {
            return _repo.GetAll();
        }

        // =====================================================
        // 2. LẤY TÀI LIỆU THEO ID
        // =====================================================
        public TaiLieuDinhKem? GetById(int id)
        {
            return _repo.GetById(id);
        }

        // =====================================================
        // 3. UPLOAD FILE
        // =====================================================
        public async Task<TaiLieuDinhKem> Upload(
            int maThucThe,
            string loaiThucThe,
            IFormFile file,
            int nguoiUpload)
        {
            // Kiểm tra mã thực thể
            if (maThucThe <= 0)
            {
                throw new Exception(
                    "Mã thực thể không hợp lệ");
            }

            // Kiểm tra loại thực thể
            if (string.IsNullOrWhiteSpace(loaiThucThe))
            {
                throw new Exception(
                    "Loại thực thể không được để trống");
            }

            // Chỉ cho phép 3 loại
            if (loaiThucThe != "HopDong" &&
                loaiThucThe != "BoiThuong" &&
                loaiThucThe != "KhachHang")
            {
                throw new Exception(
                    "Loại thực thể phải là HopDong, BoiThuong hoặc KhachHang");
            }

            // Kiểm tra file
            if (file == null || file.Length == 0)
            {
                throw new Exception(
                    "Vui lòng chọn file");
            }

            // =================================================
            // TẠO THƯ MỤC LƯU FILE
            // =================================================

            string folder =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads");

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            // =================================================
            // TẠO TÊN FILE MỚI
            // =================================================

            string extension =
                Path.GetExtension(file.FileName);

            string fileName =
                Guid.NewGuid().ToString()
                + extension;

            string filePath =
                Path.Combine(
                    folder,
                    fileName);

            // =================================================
            // LƯU FILE
            // =================================================

            using (var stream =
                   new FileStream(
                       filePath,
                       FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // =================================================
            // XÁC ĐỊNH LOẠI FILE
            // =================================================

            string loaiFile = "other";

            if (file.ContentType.StartsWith("image/"))
            {
                loaiFile = "image";
            }
            else if (
                file.ContentType == "application/pdf")
            {
                loaiFile = "pdf";
            }

            // Đường dẫn file trả về cho API
            string duongDanFile =
                "/uploads/" + fileName;

            // =================================================
            // TẠO OBJECT TÀI LIỆU
            // =================================================

            var taiLieu = new TaiLieuDinhKem
            {
                LoaiThucThe = loaiThucThe,
                MaThucThe = maThucThe,
                TenFileGoc = file.FileName,
                DuongDanFile = duongDanFile,
                LoaiFile = loaiFile,
                KichThuocByte = (int)file.Length,
                NguoiUpload = nguoiUpload,
                NgayUpload = DateTime.Now,
                IsDeleted = false,
                NgayXoa = null
            };

            // =================================================
            // LƯU THÔNG TIN VÀO DATABASE
            // =================================================

            _repo.Add(taiLieu);

            return taiLieu;
        }

        // =====================================================
        // 4. XÓA MỀM
        // =====================================================
        public bool Delete(int id)
        {
            return _repo.Delete(id);
        }
    }
}