using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;
using System.Security.Claims;

namespace QuanLyBaoHiem.Services
{
    public class PermissionFilter : IAsyncActionFilter
    {
        private readonly QuanLyBaoHiemContext _db;

        public PermissionFilter(QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // ==========================================
            // API không yêu cầu đăng nhập
            // ==========================================
            if (context.ActionDescriptor.EndpointMetadata
                .OfType<IAllowAnonymous>()
                .Any())
            {
                await next();
                return;
            }

            // ==========================================
            // Chưa đăng nhập
            // ==========================================
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                await next();
                return;
            }

            // ==========================================
            // Lấy Role từ JWT
            // ==========================================
            var roleName =
                context.HttpContext.User
                    .FindFirstValue(ClaimTypes.Role);

            if (string.IsNullOrWhiteSpace(roleName))
            {
                context.Result = new ForbidResult();
                return;
            }

            // ==========================================
            // Admin được toàn quyền
            // ==========================================
            if (roleName.Equals(
                "Admin",
                StringComparison.OrdinalIgnoreCase))
            {
                await next();
                return;
            }

            // ==========================================
            // Xác định Controller
            // ==========================================
            var controller =
                context.RouteData.Values["controller"]?.ToString();

            if (string.IsNullOrWhiteSpace(controller))
            {
                await next();
                return;
            }

            // ==========================================
            // Xác định chức năng
            // ==========================================
            var tenChucNang = controller switch
            {
                "KhachHang" => "Khách hàng",
                "NhanVien" => "Nhân viên",
                "LoaiBaoHiem" => "Loại bảo hiểm",
                "HopDong" => "Hợp đồng",
                "BoiThuong" => "Bồi thường",
                "HoaDon" => "Hóa đơn",
                "ThanhToan" => "Thanh toán",
                "LichThuPhi" => "Lịch thu phí",
                "Dashboard" => "Dashboard",
                "ThongBao" => "Thông báo",
                "AuditLog" => "AuditLog",
                "TaiLieuDinhKem" => "Tài liệu đính kèm",
                "Search" => "Tìm kiếm",
                "History" => "Lịch sử",
                "Export" => "Xuất file",
                "Pdf" => "Xuất PDF",

                _ => null
            };

            // Không có cấu hình chức năng
            if (tenChucNang == null)
            {
                await next();
                return;
            }

            // ==========================================
            // Xác định hành động
            // ==========================================
            string hanhDong;

            switch (context.HttpContext.Request.Method.ToUpper())
            {
                case "GET":

                    if (controller == "Export" ||
                        controller == "Pdf")
                    {
                        hanhDong = "XUAT";
                    }
                    else
                    {
                        hanhDong = "XEM";
                    }

                    break;

                case "POST":
                    hanhDong = "THEM";
                    break;

                case "PUT":
                    hanhDong = "SUA";
                    break;

                case "DELETE":
                    hanhDong = "XOA";
                    break;

                default:
                    await next();
                    return;
            }

            // ==========================================
            // Tìm quyền
            // ==========================================
            var quyen = await _db.Quyen
                .FirstOrDefaultAsync(x =>
                    x.TenQuyen == roleName &&
                    !x.IsDeleted);

            if (quyen == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // ==========================================
            // Tìm chức năng
            // ==========================================
            var chucNang = await _db.ChucNang
                .FirstOrDefaultAsync(x =>
                    x.TenChucNang == tenChucNang &&
                    !x.IsDeleted);

            if (chucNang == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // ==========================================
            // Tìm phân quyền
            // ==========================================
            var phanQuyen = await _db.PhanQuyen
                .FirstOrDefaultAsync(x =>
                    x.MaQuyen == quyen.MaQuyen &&
                    x.MaChucNang == chucNang.MaChucNang);

            if (phanQuyen == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            // ==========================================
            // Kiểm tra quyền
            // ==========================================
            bool duocPhep = hanhDong switch
            {
                "XEM" => phanQuyen.DuocXem,
                "THEM" => phanQuyen.DuocThem,
                "SUA" => phanQuyen.DuocSua,
                "XOA" => phanQuyen.DuocXoa,
                "XUAT" => phanQuyen.DuocXuatFile,

                _ => false
            };

            if (!duocPhep)
            {
                context.Result = new ForbidResult();
                return;
            }

            // ==========================================
            // Có quyền → cho phép chạy API
            // ==========================================
            await next();
        }
    }
}