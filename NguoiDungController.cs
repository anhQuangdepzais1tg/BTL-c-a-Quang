
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using QuanLyBaoHiem.BLL;
using QuanLyBaoHiem.Models.DTO;

namespace QuanLyBaoHiem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NguoiDungController : ControllerBase
    {
        private readonly NguoiDung _nguoiDung;

        public NguoiDungController(NguoiDung nguoiDung)
        {
            _nguoiDung = nguoiDung;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult DangKy(DangKyRequest request)
        {
            try
            {
                return Ok(new
                {
                    thanhCong = true,
                    thongBao = _nguoiDung.DangKy(request)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    thanhCong = false,
                    thongBao = ex.Message
                });
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult DangNhap(DangNhapRequest request)
        {
            try
            {
                string? ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                string? thietBi = Request.Headers.UserAgent.ToString();

                return Ok(_nguoiDung.DangNhap(request, ip, thietBi));
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    thanhCong = false,
                    thongBao = ex.Message
                });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult ThongTin()
        {
            return Ok(new
            {
                maTK = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                tenDangNhap = User.Identity?.Name,
                quyen = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh(RefreshTokenRequest request)
        {
            try
            {
                string? ip = HttpContext.Connection.RemoteIpAddress?.ToString();
                string? thietBi = Request.Headers.UserAgent.ToString();

                return Ok(_nguoiDung.LamMoiToken(
                    request.Token, ip, thietBi));
            }
            catch (Exception ex)
            {
                return BadRequest(new { thongBao = ex.Message });
            }
        }

        [HttpPost("revoke-refresh-token")]
        [AllowAnonymous]
        public IActionResult ThuHoi(RefreshTokenRequest request)
        {
            try
            {
                return Ok(new
                {
                    thongBao = _nguoiDung.ThuHoiToken(request.Token)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { thongBao = ex.Message });
            }
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public IActionResult QuenMatKhau(ForgotPasswordRequest request)
        {
            try
            {
                return Ok(_nguoiDung.QuenMatKhau(request.Email));
            }
            catch (Exception ex)
            {
                return BadRequest(new { thongBao = ex.Message });
            }
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public IActionResult DatLaiMatKhau(ResetPasswordRequest request)
        {
            try
            {
                return Ok(new
                {
                    thongBao = _nguoiDung.DatLaiMatKhau(
                        request.Token, request.MatKhauMoi)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { thongBao = ex.Message });
            }
        }
    }
}
