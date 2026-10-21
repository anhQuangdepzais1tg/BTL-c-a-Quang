using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuanLyBaoHiem.Models;

namespace QuanLyBaoHiem.Data;

public partial class QuanLyBaoHiemContext : DbContext
{
    public QuanLyBaoHiemContext(DbContextOptions<QuanLyBaoHiemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<BoiThuong> BoiThuongs { get; set; }
        
    public virtual DbSet<BoiThuongLichSu> BoiThuongLichSus { get; set; }

    public virtual DbSet<ChucNang> ChucNangs { get; set; }

    public virtual DbSet<HangDoiTacVu> HangDoiTacVus { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<HopDong> HopDongs { get; set; }

    public virtual DbSet<HopDongLichSu> HopDongLichSus { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LichThuPhi> LichThuPhis { get; set; }

    public virtual DbSet<LoaiBaoHiem> LoaiBaoHiems { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }  

    public virtual DbSet<TaiLieuDinhKem> TaiLieuDinhKems { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    public virtual DbSet<VwBoiThuongChiTiet> VwBoiThuongChiTiets { get; set; }

    public virtual DbSet<VwBoiThuongTheoLoai> VwBoiThuongTheoLoais { get; set; }

    public virtual DbSet<VwDoanhThuTheoThang> VwDoanhThuTheoThangs { get; set; }

    public virtual DbSet<VwHoaDonChiTiet> VwHoaDonChiTiets { get; set; }

    public virtual DbSet<VwHopDongChiTiet> VwHopDongChiTiets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.MaLog);
            entity.ToTable("AuditLog");

            entity.HasIndex(e => new { e.TenBang, e.MaBanGhi }, "IX_AuditLog_Bang_Ban_ghi");

            entity.HasIndex(e => e.ThoiGian, "IX_AuditLog_ThoiGian");

            entity.Property(e => e.DiaChiIp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DiaChiIP");
            entity.Property(e => e.HanhDong)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MaLog).ValueGeneratedOnAdd();
            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.TenBang).HasMaxLength(100);
            entity.Property(e => e.ThoiGian)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaTkNavigation).WithMany()
                .HasForeignKey(d => d.MaTk)
                .HasConstraintName("FK_AuditLog_TaiKhoan");
        });

        modelBuilder.Entity<BoiThuong>(entity =>
        {
            entity.HasKey(e => e.MaBt).HasName("PK__BoiThuon__27247597E84DABAF");

            entity.ToTable("BoiThuong", tb => tb.HasTrigger("trg_BoiThuong_Audit"));

            entity.HasIndex(e => e.TrangThai, "IX_BoiThuong_TrangThai").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.MaBt).HasColumnName("MaBT");
            entity.Property(e => e.LyDo).HasMaxLength(200);
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.SoPhienBan).HasDefaultValue(1);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang xử lý");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.BoiThuongs)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BoiThuong_HopDong");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.BoiThuongs)
                .HasForeignKey(d => d.MaNv)
                .HasConstraintName("FK_BoiThuong_NhanVien");

            entity.HasOne(d => d.NguoiCapNhatNavigation).WithMany(p => p.BoiThuongNguoiCapNhatNavigations)
                .HasForeignKey(d => d.NguoiCapNhat)
                .HasConstraintName("FK_BoiThuong_NguoiCapNhat");

            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.BoiThuongNguoiTaoNavigations)
                .HasForeignKey(d => d.NguoiTao)
                .HasConstraintName("FK_BoiThuong_NguoiTao");

            entity.HasOne(d => d.NguoiXoaNavigation).WithMany(p => p.BoiThuongNguoiXoaNavigations)
                .HasForeignKey(d => d.NguoiXoa)
                .HasConstraintName("FK_BoiThuong_NguoiXoa");
        });

        modelBuilder.Entity<BoiThuongLichSu>(entity =>
        {
            entity.HasKey(e => e.MaLichSu).HasName("PK__BoiThuon__C443222ACFDA1505");

            entity.ToTable("BoiThuong_LichSu");

            entity.HasIndex(e => e.MaBt, "IX_BoiThuongLS_MaBT");

            entity.Property(e => e.MaBt).HasColumnName("MaBT");
            entity.Property(e => e.ThoiGianSua)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaBtNavigation).WithMany(p => p.BoiThuongLichSus)
                .HasForeignKey(d => d.MaBt)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BoiThuongLS_BoiThuong");

            entity.HasOne(d => d.NguoiSuaNavigation).WithMany(p => p.BoiThuongLichSus)
                .HasForeignKey(d => d.NguoiSua)
                .HasConstraintName("FK_BoiThuongLS_TaiKhoan");
        });

        modelBuilder.Entity<ChucNang>(entity =>
        {
            entity.HasKey(e => e.MaChucNang).HasName("PK__ChucNang__B26DC257E0CDA6DC");

            entity.ToTable("ChucNang");

            entity.Property(e => e.DuongDan).HasMaxLength(200);
            entity.Property(e => e.TenChucNang).HasMaxLength(100);
        });

        modelBuilder.Entity<HangDoiTacVu>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("HangDoiTacVu");

            entity.HasIndex(e => new { e.TrangThai, e.NgayTao }, "IX_HangDoiTacVu_TrangThai");

            entity.Property(e => e.LoaiJob)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MaJob).ValueGeneratedOnAdd();
            entity.Property(e => e.NgayHoanThanh).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXuLy).HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("PENDING");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHdn).HasName("PK__HoaDon__3C90E8C63903BD7B");

            entity.ToTable("HoaDon");

            entity.HasIndex(e => e.TrangThai, "IX_HoaDon_TrangThai").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.MaHdn).HasColumnName("MaHDN");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa thanh toán");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDon_HopDong");

            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.NguoiTao)
                .HasConstraintName("FK_HoaDon_NguoiTao");
        });

        modelBuilder.Entity<HopDong>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HopDong__2725A6E0A8D68FB8");

            entity.ToTable("HopDong", tb =>
                {
                    tb.HasTrigger("trg_HopDong_Audit");
                    tb.HasTrigger("trg_HopDong_Versioning");
                });

            entity.HasIndex(e => e.MaKh, "IX_HopDong_KhachHang").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => new { e.TrangThai, e.NgayBatDau }, "IX_HopDong_TrangThai").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.ChuKyDongPhi)
                .HasMaxLength(20)
                .HasDefaultValue("Một lần");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.SoPhienBan).HasDefaultValue(1);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Đang hoạt động");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HopDongs)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HopDong_KhachHang");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.HopDongs)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HopDong_LoaiBaoHiem");

            entity.HasOne(d => d.MaNvNavigation).WithMany(p => p.HopDongs)
                .HasForeignKey(d => d.MaNv)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HopDong_NhanVien");

            entity.HasOne(d => d.NguoiCapNhatNavigation).WithMany(p => p.HopDongNguoiCapNhatNavigations)
                .HasForeignKey(d => d.NguoiCapNhat)
                .HasConstraintName("FK_HopDong_NguoiCapNhat");

            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.HopDongNguoiTaoNavigations)
                .HasForeignKey(d => d.NguoiTao)
                .HasConstraintName("FK_HopDong_NguoiTao");

            entity.HasOne(d => d.NguoiXoaNavigation).WithMany(p => p.HopDongNguoiXoaNavigations)
                .HasForeignKey(d => d.NguoiXoa)
                .HasConstraintName("FK_HopDong_NguoiXoa");
        });

        modelBuilder.Entity<HopDongLichSu>(entity =>
        {
            entity.HasKey(e => e.MaLichSu).HasName("PK__HopDong___C443222A0FBDFD08");

            entity.ToTable("HopDong_LichSu");

            entity.HasIndex(e => e.MaHd, "IX_HopDongLS_MaHD");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.ThoiGianSua)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.HopDongLichSus)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HopDongLS_HopDong");

            entity.HasOne(d => d.NguoiSuaNavigation).WithMany(p => p.HopDongLichSus)
                .HasForeignKey(d => d.NguoiSua)
                .HasConstraintName("FK_HopDongLS_TaiKhoan");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E5AFCB4F9");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.HoTen, "IX_KhachHang_HoTen").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.SoDienThoai, "IX_KhachHang_SDT").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.Cccd, "UQ__KhachHan__A955A0AA41DA73EA").IsUnique();

            entity.HasIndex(e => e.MaTk, "UX_KhachHang_MaTK")
                .IsUnique()
                .HasFilter("([MaTK] IS NOT NULL)");

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CCCD");
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SoPhienBan).HasDefaultValue(1);

            entity.HasOne(d => d.MaTkNavigation).WithOne(p => p.KhachHangMaTkNavigation)
                .HasForeignKey<KhachHang>(d => d.MaTk)
                .HasConstraintName("FK_KhachHang_TaiKhoan");

            entity.HasOne(d => d.NguoiCapNhatNavigation).WithMany(p => p.KhachHangNguoiCapNhatNavigations)
                .HasForeignKey(d => d.NguoiCapNhat)
                .HasConstraintName("FK_KhachHang_NguoiCapNhat");

            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.KhachHangNguoiTaoNavigations)
                .HasForeignKey(d => d.NguoiTao)
                .HasConstraintName("FK_KhachHang_NguoiTao");
        });

        modelBuilder.Entity<LichThuPhi>(entity =>
        {
            entity.HasKey(e => e.MaLichThu).HasName("PK__LichThuP__147EF5D092CA741B");

            entity.ToTable("LichThuPhi");

            entity.HasIndex(e => new { e.NgayDenHan, e.TrangThai }, "IX_LichThuPhi_DenHan");

            entity.HasIndex(e => new { e.MaHd, e.KyThu }, "UQ_LichThuPhi").IsUnique();

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHdn).HasColumnName("MaHDN");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("Chưa đến hạn");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.LichThuPhis)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LichThuPhi_HopDong");

            entity.HasOne(d => d.MaHdnNavigation).WithMany(p => p.LichThuPhis)
                .HasForeignKey(d => d.MaHdn)
                .HasConstraintName("FK_LichThuPhi_HoaDon");
        });

        modelBuilder.Entity<LoaiBaoHiem>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiBaoH__730A575965108192");

            entity.ToTable("LoaiBaoHiem");

            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.MucPhi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.TenLoai).HasMaxLength(100);

            entity.HasOne(d => d.NguoiCapNhatNavigation).WithMany(p => p.LoaiBaoHiemNguoiCapNhatNavigations)
                .HasForeignKey(d => d.NguoiCapNhat)
                .HasConstraintName("FK_LoaiBaoHiem_NguoiCapNhat");

            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.LoaiBaoHiemNguoiTaoNavigations)
                .HasForeignKey(d => d.NguoiTao)
                .HasConstraintName("FK_LoaiBaoHiem_NguoiTao");
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNv).HasName("PK__NhanVien__2725D70A2BC64CC7");

            entity.ToTable("NhanVien");

            entity.HasIndex(e => e.HoTen, "IX_NhanVien_HoTen").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.MaTk, "UX_NhanVien_MaTK")
                .IsUnique()
                .HasFilter("([MaTK] IS NOT NULL)");

            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.ChucVu).HasMaxLength(50);
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.SoPhienBan).HasDefaultValue(1);

            entity.HasOne(d => d.MaTkNavigation).WithOne(p => p.NhanVienMaTkNavigation)
                .HasForeignKey<NhanVien>(d => d.MaTk)
                .HasConstraintName("FK_NhanVien_TaiKhoan");

            entity.HasOne(d => d.NguoiCapNhatNavigation).WithMany(p => p.NhanVienNguoiCapNhatNavigations)
                .HasForeignKey(d => d.NguoiCapNhat)
                .HasConstraintName("FK_NhanVien_NguoiCapNhat");

            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.NhanVienNguoiTaoNavigations)
                .HasForeignKey(d => d.NguoiTao)
                .HasConstraintName("FK_NhanVien_NguoiTao");

            entity.HasOne(d => d.NguoiXoaNavigation).WithMany(p => p.NhanVienNguoiXoaNavigations)
                .HasForeignKey(d => d.NguoiXoa)
                .HasConstraintName("FK_NhanVien_NguoiXoa");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.MaToken).HasName("PK__Password__C30F950E3909A47E");

            entity.ToTable("PasswordResetToken");

            entity.HasIndex(e => e.Token, "UQ__Password__1EB4F81738EE9154").IsUnique();

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.NgayHetHan).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Token)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(d => d.MaTk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PRT_TaiKhoan");
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaPhanQuyen).HasName("PK__PhanQuye__529AB12B0616AC7C");

            entity.ToTable("PhanQuyen");

            entity.HasIndex(e => new { e.MaQuyen, e.MaChucNang }, "UQ_PhanQuyen").IsUnique();

            entity.HasOne(d => d.MaChucNangNavigation).WithMany(p => p.PhanQuyens)
                .HasForeignKey(d => d.MaChucNang)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanQuyen_ChucNang");

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.PhanQuyens)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhanQuyen_Quyen");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen).HasName("PK__Quyen__1D4B7ED4A97A3FDF");

            entity.ToTable("Quyen");

            entity.HasIndex(e => e.TenQuyen, "UQ__Quyen__5637EE79AD9DA312").IsUnique();

            entity.Property(e => e.MoTa).HasMaxLength(200);
            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.MaRefreshToken).HasName("PK__RefreshT__1FEA46E852C53B5E");

            entity.ToTable("RefreshToken");

            entity.HasIndex(e => e.Token, "UX_RefreshToken_Token").IsUnique();

            entity.Property(e => e.DiaChiIp)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DiaChiIP");
            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.NgayHetHan).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThietBi).HasMaxLength(200);
            entity.Property(e => e.Token)
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.MaTk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshToken_TaiKhoan");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk).HasName("PK__TaiKhoan__2725007066B43CAE");

            entity.ToTable("TaiKhoan", tb => tb.HasTrigger("trg_TaiKhoan_Audit"));

            entity.HasIndex(e => e.Email, "IX_TaiKhoan_Email").HasFilter("([IsDeleted]=(0))");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC0B54B035C").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__TaiKhoan__A9D105346EE9AFE4").IsUnique();

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.Bat2Fa).HasColumnName("Bat2FA");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IpdangNhapCuoi)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IPDangNhapCuoi");
            entity.Property(e => e.KhoaDenNgay).HasColumnType("datetime");
            entity.Property(e => e.KichHoat).HasDefaultValue(true);
            entity.Property(e => e.LanDangNhapCuoi).HasColumnType("datetime");
            entity.Property(e => e.MaBiMat2Fa)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("MaBiMat2FA");
            entity.Property(e => e.MaXacMinhEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MatKhauHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NgayCapNhat).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaQuyen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiKhoan_Quyen");
        });

        modelBuilder.Entity<TaiLieuDinhKem>(entity =>
        {
            entity.HasKey(e => e.MaTaiLieu).HasName("PK__TaiLieuD__FD18A657C8CF6489");

            entity.ToTable("TaiLieuDinhKem");

            entity.HasIndex(e => new { e.LoaiThucThe, e.MaThucThe }, "IX_TaiLieu_ThucThe").HasFilter("([IsDeleted]=(0))");

            entity.Property(e => e.DuongDanFile).HasMaxLength(500);
            entity.Property(e => e.LoaiFile)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.LoaiThucThe)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NgayUpload)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoa).HasColumnType("datetime");
            entity.Property(e => e.TenFileGoc).HasMaxLength(255);

            entity.HasOne(d => d.NguoiUploadNavigation).WithMany(p => p.TaiLieuDinhKems)
                .HasForeignKey(d => d.NguoiUpload)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiLieu_NguoiUpload");
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaTt).HasName("PK__ThanhToa__27250079D98FED41");

            entity.ToTable("ThanhToan", tb => tb.HasTrigger("trg_ThanhToan_Audit"));

            entity.Property(e => e.MaTt).HasColumnName("MaTT");
            entity.Property(e => e.MaHdn).HasColumnName("MaHDN");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhuongThuc).HasMaxLength(50);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHdnNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaHdn)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThanhToan_HoaDon");

            entity.HasOne(d => d.NguoiTaoNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.NguoiTao)
                .HasConstraintName("FK_ThanhToan_NguoiTao");
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.MaTb).HasName("PK__ThongBao__2725006FA0F3EC0B");

            entity.ToTable("ThongBao");

            entity.HasIndex(e => new { e.MaTk, e.DaDoc }, "IX_ThongBao_MaTK");

            entity.Property(e => e.MaTb).HasColumnName("MaTB");
            entity.Property(e => e.Loai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("INAPP");
            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.NgayGui).HasColumnType("datetime");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TieuDe).HasMaxLength(200);

            entity.HasOne(d => d.MaTkNavigation).WithMany(p => p.ThongBaos)
                .HasForeignKey(d => d.MaTk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThongBao_TaiKhoan");
        });

        modelBuilder.Entity<VwBoiThuongChiTiet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BoiThuong_ChiTiet");

            entity.Property(e => e.LyDo).HasMaxLength(200);
            entity.Property(e => e.MaBt).HasColumnName("MaBT");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.NhanVienXuLy).HasMaxLength(100);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenKhachHang).HasMaxLength(100);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<VwBoiThuongTheoLoai>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BoiThuong_TheoLoai");

            entity.Property(e => e.TenLoai).HasMaxLength(100);
            entity.Property(e => e.TongDaBoiThuong).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VwDoanhThuTheoThang>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DoanhThu_TheoThang");

            entity.Property(e => e.TongDoanhThu).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VwHoaDonChiTiet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_HoaDon_ChiTiet");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaHdn).HasColumnName("MaHDN");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenKhachHang).HasMaxLength(100);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<VwHopDongChiTiet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_HopDong_ChiTiet");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MaNv).HasColumnName("MaNV");
            entity.Property(e => e.SdtKhachHang)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SDT_KhachHang");
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TenKhachHang).HasMaxLength(100);
            entity.Property(e => e.TenLoai).HasMaxLength(100);
            entity.Property(e => e.TenNhanVien).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
