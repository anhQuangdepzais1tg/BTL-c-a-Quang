using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.Repositories;
using QuanLyBaoHiem.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// 1. KẾT NỐI SQL SERVER
// ======================================================

builder.Services.AddDbContext<QuanLyBaoHiemContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));


// ======================================================
// 2. HTTP CONTEXT ACCESSOR
// Dùng để lấy thông tin người đăng nhập trong AuditLog
// ======================================================

builder.Services.AddHttpContextAccessor();


// ======================================================
// 3. PERMISSION FILTER
// Kiểm tra quyền Admin / Manager / User
// ======================================================

builder.Services.AddScoped<PermissionFilter>();


// ======================================================
// 4. CONTROLLER
// Gắn PermissionFilter cho toàn bộ Controller
// ======================================================

builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionFilter>();
});


// ======================================================
// 5. REPOSITORY
// ======================================================

builder.Services.AddScoped<LoaiBaoHiemRepository>();
builder.Services.AddScoped<KhachHangRepository>();
builder.Services.AddScoped<NhanVienRepository>();
builder.Services.AddScoped<HopDongRepository>();
builder.Services.AddScoped<BoiThuongRepository>();
builder.Services.AddScoped<HoaDonRepository>();
builder.Services.AddScoped<LichThuPhiRepository>();
builder.Services.AddScoped<TaiLieuDinhKemRepository>();
builder.Services.AddScoped<ThanhToanRepository>();
builder.Services.AddScoped<ThongBaoRepository>();
builder.Services.AddScoped<AuditLogRepository>();
builder.Services.AddScoped<RbacRepository>();

// Repository đăng nhập
builder.Services.AddScoped<NguoiDungRepository>();


// ======================================================
// 6. SERVICE
// ======================================================

builder.Services.AddScoped<LoaiBaoHiemService>();
builder.Services.AddScoped<KhachHangService>();
builder.Services.AddScoped<NhanVienService>();
builder.Services.AddScoped<HopDongService>();
builder.Services.AddScoped<BoiThuongService>();
builder.Services.AddScoped<HoaDonService>();
builder.Services.AddScoped<LichThuPhiService>();
builder.Services.AddScoped<TaiLieuDinhKemService>();
builder.Services.AddScoped<ThanhToanService>();
builder.Services.AddScoped<ThongBaoService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<RbacService>();

// Service đăng nhập
builder.Services.AddScoped<NguoiDungService>();


// ======================================================
// 7. JOB QUEUE
// Xử lý các công việc chạy nền
// ======================================================

builder.Services.AddSingleton<JobQueueService>();

builder.Services.AddHostedService<JobWorker>();


// ======================================================
// 8. CORS
// Cho phép Frontend HTML/CSS/JS gọi API
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// ======================================================
// 9. JWT AUTHENTICATION
// ======================================================

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // Kiểm tra Issuer
                ValidateIssuer = true,

                // Kiểm tra Audience
                ValidateAudience = true,

                // Kiểm tra thời gian hết hạn
                ValidateLifetime = true,

                // Kiểm tra chữ ký
                ValidateIssuerSigningKey = true,

                // Issuer trong appsettings.json
                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                // Audience trong appsettings.json
                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                // Secret Key trong appsettings.json
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]!
                        )
                    ),

                // Không cho phép token hết hạn có độ trễ
                ClockSkew = TimeSpan.Zero
            };
    });


// ======================================================
// 10. AUTHORIZATION
// ======================================================

builder.Services.AddAuthorization();


// ======================================================
// 11. SWAGGER
// ======================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // ----------------------------------------------
    // Khai báo Bearer Token
    // ----------------------------------------------

    options.AddSecurityDefinition(
        "Bearer",
        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",

            Type =
                Microsoft.OpenApi.Models.SecuritySchemeType.Http,

            Scheme = "bearer",

            BearerFormat = "JWT",

            In =
                Microsoft.OpenApi.Models.ParameterLocation.Header,

            Description =
                "Nhập JWT Token. Ví dụ: Bearer eyJhbGciOiJIUzI1Ni..."
        });


    // ----------------------------------------------
    // Cho Swagger sử dụng JWT
    // ----------------------------------------------

    options.AddSecurityRequirement(
        new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
        {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type =
                                Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,

                            Id = "Bearer"
                        }
                },

                Array.Empty<string>()
            }
        });
});


// ======================================================
// 12. BUILD APP
// ======================================================

var app = builder.Build();


// ======================================================
// 13. SWAGGER
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}


// ======================================================
// 14. HTTPS
// ======================================================

app.UseHttpsRedirection();


// ======================================================
// 15. CORS
// Phải đặt trước Authentication
// ======================================================

app.UseCors("AllowAll");


// ======================================================
// 16. AUTHENTICATION
// Kiểm tra JWT
// ======================================================

app.UseAuthentication();


// ======================================================
// 17. AUTHORIZATION
// Kiểm tra quyền truy cập
// ======================================================

app.UseAuthorization();


// ======================================================
// 18. CONTROLLER
// ======================================================

app.MapControllers();


// ======================================================
// 19. CHẠY APPLICATION
// ======================================================

app.Run();