using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;

namespace QuanLyBaoHiem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly QuanLyBaoHiemContext _db;

        public HealthController(
            QuanLyBaoHiemContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Kiểm tra kết nối database
                var connected =
                    await _db.Database
                        .CanConnectAsync();

                // Không kết nối được
                if (!connected)
                {
                    return StatusCode(
                        503,
                        new
                        {
                            status = "Unhealthy",
                            database = "Disconnected"
                        });
                }

                // Kết nối thành công
                return Ok(new
                {
                    status = "Healthy",
                    database = "Connected",
                    time = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    503,
                    new
                    {
                        status = "Unhealthy",
                        message = ex.Message
                    });
            }
        }
    }
}