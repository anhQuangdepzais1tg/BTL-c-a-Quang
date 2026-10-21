using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using QuanLyBaoHiem.Data;
using QuanLyBaoHiem.DAL;
using QuanLyBaoHiem.BLL;

var builder = WebApplication.CreateBuilder(args);

// ==================================================
// 1. KẾT NỐI CƠ SỞ DỮ LIỆU SQL SERVER
// ==================================================
builder.Services.AddDbContext<QuanLyBaoHiemContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// ==================================================
// 2. ĐĂNG KÝ DAL - LỚP TRUY CẬP DỮ LIỆU
// ==================================================
builder.Services.AddScoped<NguoiDungData>();
builder.Services.AddScoped<PhanQuyenData>();
builder.Services.AddScoped<HoaDonData>();
builder.Services.AddScoped<ThanhToanData>();
builder.Services.AddScoped<LichThuPhiData>();
builder.Services.AddScoped<HopDongData>();
builder.Services.AddScoped<BoiThuongData>();
builder.Services.AddScoped<BoiThuongLichSuData>();
builder.Services.AddScoped<AuditLogData>();
builder.Services.AddScoped<KhachHangData>();
builder.Services.AddScoped<NhanVienData>();
builder.Services.AddScoped<LoaiBaoHiemData>();
builder.Services.AddScoped<ThongBaoData>();
builder.Services.AddScoped<ChucNangData>();
builder.Services.AddScoped<HopDongLichSuData>();
builder.Services.AddScoped<TaiLieuDinhKemData>();
builder.Services.AddScoped<VwBoiThuongChiTietData>();
builder.Services.AddScoped<VwBoiThuongTheoLoaiData>();
builder.Services.AddScoped<VwDoanhThuTheoThangData>();
builder.Services.AddScoped<VwHoaDonChiTietData>();
builder.Services.AddScoped<VwHopDongChiTietData>();
builder.Services.AddScoped<HangDoiTacVuData>();

// ==================================================
// 3. ĐĂNG KÝ BLL - LỚP XỬ LÝ NGHIỆP VỤ
// ==================================================
builder.Services.AddScoped<NguoiDung>();
builder.Services.AddScoped<PhanQuyenService>();
builder.Services.AddScoped<HoaDonService>();
builder.Services.AddScoped<ThanhToanService>();
builder.Services.AddScoped<LichThuPhiService>();
builder.Services.AddScoped<HopDongService>();
builder.Services.AddScoped<BoiThuongService>();
builder.Services.AddScoped<BoiThuongLichSuService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<KhachHangService>();
builder.Services.AddScoped<NhanVienService>();
builder.Services.AddScoped<LoaiBaoHiemService>();
builder.Services.AddScoped<ThongBaoService>();
builder.Services.AddScoped<ChucNangService>();
builder.Services.AddScoped<HopDongLichSuService>();
builder.Services.AddScoped<TaiLieuDinhKemService>();
builder.Services.AddScoped<VwBoiThuongChiTietService>();
builder.Services.AddScoped<VwBoiThuongTheoLoaiService>();
builder.Services.AddScoped<VwDoanhThuTheoThangService>();
builder.Services.AddScoped<VwHoaDonChiTietService>();
builder.Services.AddScoped<VwHopDongChiTietService>();
builder.Services.AddScoped<HangDoiTacVuService>();

// ==================================================
// 4. CẤU HÌNH CONTROLLER VÀ SWAGGER
// ==================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "QuanLyBaoHiem API",
        Version = "v1"
    });

    // Cấu hình nhập JWT trên Swagger
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Nhập JWT token"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

// ==================================================
// 5. CẤU HÌNH XÁC THỰC JWT
// ==================================================
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Thiếu Jwt:Key.");

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey))
            };
    });

// ==================================================
// 6. CẤU HÌNH PHÂN QUYỀN
// ==================================================
builder.Services.AddAuthorization();

// ==================================================
// 7. KHỞI TẠO ỨNG DỤNG
// ==================================================
var app = builder.Build();

// ==================================================
// 8. CẤU HÌNH SWAGGER
// ==================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==================================================
// 9. CẤU HÌNH MIDDLEWARE
// ==================================================
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// ==================================================
// 10. ÁNH XẠ CONTROLLER VÀ CHẠY ỨNG DỤNG
// ==================================================
app.MapControllers();

app.Run();
