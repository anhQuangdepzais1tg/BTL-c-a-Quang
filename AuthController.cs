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
        private readonly AuthService _service;

        public NguoiDungController(AuthService service)
        {
            _service = service;
        }

        // ===============================
        // ĐĂNG KÝ
        // POST: api/Auth/register
        // ===============================
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

        // ===============================
        // ĐĂNG NHẬP
        // POST: api/Auth/login
        // ===============================
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            try
            {
                var result = _service.Login(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ===============================
        // KIỂM TRA JWT
        // GET: api/Auth/me
        // ===============================
        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                message = "JWT hoạt động",

                maTK = User.FindFirst(
                    ClaimTypes.NameIdentifier)?.Value,

                tenDangNhap = User.Identity?.Name,

                quyen = User.FindFirst(
                    ClaimTypes.Role)?.Value
            });
        }
    }
}