using QuanLyBaoHiem.Services;
using QuanLyBaoHiem.Repositories;
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Data;

var builder = WebApplication.CreateBuilder(args);

// Kết nối với SQL Server
builder.Services.AddDbContext<QuanLyBaoHiemContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddControllers();
builder.Services.AddScoped<LoaiBaoHiemRepository>();
builder.Services.AddScoped<LoaiBaoHiemService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Sử dụng Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();