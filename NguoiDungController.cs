using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyBaoHiem.Models;
using QuanLyBaoHiem.Services;
using System.Security.Claims;

namespace QuanLyBaoHiem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly NguoiDungService _service;

        public NguoiDungController(NguoiDungService service)
        {
            _service = service;
        }

        // =====================================================
        // ĐĂNG KÝ
        // POST: api/NguoiDung/register
        // =====================================================
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register(RegisterRequest request)
        {
            try
            {
                var result = _service.Register(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =====================================================
        // ĐĂNG NHẬP
        // POST: api/NguoiDung/login
        // =====================================================
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var ip = HttpContext.Connection
                    .RemoteIpAddress?
                    .ToString();

                var result = _service.Login(request, ip);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.InnerException?.Message
                    ?? ex.Message);
            }
        }

        // =====================================================
        // THÔNG TIN NGƯỜI ĐANG ĐĂNG NHẬP
        // GET: api/NguoiDung/me
        // =====================================================
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            var maTK = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            var tenDangNhap = User.FindFirstValue(
                ClaimTypes.Name);

            var email = User.FindFirstValue(
                ClaimTypes.Email);

            var quyen = User.FindFirstValue(
                ClaimTypes.Role);

            return Ok(new
            {
                MaTK = maTK,
                TenDangNhap = tenDangNhap,
                Email = email,
                Quyen = quyen
            });
        }

        // =====================================================
        // ĐĂNG XUẤT
        // POST: api/NguoiDung/logout
        // =====================================================
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var maTK = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (maTK == null)
                return Unauthorized();

            _service.Logout(int.Parse(maTK));

            return Ok("Đăng xuất thành công");
        }

        // =====================================================
        // REFRESH TOKEN
        // POST: api/NguoiDung/refresh
        // =====================================================
        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh(string refreshToken)
        {
            try
            {
                var result =
                    _service.Refresh(refreshToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =====================================================
        // ĐỔI MẬT KHẨU
        // POST: api/NguoiDung/change-password
        // =====================================================
        [Authorize]
        [HttpPost("change-password")]
        public IActionResult ChangePassword(
            ChangePasswordRequest request)
        {
            try
            {
                var maTK = User.FindFirstValue(
                    ClaimTypes.NameIdentifier);

                if (maTK == null)
                    return Unauthorized();

                _service.ChangePassword(
                    int.Parse(maTK),
                    request);

                return Ok("Đổi mật khẩu thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =====================================================
        // QUÊN MẬT KHẨU
        // POST: api/NguoiDung/forgot-password
        // =====================================================
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                var token =
                    _service.ForgotPassword(email);

                return Ok(new
                {
                    message = "Đã tạo mã đặt lại mật khẩu",
                    token = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =====================================================
        // ĐẶT LẠI MẬT KHẨU
        // POST: api/NguoiDung/reset-password
        // =====================================================
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(
            ResetPasswordRequest request)
        {
            try
            {
                _service.ResetPassword(request);

                return Ok("Đặt lại mật khẩu thành công");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}