USE master
GO

IF DB_ID('QuanLyBaoHiem') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyBaoHiem SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyBaoHiem;
END
GO

CREATE DATABASE QuanLyBaoHiem
GO

USE QuanLyBaoHiem
GO

/* ------------------------------------------------------------
   1. RBAC: Quyen (Role) - ChucNang (Screen/Module) - PhanQuyen
   ------------------------------------------------------------ */
CREATE TABLE Quyen
(
    MaQuyen     INT IDENTITY PRIMARY KEY,
    TenQuyen    NVARCHAR(50) NOT NULL UNIQUE,
    MoTa        NVARCHAR(200) NULL,
    IsDeleted   BIT NOT NULL DEFAULT 0
)
GO

CREATE TABLE ChucNang
(
    MaChucNang   INT IDENTITY PRIMARY KEY,
    TenChucNang  NVARCHAR(100) NOT NULL,
    MaChucNangCha INT NULL,               -- cho phép menu cha/con
    DuongDan     NVARCHAR(200) NULL,      -- route/screen key, vd: "khachhang", "hopdong"
    IsDeleted    BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_ChucNang_Cha FOREIGN KEY (MaChucNangCha) REFERENCES ChucNang(MaChucNang)
)
GO

-- Phân quyền theo màn hình & theo hành động (Xem/Thêm/Sửa/Xóa)
CREATE TABLE PhanQuyen
(
    MaPhanQuyen  INT IDENTITY PRIMARY KEY,
    MaQuyen      INT NOT NULL,
    MaChucNang   INT NOT NULL,
    DuocXem      BIT NOT NULL DEFAULT 0,
    DuocThem     BIT NOT NULL DEFAULT 0,
    DuocSua      BIT NOT NULL DEFAULT 0,
    DuocXoa      BIT NOT NULL DEFAULT 0,
    DuocXuatFile BIT NOT NULL DEFAULT 0,  -- xuất Excel/PDF
    CONSTRAINT UQ_PhanQuyen UNIQUE (MaQuyen, MaChucNang),
    CONSTRAINT FK_PhanQuyen_Quyen FOREIGN KEY (MaQuyen) REFERENCES Quyen(MaQuyen),
    CONSTRAINT FK_PhanQuyen_ChucNang FOREIGN KEY (MaChucNang) REFERENCES ChucNang(MaChucNang)
)
GO

/* ------------------------------------------------------------
   2. TaiKhoan: đăng nhập, JWT (refresh token), khoá tài khoản,
      xác thực 2 bước, quên/đặt lại mật khẩu
   ------------------------------------------------------------ */
CREATE TABLE TaiKhoan
(
    MaTK                INT IDENTITY PRIMARY KEY,
    TenDangNhap         VARCHAR(50)  NOT NULL UNIQUE,
    MatKhauHash         VARCHAR(255) NOT NULL,      -- hash (vd BCrypt) sinh ở backend, KHÔNG lưu plaintext
    Email               VARCHAR(100) NOT NULL UNIQUE,
    MaQuyen             INT NOT NULL,

    -- Bảo mật đăng nhập
    SoLanDangNhapSai    INT NOT NULL DEFAULT 0,
    KhoaDenNgay         DATETIME NULL,               -- lock-out đến thời điểm này
    LanDangNhapCuoi     DATETIME NULL,
    IPDangNhapCuoi      VARCHAR(50) NULL,

    -- Xác thực 2 bước (2FA - TOTP)
    Bat2FA              BIT NOT NULL DEFAULT 0,
    MaBiMat2FA          VARCHAR(100) NULL,

    -- Trạng thái tài khoản
    KichHoat            BIT NOT NULL DEFAULT 1,      -- tài khoản active (chưa xác minh email thì = 0)
    MaXacMinhEmail      VARCHAR(100) NULL,
    DaXacMinhEmail      BIT NOT NULL DEFAULT 0,

    -- Audit & soft-delete
    NgayTao             DATETIME NOT NULL DEFAULT GETDATE(),
    NgayCapNhat         DATETIME NULL,
    IsDeleted           BIT NOT NULL DEFAULT 0,
    NgayXoa             DATETIME NULL,

    CONSTRAINT FK_TaiKhoan_Quyen FOREIGN KEY (MaQuyen) REFERENCES Quyen(MaQuyen)
)
GO

CREATE INDEX IX_TaiKhoan_Email ON TaiKhoan(Email) WHERE IsDeleted = 0;
GO

-- Refresh token cho JWT (nhiều thiết bị / phiên đăng nhập)
CREATE TABLE RefreshToken
(
    MaRefreshToken  INT IDENTITY PRIMARY KEY,
    MaTK            INT NOT NULL,
    Token           VARCHAR(500) NOT NULL,
    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NgayHetHan      DATETIME NOT NULL,
    DaThuHoi        BIT NOT NULL DEFAULT 0,
    ThietBi         NVARCHAR(200) NULL,
    DiaChiIP        VARCHAR(50) NULL,
    CONSTRAINT FK_RefreshToken_TaiKhoan FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE UNIQUE INDEX UX_RefreshToken_Token ON RefreshToken(Token);
GO

-- Token quên mật khẩu / đặt lại mật khẩu
CREATE TABLE PasswordResetToken
(
    MaToken     INT IDENTITY PRIMARY KEY,
    MaTK        INT NOT NULL,
    Token       VARCHAR(200) NOT NULL UNIQUE,
    NgayTao     DATETIME NOT NULL DEFAULT GETDATE(),
    NgayHetHan  DATETIME NOT NULL,
    DaSuDung    BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_PRT_TaiKhoan FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
)
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 02: Bảng nghiệp vụ
   Các bảng nghiệp vụ chính có trường audit/soft-delete; HopDong và BoiThuong
   có versioning để lưu lịch sử thay đổi.
   ============================================================ */

CREATE TABLE KhachHang
(
    MaKH            INT IDENTITY PRIMARY KEY,
    MaTK            INT NULL,
    HoTen           NVARCHAR(100) NOT NULL,
    SoDienThoai     VARCHAR(20) NOT NULL,
    DiaChi          NVARCHAR(200) NULL,
    CCCD            VARCHAR(20) NOT NULL UNIQUE,
    NgaySinh        DATE NULL,

    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NguoiTao        INT NULL,
    NgayCapNhat     DATETIME NULL,
    NguoiCapNhat    INT NULL,
    SoPhienBan      INT NOT NULL DEFAULT 1,
    IsDeleted       BIT NOT NULL DEFAULT 0,
    NgayXoa         DATETIME NULL,
    NguoiXoa        INT NULL,

    CONSTRAINT FK_KhachHang_TaiKhoan FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK),
    CONSTRAINT FK_KhachHang_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES TaiKhoan(MaTK),
    CONSTRAINT FK_KhachHang_NguoiCapNhat FOREIGN KEY (NguoiCapNhat) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE INDEX IX_KhachHang_HoTen ON KhachHang(HoTen) WHERE IsDeleted = 0;
CREATE INDEX IX_KhachHang_SDT ON KhachHang(SoDienThoai) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_KhachHang_MaTK ON KhachHang(MaTK) WHERE MaTK IS NOT NULL;
GO

CREATE TABLE NhanVien
(
    MaNV            INT IDENTITY PRIMARY KEY,
    MaTK            INT NULL,
    HoTen           NVARCHAR(100) NOT NULL,
    SoDienThoai     VARCHAR(20) NOT NULL,
    DiaChi          NVARCHAR(200) NULL,
    ChucVu          NVARCHAR(50) NOT NULL,

    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NguoiTao        INT NULL,
    NgayCapNhat     DATETIME NULL,
    NguoiCapNhat    INT NULL,
    SoPhienBan      INT NOT NULL DEFAULT 1,
    IsDeleted       BIT NOT NULL DEFAULT 0,
    NgayXoa         DATETIME NULL,
    NguoiXoa        INT NULL,

    CONSTRAINT FK_NhanVien_TaiKhoan FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE INDEX IX_NhanVien_HoTen ON NhanVien(HoTen) WHERE IsDeleted = 0;
CREATE UNIQUE INDEX UX_NhanVien_MaTK ON NhanVien(MaTK) WHERE MaTK IS NOT NULL;
GO

CREATE TABLE LoaiBaoHiem
(
    MaLoai          INT IDENTITY PRIMARY KEY,
    TenLoai         NVARCHAR(100) NOT NULL,
    MoTa            NVARCHAR(200) NULL,
    MucPhi          DECIMAL(18,2) NOT NULL CHECK (MucPhi >= 0),

    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NguoiTao        INT NULL,
    NgayCapNhat     DATETIME NULL,
    NguoiCapNhat    INT NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0,
    NgayXoa         DATETIME NULL
)
GO

CREATE TABLE HopDong
(
    MaHD            INT IDENTITY PRIMARY KEY,
    MaKH            INT NOT NULL,
    MaLoai          INT NOT NULL,
    MaNV            INT NOT NULL,
    NgayBatDau      DATE NOT NULL,
    NgayKetThuc     DATE NOT NULL,
    SoTien          DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    TrangThai       NVARCHAR(50) NOT NULL DEFAULT N'Đang hoạt động'
                    CHECK (TrangThai IN (N'Đang hoạt động', N'Hết hạn', N'Đã hủy')),
    ChuKyDongPhi    NVARCHAR(20) NOT NULL DEFAULT N'Một lần'
                    CHECK (ChuKyDongPhi IN (N'Một lần', N'Hàng tháng', N'Hàng quý', N'Hàng năm')),

    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NguoiTao        INT NULL,
    NgayCapNhat     DATETIME NULL,
    NguoiCapNhat    INT NULL,
    SoPhienBan      INT NOT NULL DEFAULT 1,
    IsDeleted       BIT NOT NULL DEFAULT 0,
    NgayXoa         DATETIME NULL,
    NguoiXoa        INT NULL,

    CONSTRAINT CK_HopDong_Ngay CHECK (NgayKetThuc > NgayBatDau),
    CONSTRAINT FK_HopDong_KhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH),
    CONSTRAINT FK_HopDong_LoaiBaoHiem FOREIGN KEY (MaLoai) REFERENCES LoaiBaoHiem(MaLoai),
    CONSTRAINT FK_HopDong_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
)
GO
CREATE INDEX IX_HopDong_TrangThai ON HopDong(TrangThai, NgayBatDau) WHERE IsDeleted = 0;
CREATE INDEX IX_HopDong_KhachHang ON HopDong(MaKH) WHERE IsDeleted = 0;
GO

-- Bảng lịch sử phiên bản hợp đồng (versioning) - lưu bản ghi TRƯỚC khi sửa
CREATE TABLE HopDong_LichSu
(
    MaLichSu        INT IDENTITY PRIMARY KEY,
    MaHD            INT NOT NULL,
    SoPhienBan      INT NOT NULL,
    DuLieuCu        NVARCHAR(MAX) NOT NULL,   -- JSON snapshot bản ghi cũ
    NguoiSua        INT NULL,
    ThoiGianSua     DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_HopDongLS_HopDong FOREIGN KEY (MaHD) REFERENCES HopDong(MaHD),
    CONSTRAINT FK_HopDongLS_TaiKhoan FOREIGN KEY (NguoiSua) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE INDEX IX_HopDongLS_MaHD ON HopDong_LichSu(MaHD);
GO

CREATE TABLE BoiThuong
(
    MaBT            INT IDENTITY PRIMARY KEY,
    MaHD            INT NOT NULL,
    MaNV            INT NULL,               -- NULL khi khách hàng vừa tạo yêu cầu, NV xử lý sẽ gán sau
    NgayYeuCau      DATE NOT NULL,
    LyDo            NVARCHAR(200) NOT NULL,
    SoTien          DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    TrangThai       NVARCHAR(50) NOT NULL DEFAULT N'Đang xử lý'
                    CHECK (TrangThai IN (N'Đang xử lý', N'Đã duyệt', N'Từ chối')),

    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NguoiTao        INT NULL,
    NgayCapNhat     DATETIME NULL,
    NguoiCapNhat    INT NULL,
    SoPhienBan      INT NOT NULL DEFAULT 1,
    IsDeleted       BIT NOT NULL DEFAULT 0,
    NgayXoa         DATETIME NULL,
    NguoiXoa        INT NULL,

    CONSTRAINT FK_BoiThuong_HopDong FOREIGN KEY (MaHD) REFERENCES HopDong(MaHD),
    CONSTRAINT FK_BoiThuong_NhanVien FOREIGN KEY (MaNV) REFERENCES NhanVien(MaNV)
)
GO
CREATE INDEX IX_BoiThuong_TrangThai ON BoiThuong(TrangThai) WHERE IsDeleted = 0;
GO

CREATE TABLE BoiThuong_LichSu
(
    MaLichSu        INT IDENTITY PRIMARY KEY,
    MaBT            INT NOT NULL,
    SoPhienBan      INT NOT NULL,
    DuLieuCu        NVARCHAR(MAX) NOT NULL,
    NguoiSua        INT NULL,
    ThoiGianSua     DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_BoiThuongLS_BoiThuong FOREIGN KEY (MaBT) REFERENCES BoiThuong(MaBT),
    CONSTRAINT FK_BoiThuongLS_TaiKhoan FOREIGN KEY (NguoiSua) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE INDEX IX_BoiThuongLS_MaBT ON BoiThuong_LichSu(MaBT);
GO

CREATE TABLE HoaDon
(
    MaHDN           INT IDENTITY PRIMARY KEY,
    MaHD            INT NOT NULL,
    NgayLap         DATE NOT NULL,
    SoTien          DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    TrangThai       NVARCHAR(50) NOT NULL DEFAULT N'Chưa thanh toán'
                    CHECK (TrangThai IN (N'Chưa thanh toán', N'Đã thanh toán', N'Quá hạn')),

    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NguoiTao        INT NULL,
    NgayCapNhat     DATETIME NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0,
    NgayXoa         DATETIME NULL,

    CONSTRAINT FK_HoaDon_HopDong FOREIGN KEY (MaHD) REFERENCES HopDong(MaHD)
)
GO
CREATE INDEX IX_HoaDon_TrangThai ON HoaDon(TrangThai) WHERE IsDeleted = 0;
GO

CREATE TABLE ThanhToan
(
    MaTT            INT IDENTITY PRIMARY KEY,
    MaHDN           INT NOT NULL,
    NgayThanhToan   DATE NOT NULL,
    SoTien          DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    PhuongThuc      NVARCHAR(50) NOT NULL CHECK (PhuongThuc IN (N'Tiền mặt', N'Chuyển khoản', N'Thẻ')),

    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NguoiTao        INT NULL,
    IsDeleted       BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_ThanhToan_HoaDon FOREIGN KEY (MaHDN) REFERENCES HoaDon(MaHDN)
)
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 03: Hạ tầng vận hành - Audit log, Thông báo, Hàng đợi Job
   ============================================================ */

-- Nhật ký thao tác: ai làm gì, lúc nào, dữ liệu trước/sau
CREATE TABLE AuditLog
(
    MaLog           BIGINT IDENTITY PRIMARY KEY,
    TenBang         NVARCHAR(100) NOT NULL,
    MaBanGhi        INT NOT NULL,
    HanhDong        VARCHAR(10) NOT NULL CHECK (HanhDong IN ('INSERT','UPDATE','DELETE')),
    DuLieuCu        NVARCHAR(MAX) NULL,   -- JSON
    DuLieuMoi       NVARCHAR(MAX) NULL,   -- JSON
    MaTK            INT NULL,             -- ai thực hiện (NULL nếu hệ thống)
    DiaChiIP        VARCHAR(50) NULL,
    ThoiGian        DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_AuditLog_TaiKhoan FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE INDEX IX_AuditLog_Bang_Ban_ghi ON AuditLog(TenBang, MaBanGhi);
CREATE INDEX IX_AuditLog_ThoiGian ON AuditLog(ThoiGian);
GO

-- Thông báo email / in-app
CREATE TABLE ThongBao
(
    MaTB            INT IDENTITY PRIMARY KEY,
    MaTK            INT NOT NULL,          -- người nhận
    TieuDe          NVARCHAR(200) NOT NULL,
    NoiDung         NVARCHAR(MAX) NOT NULL,
    Loai            VARCHAR(20) NOT NULL DEFAULT 'INAPP' CHECK (Loai IN ('INAPP','EMAIL','CA_HAI')),
    DaDoc           BIT NOT NULL DEFAULT 0,
    DaGui           BIT NOT NULL DEFAULT 0,
    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NgayGui         DATETIME NULL,
    CONSTRAINT FK_ThongBao_TaiKhoan FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE INDEX IX_ThongBao_MaTK ON ThongBao(MaTK, DaDoc);
GO

-- Hàng đợi tác vụ nền: gửi mail, xuất báo cáo lớn, xuất Excel/PDF...
CREATE TABLE HangDoiTacVu
(
    MaJob           BIGINT IDENTITY PRIMARY KEY,
    LoaiJob         VARCHAR(50) NOT NULL,     -- vd: 'GUI_EMAIL', 'XUAT_BAO_CAO_PDF', 'XUAT_EXCEL'
    DuLieuJob       NVARCHAR(MAX) NULL,       -- JSON tham số
    TrangThai       VARCHAR(20) NOT NULL DEFAULT 'PENDING'
                    CHECK (TrangThai IN ('PENDING','PROCESSING','DONE','FAILED')),
    SoLanThu        INT NOT NULL DEFAULT 0,
    LoiNeuCo        NVARCHAR(MAX) NULL,
    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),
    NgayXuLy        DATETIME NULL,
    NgayHoanThanh   DATETIME NULL
)
GO
CREATE INDEX IX_HangDoiTacVu_TrangThai ON HangDoiTacVu(TrangThai, NgayTao);
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 04: Khóa ngoại bổ sung cho cột audit (người tạo/sửa/xóa)
   ============================================================ */
ALTER TABLE HopDong ADD CONSTRAINT FK_HopDong_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES TaiKhoan(MaTK);
ALTER TABLE HopDong ADD CONSTRAINT FK_HopDong_NguoiCapNhat FOREIGN KEY (NguoiCapNhat) REFERENCES TaiKhoan(MaTK);
ALTER TABLE HopDong ADD CONSTRAINT FK_HopDong_NguoiXoa FOREIGN KEY (NguoiXoa) REFERENCES TaiKhoan(MaTK);

ALTER TABLE BoiThuong ADD CONSTRAINT FK_BoiThuong_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES TaiKhoan(MaTK);
ALTER TABLE BoiThuong ADD CONSTRAINT FK_BoiThuong_NguoiCapNhat FOREIGN KEY (NguoiCapNhat) REFERENCES TaiKhoan(MaTK);

ALTER TABLE HoaDon ADD CONSTRAINT FK_HoaDon_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES TaiKhoan(MaTK);
ALTER TABLE ThanhToan ADD CONSTRAINT FK_ThanhToan_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES TaiKhoan(MaTK);

ALTER TABLE NhanVien ADD CONSTRAINT FK_NhanVien_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES TaiKhoan(MaTK);
ALTER TABLE NhanVien ADD CONSTRAINT FK_NhanVien_NguoiCapNhat FOREIGN KEY (NguoiCapNhat) REFERENCES TaiKhoan(MaTK);
ALTER TABLE NhanVien ADD CONSTRAINT FK_NhanVien_NguoiXoa FOREIGN KEY (NguoiXoa) REFERENCES TaiKhoan(MaTK);

ALTER TABLE BoiThuong ADD CONSTRAINT FK_BoiThuong_NguoiXoa FOREIGN KEY (NguoiXoa) REFERENCES TaiKhoan(MaTK);

ALTER TABLE LoaiBaoHiem ADD CONSTRAINT FK_LoaiBaoHiem_NguoiTao FOREIGN KEY (NguoiTao) REFERENCES TaiKhoan(MaTK);
ALTER TABLE LoaiBaoHiem ADD CONSTRAINT FK_LoaiBaoHiem_NguoiCapNhat FOREIGN KEY (NguoiCapNhat) REFERENCES TaiKhoan(MaTK);
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 05: Views (dữ liệu tổng hợp cho báo cáo, đều lọc IsDeleted=0)
   ============================================================ */

CREATE OR ALTER VIEW vw_HopDong_ChiTiet AS
SELECT
    hd.MaHD, hd.NgayBatDau, hd.NgayKetThuc, hd.SoTien, hd.TrangThai,
    kh.MaKH, kh.HoTen AS TenKhachHang, kh.SoDienThoai AS SDT_KhachHang,
    lb.MaLoai, lb.TenLoai,
    nv.MaNV, nv.HoTen AS TenNhanVien
FROM HopDong hd
JOIN KhachHang kh   ON hd.MaKH = kh.MaKH
JOIN LoaiBaoHiem lb ON hd.MaLoai = lb.MaLoai
JOIN NhanVien nv    ON hd.MaNV = nv.MaNV
WHERE hd.IsDeleted = 0 AND kh.IsDeleted = 0
GO

CREATE OR ALTER VIEW vw_BoiThuong_ChiTiet AS
SELECT
    bt.MaBT, bt.NgayYeuCau, bt.LyDo, bt.SoTien, bt.TrangThai,
    hd.MaHD, kh.HoTen AS TenKhachHang, lb.TenLoai,
    nv.HoTen AS NhanVienXuLy
FROM BoiThuong bt
JOIN HopDong hd     ON bt.MaHD = hd.MaHD
JOIN KhachHang kh   ON hd.MaKH = kh.MaKH
JOIN LoaiBaoHiem lb ON hd.MaLoai = lb.MaLoai
LEFT JOIN NhanVien nv ON bt.MaNV = nv.MaNV
WHERE bt.IsDeleted = 0
GO

CREATE OR ALTER VIEW vw_HoaDon_ChiTiet AS
SELECT
    hdn.MaHDN, hdn.NgayLap, hdn.SoTien, hdn.TrangThai,
    hd.MaHD, kh.HoTen AS TenKhachHang, lb.TenLoai
FROM HoaDon hdn
JOIN HopDong hd     ON hdn.MaHD = hd.MaHD
JOIN KhachHang kh   ON hd.MaKH = kh.MaKH
JOIN LoaiBaoHiem lb ON hd.MaLoai = lb.MaLoai
WHERE hdn.IsDeleted = 0
GO

CREATE OR ALTER VIEW vw_DoanhThu_TheoThang AS
SELECT
    YEAR(tt.NgayThanhToan)  AS Nam,
    MONTH(tt.NgayThanhToan) AS Thang,
    SUM(tt.SoTien)          AS TongDoanhThu,
    COUNT(*)                AS SoLuotThanhToan
FROM ThanhToan tt
WHERE tt.IsDeleted = 0
GROUP BY YEAR(tt.NgayThanhToan), MONTH(tt.NgayThanhToan)
GO

CREATE OR ALTER VIEW vw_BoiThuong_TheoLoai AS
SELECT
    lb.TenLoai,
    COUNT(*) AS SoLuongYeuCau,
    SUM(CASE WHEN bt.TrangThai = N'Đã duyệt' THEN bt.SoTien ELSE 0 END) AS TongDaBoiThuong
FROM BoiThuong bt
JOIN HopDong hd ON bt.MaHD = hd.MaHD
JOIN LoaiBaoHiem lb ON hd.MaLoai = lb.MaLoai
WHERE bt.IsDeleted = 0
GROUP BY lb.TenLoai
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 06: Triggers
   - Audit log tự động cho các bảng nhạy cảm (INSERT/UPDATE/DELETE)
   - Versioning: lưu snapshot bản ghi cũ trước khi UPDATE (HopDong, BoiThuong)
   ============================================================ */

------------------------------------------------------------
-- 6.1 AUDIT LOG: HopDong
------------------------------------------------------------
CREATE OR ALTER TRIGGER trg_HopDong_Audit
ON HopDong
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
        INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuCu, DuLieuMoi, MaTK)
        SELECT 'HopDong', i.MaHD, 'UPDATE',
               (SELECT d.* FROM deleted d WHERE d.MaHD = i.MaHD FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
               (SELECT * FROM inserted x WHERE x.MaHD = i.MaHD FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
               i.NguoiCapNhat
        FROM inserted i;
    ELSE IF EXISTS (SELECT 1 FROM inserted)
        INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuMoi, MaTK)
        SELECT 'HopDong', i.MaHD, 'INSERT',
               (SELECT * FROM inserted x WHERE x.MaHD = i.MaHD FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
               i.NguoiTao
        FROM inserted i;
    ELSE
        INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuCu)
        SELECT 'HopDong', d.MaHD, 'DELETE',
               (SELECT * FROM deleted x WHERE x.MaHD = d.MaHD FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM deleted d;
END
GO

------------------------------------------------------------
-- 6.2 VERSIONING: HopDong (lưu bản cũ trước khi update, +1 phiên bản)
------------------------------------------------------------
CREATE OR ALTER TRIGGER trg_HopDong_Versioning
ON HopDong
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO HopDong_LichSu (MaHD, SoPhienBan, DuLieuCu, NguoiSua)
    SELECT d.MaHD, d.SoPhienBan,
           (SELECT d2.* FROM deleted d2 WHERE d2.MaHD = d.MaHD FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
           i.NguoiCapNhat
    FROM deleted d
    JOIN inserted i ON i.MaHD = d.MaHD;

    UPDATE hd
    SET hd.SoPhienBan = hd.SoPhienBan + 1
    FROM HopDong hd
    JOIN inserted i ON i.MaHD = hd.MaHD;
END
GO

------------------------------------------------------------
-- 6.3 AUDIT LOG + VERSIONING: BoiThuong
------------------------------------------------------------
CREATE OR ALTER TRIGGER trg_BoiThuong_Audit
ON BoiThuong
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM inserted) AND EXISTS (SELECT 1 FROM deleted)
    BEGIN
        INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuCu, DuLieuMoi, MaTK)
        SELECT 'BoiThuong', i.MaBT, 'UPDATE',
               (SELECT d.* FROM deleted d WHERE d.MaBT = i.MaBT FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
               (SELECT * FROM inserted x WHERE x.MaBT = i.MaBT FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
               i.NguoiCapNhat
        FROM inserted i;

        INSERT INTO BoiThuong_LichSu (MaBT, SoPhienBan, DuLieuCu, NguoiSua)
        SELECT d.MaBT, d.SoPhienBan,
               (SELECT d2.* FROM deleted d2 WHERE d2.MaBT = d.MaBT FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
               i.NguoiCapNhat
        FROM deleted d JOIN inserted i ON i.MaBT = d.MaBT;

        UPDATE bt SET bt.SoPhienBan = bt.SoPhienBan + 1
        FROM BoiThuong bt JOIN inserted i ON i.MaBT = bt.MaBT;
    END
    ELSE IF EXISTS (SELECT 1 FROM inserted)
        INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuMoi, MaTK)
        SELECT 'BoiThuong', i.MaBT, 'INSERT',
               (SELECT * FROM inserted x WHERE x.MaBT = i.MaBT FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
               i.NguoiTao
        FROM inserted i;
    ELSE
        INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuCu)
        SELECT 'BoiThuong', d.MaBT, 'DELETE',
               (SELECT * FROM deleted x WHERE x.MaBT = d.MaBT FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
        FROM deleted d;
END
GO

------------------------------------------------------------
-- 6.4 AUDIT LOG: ThanhToan (khi ghi nhận thanh toán -> cũng cập nhật HoaDon)
------------------------------------------------------------
CREATE OR ALTER TRIGGER trg_ThanhToan_Audit
ON ThanhToan
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuMoi, MaTK)
    SELECT 'ThanhToan', i.MaTT, 'INSERT',
           (SELECT * FROM inserted x WHERE x.MaTT = i.MaTT FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
           i.NguoiTao
    FROM inserted i;

    -- Tự động cập nhật trạng thái Hóa đơn khi có thanh toán đủ số tiền
    UPDATE hdn
    SET hdn.TrangThai = N'Đã thanh toán', hdn.NgayCapNhat = GETDATE()
    FROM HoaDon hdn
    JOIN inserted i ON i.MaHDN = hdn.MaHDN
    WHERE hdn.SoTien <= (
        SELECT ISNULL(SUM(t.SoTien), 0) FROM ThanhToan t
        WHERE t.MaHDN = hdn.MaHDN AND t.IsDeleted = 0
    );
END
GO

------------------------------------------------------------
-- 6.5 AUDIT LOG: TaiKhoan (không log cột MatKhauHash ra JSON để tránh lộ hash)
------------------------------------------------------------
CREATE OR ALTER TRIGGER trg_TaiKhoan_Audit
ON TaiKhoan
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuCu, DuLieuMoi)
    SELECT 'TaiKhoan', i.MaTK, 'UPDATE',
           (SELECT d.MaTK, d.TenDangNhap, d.Email, d.MaQuyen, d.KichHoat, d.Bat2FA
            FROM deleted d WHERE d.MaTK = i.MaTK FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
           (SELECT x.MaTK, x.TenDangNhap, x.Email, x.MaQuyen, x.KichHoat, x.Bat2FA
            FROM inserted x WHERE x.MaTK = i.MaTK FOR JSON PATH, WITHOUT_ARRAY_WRAPPER)
    FROM inserted i;
END
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 07a: Stored Procedures - Authentication & RBAC
   Ghi chú bảo mật: SP chỉ nhận MatKhauHash đã băm sẵn từ backend
   (BCrypt/Argon2...). SP KHÔNG băm và KHÔNG so sánh plaintext.
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_TaiKhoan_DangKy
    @TenDangNhap    VARCHAR(50),
    @MatKhauHash    VARCHAR(255),
    @Email          VARCHAR(100),
    @MaQuyen        INT,
    @MaXacMinhEmail VARCHAR(100),
    @MaTK_Moi       INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap OR Email = @Email)
    BEGIN
        RAISERROR(N'Tên đăng nhập hoặc email đã tồn tại', 16, 1);
        RETURN;
    END
    INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Email, MaQuyen, MaXacMinhEmail, KichHoat)
    VALUES (@TenDangNhap, @MatKhauHash, @Email, @MaQuyen, @MaXacMinhEmail, 0);
    SET @MaTK_Moi = SCOPE_IDENTITY();
END
GO

-- Trả về thông tin cần thiết để backend tự so khớp hash + kiểm tra khóa tài khoản
CREATE OR ALTER PROCEDURE sp_TaiKhoan_LayThongTinDangNhap
    @TenDangNhap VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT tk.MaTK, tk.TenDangNhap, tk.MatKhauHash, tk.Email, tk.MaQuyen,
           q.TenQuyen, tk.SoLanDangNhapSai, tk.KhoaDenNgay, tk.KichHoat,
           tk.Bat2FA, tk.MaBiMat2FA, tk.DaXacMinhEmail
    FROM TaiKhoan tk
    JOIN Quyen q ON tk.MaQuyen = q.MaQuyen
    WHERE tk.TenDangNhap = @TenDangNhap AND tk.IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_TaiKhoan_GhiNhanDangNhapThatBai
    @MaTK INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TaiKhoan
    SET SoLanDangNhapSai = SoLanDangNhapSai + 1,
        KhoaDenNgay = CASE WHEN SoLanDangNhapSai + 1 >= 5
                           THEN DATEADD(MINUTE, 15, GETDATE()) ELSE KhoaDenNgay END
    WHERE MaTK = @MaTK;
END
GO

CREATE OR ALTER PROCEDURE sp_TaiKhoan_GhiNhanDangNhapThanhCong
    @MaTK INT, @DiaChiIP VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TaiKhoan
    SET SoLanDangNhapSai = 0, KhoaDenNgay = NULL,
        LanDangNhapCuoi = GETDATE(), IPDangNhapCuoi = @DiaChiIP
    WHERE MaTK = @MaTK;
END
GO

CREATE OR ALTER PROCEDURE sp_RefreshToken_Luu
    @MaTK INT, @Token VARCHAR(500), @NgayHetHan DATETIME,
    @ThietBi NVARCHAR(200) = NULL, @DiaChiIP VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO RefreshToken (MaTK, Token, NgayHetHan, ThietBi, DiaChiIP)
    VALUES (@MaTK, @Token, @NgayHetHan, @ThietBi, @DiaChiIP);
END
GO

CREATE OR ALTER PROCEDURE sp_RefreshToken_KiemTra
    @Token VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT rt.MaRefreshToken, rt.MaTK, rt.NgayHetHan, rt.DaThuHoi, tk.MaQuyen
    FROM RefreshToken rt
    JOIN TaiKhoan tk ON tk.MaTK = rt.MaTK
    WHERE rt.Token = @Token AND rt.DaThuHoi = 0 AND rt.NgayHetHan > GETDATE()
          AND tk.IsDeleted = 0 AND tk.KichHoat = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_RefreshToken_ThuHoi
    @Token VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RefreshToken SET DaThuHoi = 1 WHERE Token = @Token;
END
GO

CREATE OR ALTER PROCEDURE sp_TaiKhoan_TaoTokenQuenMatKhau
    @Email VARCHAR(100), @Token VARCHAR(200), @PhutHetHan INT = 30
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaTK INT = (SELECT MaTK FROM TaiKhoan WHERE Email = @Email AND IsDeleted = 0);
    IF @MaTK IS NULL
    BEGIN
        RAISERROR(N'Email không tồn tại', 16, 1);
        RETURN;
    END
    INSERT INTO PasswordResetToken (MaTK, Token, NgayHetHan)
    VALUES (@MaTK, @Token, DATEADD(MINUTE, @PhutHetHan, GETDATE()));

    -- đẩy vào hàng đợi job để gửi email (xử lý bất đồng bộ)
    INSERT INTO HangDoiTacVu (LoaiJob, DuLieuJob)
    VALUES ('GUI_EMAIL_QUEN_MK', (SELECT @Email AS Email, @Token AS Token FOR JSON PATH, WITHOUT_ARRAY_WRAPPER));
END
GO

CREATE OR ALTER PROCEDURE sp_TaiKhoan_DatLaiMatKhau
    @Token VARCHAR(200), @MatKhauHashMoi VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @MaTK INT;
    SELECT @MaTK = MaTK FROM PasswordResetToken
    WHERE Token = @Token AND DaSuDung = 0 AND NgayHetHan > GETDATE();

    IF @MaTK IS NULL
    BEGIN
        RAISERROR(N'Token không hợp lệ hoặc đã hết hạn', 16, 1);
        RETURN;
    END

    UPDATE TaiKhoan SET MatKhauHash = @MatKhauHashMoi, NgayCapNhat = GETDATE() WHERE MaTK = @MaTK;
    UPDATE PasswordResetToken SET DaSuDung = 1 WHERE Token = @Token;
END
GO

CREATE OR ALTER PROCEDURE sp_TaiKhoan_DoiMatKhau
    @MaTK INT, @MatKhauHashMoi VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TaiKhoan SET MatKhauHash = @MatKhauHashMoi, NgayCapNhat = GETDATE() WHERE MaTK = @MaTK;
END
GO

-- Lấy toàn bộ quyền theo màn hình của 1 role (dùng để dựng menu + guard theo hành động)
CREATE OR ALTER PROCEDURE sp_PhanQuyen_LayTheoQuyen
    @MaQuyen INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT cn.MaChucNang, cn.TenChucNang, cn.DuongDan, cn.MaChucNangCha,
           pq.DuocXem, pq.DuocThem, pq.DuocSua, pq.DuocXoa, pq.DuocXuatFile
    FROM ChucNang cn
    LEFT JOIN PhanQuyen pq ON pq.MaChucNang = cn.MaChucNang AND pq.MaQuyen = @MaQuyen
    WHERE cn.IsDeleted = 0
    ORDER BY cn.MaChucNangCha, cn.MaChucNang;
END
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 07b: SP cho KhachHang / NhanVien / LoaiBaoHiem
   Mẫu chung: GetPaged (@Keyword,@PageIndex,@PageSize,@SortColumn,@SortDir,@TotalCount OUT)
   Sắp xếp dùng CASE...WHEN thay vì SQL động -> chống SQL Injection.
   ============================================================ */

------------------------------------------------------------
-- KHÁCH HÀNG
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_KhachHang_GetPaged
    @Keyword     NVARCHAR(100) = NULL,
    @PageIndex   INT = 1,
    @PageSize    INT = 20,
    @SortColumn  VARCHAR(30) = 'MaKH',
    @SortDir     VARCHAR(4)  = 'ASC',
    @TotalCount  INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*)
    FROM KhachHang
    WHERE IsDeleted = 0
      AND (@Keyword IS NULL OR HoTen LIKE '%'+@Keyword+'%'
           OR SoDienThoai LIKE '%'+@Keyword+'%' OR CCCD LIKE '%'+@Keyword+'%');

    SELECT MaKH, HoTen, SoDienThoai, DiaChi, CCCD, NgaySinh, NgayTao
    FROM KhachHang
    WHERE IsDeleted = 0
      AND (@Keyword IS NULL OR HoTen LIKE '%'+@Keyword+'%'
           OR SoDienThoai LIKE '%'+@Keyword+'%' OR CCCD LIKE '%'+@Keyword+'%')
    ORDER BY
        CASE WHEN @SortColumn = 'HoTen'   AND @SortDir = 'ASC'  THEN HoTen END ASC,
        CASE WHEN @SortColumn = 'HoTen'   AND @SortDir = 'DESC' THEN HoTen END DESC,
        CASE WHEN @SortColumn = 'NgayTao' AND @SortDir = 'ASC'  THEN NgayTao END ASC,
        CASE WHEN @SortColumn = 'NgayTao' AND @SortDir = 'DESC' THEN NgayTao END DESC,
        MaKH ASC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE sp_KhachHang_ThemMoi
    @MaTK INT = NULL, @HoTen NVARCHAR(100), @SoDienThoai VARCHAR(20),
    @DiaChi NVARCHAR(200), @CCCD VARCHAR(20), @NgaySinh DATE = NULL,
    @NguoiTao INT, @MaKH_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM KhachHang WHERE CCCD = @CCCD AND IsDeleted = 0)
    BEGIN
        RAISERROR(N'CCCD đã tồn tại', 16, 1);
        RETURN;
    END
    INSERT INTO KhachHang (MaTK, HoTen, SoDienThoai, DiaChi, CCCD, NgaySinh, NguoiTao)
    VALUES (@MaTK, @HoTen, @SoDienThoai, @DiaChi, @CCCD, @NgaySinh, @NguoiTao);
    SET @MaKH_Moi = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_KhachHang_CapNhat
    @MaKH INT, @HoTen NVARCHAR(100), @SoDienThoai VARCHAR(20),
    @DiaChi NVARCHAR(200), @NgaySinh DATE = NULL, @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE KhachHang
    SET HoTen = @HoTen, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi,
        NgaySinh = @NgaySinh, NguoiCapNhat = @NguoiCapNhat, NgayCapNhat = GETDATE(),
        SoPhienBan = SoPhienBan + 1
    WHERE MaKH = @MaKH AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_KhachHang_XoaMem
    @MaKH INT, @NguoiXoa INT
AS
BEGIN
    SET NOCOUNT ON;
    IF EXISTS (SELECT 1 FROM HopDong WHERE MaKH = @MaKH AND IsDeleted = 0 AND TrangThai = N'Đang hoạt động')
    BEGIN
        RAISERROR(N'Không thể xóa: khách hàng còn hợp đồng đang hoạt động', 16, 1);
        RETURN;
    END
    UPDATE KhachHang SET IsDeleted = 1, NgayXoa = GETDATE(), NguoiXoa = @NguoiXoa WHERE MaKH = @MaKH;
END
GO

CREATE OR ALTER PROCEDURE sp_KhachHang_KhoiPhuc
    @MaKH INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE KhachHang SET IsDeleted = 0, NgayXoa = NULL, NguoiXoa = NULL WHERE MaKH = @MaKH;
END
GO

------------------------------------------------------------
-- NHÂN VIÊN
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_NhanVien_GetPaged
    @Keyword NVARCHAR(100) = NULL, @PageIndex INT = 1, @PageSize INT = 20,
    @SortColumn VARCHAR(30) = 'MaNV', @SortDir VARCHAR(4) = 'ASC',
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM NhanVien
    WHERE IsDeleted = 0 AND (@Keyword IS NULL OR HoTen LIKE '%'+@Keyword+'%' OR SoDienThoai LIKE '%'+@Keyword+'%');

    SELECT MaNV, HoTen, SoDienThoai, DiaChi, ChucVu, NgayTao
    FROM NhanVien
    WHERE IsDeleted = 0 AND (@Keyword IS NULL OR HoTen LIKE '%'+@Keyword+'%' OR SoDienThoai LIKE '%'+@Keyword+'%')
    ORDER BY
        CASE WHEN @SortColumn = 'HoTen' AND @SortDir = 'ASC'  THEN HoTen END ASC,
        CASE WHEN @SortColumn = 'HoTen' AND @SortDir = 'DESC' THEN HoTen END DESC,
        MaNV ASC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE sp_NhanVien_ThemMoi
    @MaTK INT = NULL, @HoTen NVARCHAR(100), @SoDienThoai VARCHAR(20),
    @DiaChi NVARCHAR(200), @ChucVu NVARCHAR(50), @NguoiTao INT, @MaNV_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO NhanVien (MaTK, HoTen, SoDienThoai, DiaChi, ChucVu, NguoiTao)
    VALUES (@MaTK, @HoTen, @SoDienThoai, @DiaChi, @ChucVu, @NguoiTao);
    SET @MaNV_Moi = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_NhanVien_CapNhat
    @MaNV INT, @HoTen NVARCHAR(100), @SoDienThoai VARCHAR(20),
    @DiaChi NVARCHAR(200), @ChucVu NVARCHAR(50), @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE NhanVien
    SET HoTen = @HoTen, SoDienThoai = @SoDienThoai, DiaChi = @DiaChi, ChucVu = @ChucVu,
        NguoiCapNhat = @NguoiCapNhat, NgayCapNhat = GETDATE(), SoPhienBan = SoPhienBan + 1
    WHERE MaNV = @MaNV AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_NhanVien_XoaMem
    @MaNV INT, @NguoiXoa INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE NhanVien SET IsDeleted = 1, NgayXoa = GETDATE(), NguoiXoa = @NguoiXoa WHERE MaNV = @MaNV;
END
GO

------------------------------------------------------------
-- LOẠI BẢO HIỂM
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_LoaiBaoHiem_GetPaged
    @Keyword NVARCHAR(100) = NULL, @PageIndex INT = 1, @PageSize INT = 20,
    @SortColumn VARCHAR(30) = 'MaLoai', @SortDir VARCHAR(4) = 'ASC',
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM LoaiBaoHiem
    WHERE IsDeleted = 0 AND (@Keyword IS NULL OR TenLoai LIKE '%'+@Keyword+'%');

    SELECT MaLoai, TenLoai, MoTa, MucPhi
    FROM LoaiBaoHiem
    WHERE IsDeleted = 0 AND (@Keyword IS NULL OR TenLoai LIKE '%'+@Keyword+'%')
    ORDER BY
        CASE WHEN @SortColumn = 'MucPhi' AND @SortDir = 'ASC'  THEN MucPhi END ASC,
        CASE WHEN @SortColumn = 'MucPhi' AND @SortDir = 'DESC' THEN MucPhi END DESC,
        MaLoai ASC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE sp_LoaiBaoHiem_ThemMoi
    @TenLoai NVARCHAR(100), @MoTa NVARCHAR(200), @MucPhi DECIMAL(18,2),
    @NguoiTao INT, @MaLoai_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO LoaiBaoHiem (TenLoai, MoTa, MucPhi, NguoiTao)
    VALUES (@TenLoai, @MoTa, @MucPhi, @NguoiTao);
    SET @MaLoai_Moi = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_LoaiBaoHiem_CapNhat
    @MaLoai INT, @TenLoai NVARCHAR(100), @MoTa NVARCHAR(200), @MucPhi DECIMAL(18,2), @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE LoaiBaoHiem
    SET TenLoai = @TenLoai, MoTa = @MoTa, MucPhi = @MucPhi,
        NguoiCapNhat = @NguoiCapNhat, NgayCapNhat = GETDATE()
    WHERE MaLoai = @MaLoai AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_LoaiBaoHiem_XoaMem
    @MaLoai INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE LoaiBaoHiem SET IsDeleted = 1, NgayXoa = GETDATE() WHERE MaLoai = @MaLoai;
END
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 07c: SP nghiệp vụ chính
   ============================================================ */

------------------------------------------------------------
-- HỢP ĐỒNG
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_HopDong_GetPaged
    @Keyword     NVARCHAR(100) = NULL,   -- tìm theo tên khách hàng
    @TrangThai   NVARCHAR(50) = NULL,
    @TuNgay      DATE = NULL,
    @DenNgay     DATE = NULL,
    @PageIndex   INT = 1,
    @PageSize    INT = 20,
    @SortColumn  VARCHAR(30) = 'MaHD',
    @SortDir     VARCHAR(4)  = 'DESC',
    @TotalCount  INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*)
    FROM vw_HopDong_ChiTiet
    WHERE (@Keyword   IS NULL OR TenKhachHang LIKE '%'+@Keyword+'%')
      AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
      AND (@TuNgay    IS NULL OR NgayBatDau >= @TuNgay)
      AND (@DenNgay   IS NULL OR NgayKetThuc <= @DenNgay);

    SELECT *
    FROM vw_HopDong_ChiTiet
    WHERE (@Keyword   IS NULL OR TenKhachHang LIKE '%'+@Keyword+'%')
      AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
      AND (@TuNgay    IS NULL OR NgayBatDau >= @TuNgay)
      AND (@DenNgay   IS NULL OR NgayKetThuc <= @DenNgay)
    ORDER BY
        CASE WHEN @SortColumn = 'NgayBatDau' AND @SortDir = 'ASC'  THEN NgayBatDau END ASC,
        CASE WHEN @SortColumn = 'NgayBatDau' AND @SortDir = 'DESC' THEN NgayBatDau END DESC,
        CASE WHEN @SortColumn = 'SoTien'     AND @SortDir = 'ASC'  THEN SoTien END ASC,
        CASE WHEN @SortColumn = 'SoTien'     AND @SortDir = 'DESC' THEN SoTien END DESC,
        CASE WHEN @SortColumn = 'MaHD'       AND @SortDir = 'ASC'  THEN MaHD END ASC,
        MaHD DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE sp_HopDong_TaoMoi
    @MaKH INT, @MaLoai INT, @MaNV INT, @NgayBatDau DATE, @NgayKetThuc DATE,
    @SoTien DECIMAL(18,2), @ChuKyDongPhi NVARCHAR(20) = N'Một lần',
    @NguoiTao INT, @MaHD_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @NgayKetThuc <= @NgayBatDau
        THROW 50001, N'Ngày kết thúc phải lớn hơn ngày bắt đầu', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO HopDong
            (MaKH, MaLoai, MaNV, NgayBatDau, NgayKetThuc, SoTien, ChuKyDongPhi, NguoiTao)
        VALUES
            (@MaKH, @MaLoai, @MaNV, @NgayBatDau, @NgayKetThuc, @SoTien, @ChuKyDongPhi, @NguoiTao);

        SET @MaHD_Moi = SCOPE_IDENTITY();

        -- Chỉ tạo hóa đơn ngay với hợp đồng đóng một lần.
        IF @ChuKyDongPhi = N'Một lần'
        BEGIN
            INSERT INTO HoaDon (MaHD, NgayLap, SoTien, NguoiTao)
            VALUES (@MaHD_Moi, @NgayBatDau, @SoTien, @NguoiTao);
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_HopDong_CapNhat
    @MaHD INT, @NgayKetThuc DATE, @SoTien DECIMAL(18,2), @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE HopDong
    SET NgayKetThuc = @NgayKetThuc, SoTien = @SoTien,
        NguoiCapNhat = @NguoiCapNhat, NgayCapNhat = GETDATE()
    WHERE MaHD = @MaHD AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_HopDong_GiaHan
    @MaHD INT, @NgayKetThucMoi DATE, @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE HopDong
    SET NgayKetThuc = @NgayKetThucMoi, TrangThai = N'Đang hoạt động',
        NguoiCapNhat = @NguoiCapNhat, NgayCapNhat = GETDATE()
    WHERE MaHD = @MaHD AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_HopDong_Huy
    @MaHD INT, @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE HopDong
    SET TrangThai = N'Đã hủy',
        NguoiCapNhat = @NguoiCapNhat,
        NgayCapNhat = GETDATE()
    WHERE MaHD = @MaHD AND IsDeleted = 0;
END
GO

-- Job định kỳ: tự cập nhật hợp đồng hết hạn (gọi từ SQL Agent job / background service)
CREATE OR ALTER PROCEDURE sp_HopDong_CapNhatHetHan
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE HopDong
    SET TrangThai = N'Hết hạn', NgayCapNhat = GETDATE()
    WHERE TrangThai = N'Đang hoạt động' AND NgayKetThuc < CAST(GETDATE() AS DATE) AND IsDeleted = 0;
END
GO

------------------------------------------------------------
-- BỒI THƯỜNG
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_BoiThuong_GetPaged
    @Keyword NVARCHAR(100) = NULL, @TrangThai NVARCHAR(50) = NULL,
    @PageIndex INT = 1, @PageSize INT = 20,
    @SortColumn VARCHAR(30) = 'MaBT', @SortDir VARCHAR(4) = 'DESC',
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM vw_BoiThuong_ChiTiet
    WHERE (@Keyword IS NULL OR TenKhachHang LIKE '%'+@Keyword+'%')
      AND (@TrangThai IS NULL OR TrangThai = @TrangThai);

    SELECT * FROM vw_BoiThuong_ChiTiet
    WHERE (@Keyword IS NULL OR TenKhachHang LIKE '%'+@Keyword+'%')
      AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
    ORDER BY
        CASE WHEN @SortColumn = 'NgayYeuCau' AND @SortDir='ASC'  THEN NgayYeuCau END ASC,
        CASE WHEN @SortColumn = 'NgayYeuCau' AND @SortDir='DESC' THEN NgayYeuCau END DESC,
        MaBT DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE sp_BoiThuong_TaoYeuCau
    @MaHD INT, @NgayYeuCau DATE, @LyDo NVARCHAR(200), @SoTien DECIMAL(18,2),
    @NguoiTao INT, @MaBT_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM HopDong WHERE MaHD = @MaHD AND TrangThai = N'Đang hoạt động' AND IsDeleted = 0)
    BEGIN
        RAISERROR(N'Hợp đồng không còn hiệu lực để yêu cầu bồi thường', 16, 1);
        RETURN;
    END
    INSERT INTO BoiThuong (MaHD, NgayYeuCau, LyDo, SoTien, NguoiTao)
    VALUES (@MaHD, @NgayYeuCau, @LyDo, @SoTien, @NguoiTao);
    SET @MaBT_Moi = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_BoiThuong_Duyet
    @MaBT INT, @MaNV_XuLy INT, @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE BoiThuong
    SET TrangThai = N'Đã duyệt', MaNV = @MaNV_XuLy,
        NguoiCapNhat = @NguoiCapNhat, NgayCapNhat = GETDATE()
    WHERE MaBT = @MaBT AND TrangThai = N'Đang xử lý' AND IsDeleted = 0;
END
GO

CREATE OR ALTER PROCEDURE sp_BoiThuong_TuChoi
    @MaBT INT, @MaNV_XuLy INT, @NguoiCapNhat INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE BoiThuong
    SET TrangThai = N'Từ chối', MaNV = @MaNV_XuLy,
        NguoiCapNhat = @NguoiCapNhat, NgayCapNhat = GETDATE()
    WHERE MaBT = @MaBT AND TrangThai = N'Đang xử lý' AND IsDeleted = 0;
END
GO

------------------------------------------------------------
-- HÓA ĐƠN
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_HoaDon_GetPaged
    @Keyword NVARCHAR(100) = NULL, @TrangThai NVARCHAR(50) = NULL,
    @PageIndex INT = 1, @PageSize INT = 20,
    @SortColumn VARCHAR(30) = 'MaHDN', @SortDir VARCHAR(4) = 'DESC',
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM vw_HoaDon_ChiTiet
    WHERE (@Keyword IS NULL OR TenKhachHang LIKE '%'+@Keyword+'%')
      AND (@TrangThai IS NULL OR TrangThai = @TrangThai);

    SELECT * FROM vw_HoaDon_ChiTiet
    WHERE (@Keyword IS NULL OR TenKhachHang LIKE '%'+@Keyword+'%')
      AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
    ORDER BY
        CASE WHEN @SortColumn = 'NgayLap' AND @SortDir='ASC'  THEN NgayLap END ASC,
        CASE WHEN @SortColumn = 'NgayLap' AND @SortDir='DESC' THEN NgayLap END DESC,
        MaHDN DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

------------------------------------------------------------
-- THANH TOÁN
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_ThanhToan_GetPaged
    @PageIndex INT = 1, @PageSize INT = 20,
    @SortColumn VARCHAR(30) = 'MaTT', @SortDir VARCHAR(4) = 'DESC',
    @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM ThanhToan WHERE IsDeleted = 0;

    SELECT tt.MaTT, tt.NgayThanhToan, tt.SoTien, tt.PhuongThuc,
           hdn.MaHDN, hd.MaHD, kh.HoTen AS TenKhachHang
    FROM ThanhToan tt
    JOIN HoaDon hdn ON tt.MaHDN = hdn.MaHDN
    JOIN HopDong hd ON hdn.MaHD = hd.MaHD
    JOIN KhachHang kh ON hd.MaKH = kh.MaKH
    WHERE tt.IsDeleted = 0
    ORDER BY
        CASE WHEN @SortColumn = 'NgayThanhToan' AND @SortDir='ASC'  THEN tt.NgayThanhToan END ASC,
        CASE WHEN @SortColumn = 'NgayThanhToan' AND @SortDir='DESC' THEN tt.NgayThanhToan END DESC,
        tt.MaTT DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE sp_ThanhToan_GhiNhan
    @MaHDN INT, @NgayThanhToan DATE, @SoTien DECIMAL(18,2), @PhuongThuc NVARCHAR(50),
    @NguoiTao INT, @MaTT_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @SoTien <= 0
        THROW 50002, N'Số tiền thanh toán phải lớn hơn 0', 1;

    DECLARE @TienHoaDon DECIMAL(18,2),
            @DaThanhToan DECIMAL(18,2);

    SELECT @TienHoaDon = SoTien
    FROM HoaDon
    WHERE MaHDN = @MaHDN AND IsDeleted = 0;

    IF @TienHoaDon IS NULL
        THROW 50003, N'Hóa đơn không tồn tại', 1;

    SELECT @DaThanhToan = ISNULL(SUM(SoTien),0)
    FROM ThanhToan
    WHERE MaHDN = @MaHDN AND IsDeleted = 0;

    IF @DaThanhToan + @SoTien > @TienHoaDon
        THROW 50004, N'Số tiền thanh toán vượt quá số tiền hóa đơn', 1;

    INSERT INTO ThanhToan (MaHDN, NgayThanhToan, SoTien, PhuongThuc, NguoiTao)
    VALUES (@MaHDN, @NgayThanhToan, @SoTien, @PhuongThuc, @NguoiTao);

    SET @MaTT_Moi = SCOPE_IDENTITY();
END
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 07d: SP cho Thông báo / Hàng đợi Job / Tra cứu Audit log
   ============================================================ */

CREATE OR ALTER PROCEDURE sp_ThongBao_Tao
    @MaTK INT, @TieuDe NVARCHAR(200), @NoiDung NVARCHAR(MAX), @Loai VARCHAR(20) = 'INAPP'
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO ThongBao (MaTK, TieuDe, NoiDung, Loai)
    VALUES (@MaTK, @TieuDe, @NoiDung, @Loai);

    IF @Loai IN ('EMAIL','CA_HAI')
        INSERT INTO HangDoiTacVu (LoaiJob, DuLieuJob)
        VALUES ('GUI_EMAIL_THONG_BAO',
                (SELECT @MaTK AS MaTK, @TieuDe AS TieuDe, @NoiDung AS NoiDung FOR JSON PATH, WITHOUT_ARRAY_WRAPPER));
END
GO

CREATE OR ALTER PROCEDURE sp_ThongBao_LayTheoTaiKhoan
    @MaTK INT, @ChiChuaDoc BIT = 0, @PageIndex INT = 1, @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaTB, TieuDe, NoiDung, Loai, DaDoc, NgayTao
    FROM ThongBao
    WHERE MaTK = @MaTK AND (@ChiChuaDoc = 0 OR DaDoc = 0)
    ORDER BY NgayTao DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE sp_ThongBao_DanhDauDaDoc
    @MaTB INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ThongBao SET DaDoc = 1 WHERE MaTB = @MaTB;
END
GO

------------------------------------------------------------
-- HÀNG ĐỢI TÁC VỤ (worker service sẽ poll các proc này)
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_HangDoiTacVu_Them
    @LoaiJob VARCHAR(50), @DuLieuJob NVARCHAR(MAX), @MaJob_Moi BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO HangDoiTacVu (LoaiJob, DuLieuJob) VALUES (@LoaiJob, @DuLieuJob);
    SET @MaJob_Moi = SCOPE_IDENTITY();
END
GO

-- Lấy 1 job PENDING kế tiếp và khóa nó lại (an toàn khi có nhiều worker chạy song song)
CREATE OR ALTER PROCEDURE sp_HangDoiTacVu_LayJobTiepTheo
AS
BEGIN
    SET NOCOUNT ON;
    ;WITH cte AS (
        SELECT TOP (1) *
        FROM HangDoiTacVu WITH (ROWLOCK, READPAST)
        WHERE TrangThai = 'PENDING'
        ORDER BY NgayTao ASC
    )
    UPDATE cte
    SET TrangThai = 'PROCESSING', NgayXuLy = GETDATE()
    OUTPUT inserted.MaJob, inserted.LoaiJob, inserted.DuLieuJob, inserted.SoLanThu;
END
GO

CREATE OR ALTER PROCEDURE sp_HangDoiTacVu_CapNhatKetQua
    @MaJob BIGINT, @ThanhCong BIT, @Loi NVARCHAR(MAX) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE HangDoiTacVu
    SET TrangThai = CASE WHEN @ThanhCong = 1 THEN 'DONE'
                          WHEN SoLanThu + 1 >= 5 THEN 'FAILED'
                          ELSE 'PENDING' END,
        SoLanThu = SoLanThu + 1,
        LoiNeuCo = @Loi,
        NgayHoanThanh = CASE WHEN @ThanhCong = 1 THEN GETDATE() ELSE NULL END
    WHERE MaJob = @MaJob;
END
GO

------------------------------------------------------------
-- TRA CỨU AUDIT LOG
------------------------------------------------------------
CREATE OR ALTER PROCEDURE sp_AuditLog_TraCuu
    @TenBang NVARCHAR(100) = NULL, @MaBanGhi INT = NULL, @MaTK INT = NULL,
    @TuNgay DATETIME = NULL, @DenNgay DATETIME = NULL,
    @PageIndex INT = 1, @PageSize INT = 50, @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM AuditLog
    WHERE (@TenBang IS NULL OR TenBang = @TenBang)
      AND (@MaBanGhi IS NULL OR MaBanGhi = @MaBanGhi)
      AND (@MaTK IS NULL OR MaTK = @MaTK)
      AND (@TuNgay IS NULL OR ThoiGian >= @TuNgay)
      AND (@DenNgay IS NULL OR ThoiGian <= @DenNgay);

    SELECT al.MaLog, al.TenBang, al.MaBanGhi, al.HanhDong, al.DuLieuCu, al.DuLieuMoi,
           al.ThoiGian, tk.TenDangNhap
    FROM AuditLog al
    LEFT JOIN TaiKhoan tk ON al.MaTK = tk.MaTK
    WHERE (@TenBang IS NULL OR al.TenBang = @TenBang)
      AND (@MaBanGhi IS NULL OR al.MaBanGhi = @MaBanGhi)
      AND (@MaTK IS NULL OR al.MaTK = @MaTK)
      AND (@TuNgay IS NULL OR al.ThoiGian >= @TuNgay)
      AND (@DenNgay IS NULL OR al.ThoiGian <= @DenNgay)
    ORDER BY al.ThoiGian DESC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 08: Seed danh mục cố định + tài khoản mẫu
   Lưu ý: MatKhauHash dưới đây dùng HASHBYTES('SHA2_256', ...) CHỈ để
   demo/test cho có dữ liệu chạy được. Khi tích hợp backend thật,
   hãy băm bằng BCrypt/Argon2 ở tầng ứng dụng và INSERT hash đó vào.
   Mật khẩu gốc của mọi tài khoản mẫu bên dưới đều là: 123456
   ============================================================ */

INSERT INTO Quyen (TenQuyen, MoTa) VALUES
(N'Admin',     N'Toàn quyền quản trị hệ thống'),
(N'Nhân viên', N'Xử lý nghiệp vụ hằng ngày'),
(N'Khách hàng',N'Tự quản lý hợp đồng, bồi thường, thanh toán của mình')
GO

INSERT INTO ChucNang (TenChucNang, DuongDan) VALUES
(N'Quản lý tài khoản và phân quyền', 'taikhoan'),
(N'Quản lý khách hàng',   'khachhang'),
(N'Quản lý nhân viên',    'nhanvien'),
(N'Quản lý loại bảo hiểm','loaibaohiem'),
(N'Quản lý hợp đồng',     'hopdong'),
(N'Quản lý bồi thường',   'boithuong'),
(N'Quản lý hóa đơn',      'hoadon'),
(N'Quản lý thanh toán',   'thanhtoan'),
(N'Xem báo cáo',          'baocao'),
(N'Nhật ký hệ thống',     'auditlog')
GO

-- Phân quyền: Admin full quyền mọi chức năng
INSERT INTO PhanQuyen (MaQuyen, MaChucNang, DuocXem, DuocThem, DuocSua, DuocXoa, DuocXuatFile)
SELECT 1, MaChucNang, 1, 1, 1, 1, 1 FROM ChucNang
GO

-- Nhân viên: xem/thêm/sửa nghiệp vụ, không xóa, không quản lý tài khoản/nhân viên/audit log
INSERT INTO PhanQuyen (MaQuyen, MaChucNang, DuocXem, DuocThem, DuocSua, DuocXoa, DuocXuatFile)
SELECT 2, MaChucNang,
    CASE WHEN TenChucNang IN (N'Quản lý tài khoản và phân quyền', N'Nhật ký hệ thống') THEN 0 ELSE 1 END,
    CASE WHEN TenChucNang IN (N'Quản lý tài khoản và phân quyền', N'Quản lý nhân viên', N'Nhật ký hệ thống', N'Xem báo cáo') THEN 0 ELSE 1 END,
    CASE WHEN TenChucNang IN (N'Quản lý tài khoản và phân quyền', N'Quản lý nhân viên', N'Nhật ký hệ thống', N'Xem báo cáo') THEN 0 ELSE 1 END,
    0, 1
FROM ChucNang
GO

-- Khách hàng: chỉ xem/thao tác trên hợp đồng, bồi thường, thanh toán của chính họ (lọc theo MaTK ở tầng API)
INSERT INTO PhanQuyen (MaQuyen, MaChucNang, DuocXem, DuocThem, DuocSua, DuocXoa, DuocXuatFile)
SELECT 3, MaChucNang,
    CASE WHEN TenChucNang IN (N'Quản lý hợp đồng', N'Quản lý bồi thường', N'Quản lý thanh toán') THEN 1 ELSE 0 END,
    CASE WHEN TenChucNang IN (N'Quản lý bồi thường', N'Quản lý thanh toán') THEN 1 ELSE 0 END,
    0, 0, 1
FROM ChucNang
GO

-- Tài khoản mẫu (mật khẩu gốc: 123456)
DECLARE @HashDemo VARCHAR(255) = '$2y$10$1qNUL335jEKoJIUnEYoYaerHGA.xc23he3htM4zR2oTExEwymzXoO';

INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Email, MaQuyen, KichHoat, DaXacMinhEmail)
VALUES
('admin',       @HashDemo, 'admin@gmail.com',    1, 1, 1),
('nhanvien01',  @HashDemo, 'nv01@gmail.com',     2, 1, 1),
('nhanvien02',  @HashDemo, 'nv02@gmail.com',     2, 1, 1),
('khachhang01', @HashDemo, 'kh01@gmail.com',     3, 1, 1),
('khachhang02', @HashDemo, 'kh02@gmail.com',     3, 1, 1)
GO

INSERT INTO NhanVien (MaTK, HoTen, SoDienThoai, DiaChi, ChucVu, NguoiTao) VALUES
(2, N'Lê Văn Nam',    '0901234567', N'Hưng Yên', N'Nhân viên', 1),
(3, N'Phạm Văn Minh', '0909876543', N'Hà Nội',   N'Nhân viên', 1)
GO

INSERT INTO KhachHang (MaTK, HoTen, SoDienThoai, DiaChi, CCCD, NguoiTao) VALUES
(4, N'Nguyễn Văn An',  '0912345678', N'Hưng Yên', '001234567890', 1),
(5, N'Trần Văn Bình',  '0987654321', N'Hà Nội',   '001234567891', 1)
GO

INSERT INTO LoaiBaoHiem (TenLoai, MoTa, MucPhi, NguoiTao) VALUES
(N'Bảo hiểm xe',       N'Bảo hiểm xe máy và ô tô',   500000,  1),
(N'Bảo hiểm nhà',      N'Bảo hiểm nhà ở',            3000000, 1),
(N'Bảo hiểm sức khỏe', N'Bảo hiểm khám chữa bệnh',   2000000, 1)
GO

INSERT INTO HopDong (MaKH, MaLoai, MaNV, NgayBatDau, NgayKetThuc, SoTien, TrangThai, NguoiTao) VALUES
(1, 1, 1, '2026-01-01', '2026-12-31', 500000,  N'Đang hoạt động', 1),
(1, 2, 2, '2026-02-01', '2027-01-31', 3000000, N'Đang hoạt động', 1),
(2, 3, 1, '2026-03-01', '2027-02-28', 2000000, N'Đang hoạt động', 1)
GO

INSERT INTO BoiThuong (MaHD, MaNV, NgayYeuCau, LyDo, SoTien, TrangThai, NguoiTao) VALUES
(1, 1, '2026-05-10', N'Tai nạn xe',  200000,  N'Đang xử lý', 4),
(2, 2, '2026-06-15', N'Cháy nhà',    1000000, N'Đã duyệt',   5)
GO

INSERT INTO HoaDon (MaHD, NgayLap, SoTien, TrangThai, NguoiTao) VALUES
(1, '2026-01-01', 500000,  N'Đã thanh toán',    1),
(2, '2026-02-01', 3000000, N'Đã thanh toán',    1),
(3, '2026-03-01', 2000000, N'Chưa thanh toán',  1)
GO

INSERT INTO ThanhToan (MaHDN, NgayThanhToan, SoTien, PhuongThuc, NguoiTao) VALUES
(1, '2026-01-01', 500000,  N'Chuyển khoản', 4),
(2, '2026-02-01', 3000000, N'Tiền mặt',     5)
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 10: Bổ sung theo đúng đề 28 - Quản lý bảo hiểm
   - Tài liệu đính kèm (upload hồ sơ hình ảnh cho hợp đồng/claim)
   - Lịch thu phí định kỳ + cảnh báo đến hạn
   ============================================================ */

------------------------------------------------------------
-- 10.1 TÀI LIỆU ĐÍNH KÈM (hồ sơ khách hàng, ảnh hiện trường claim...)
-- DB chỉ lưu METADATA + đường dẫn; file thật lưu ở cloud storage
-- (Azure Blob/S3) hoặc thư mục server, backend trả về DuongDanFile.
------------------------------------------------------------
CREATE TABLE TaiLieuDinhKem
(
    MaTaiLieu       INT IDENTITY PRIMARY KEY,
    LoaiThucThe     VARCHAR(20) NOT NULL CHECK (LoaiThucThe IN ('HopDong','BoiThuong','KhachHang')),
    MaThucThe       INT NOT NULL,             -- MaHD / MaBT / MaKH tương ứng
    TenFileGoc      NVARCHAR(255) NOT NULL,
    DuongDanFile    NVARCHAR(500) NOT NULL,   -- URL/path trên storage
    LoaiFile        VARCHAR(20) NOT NULL CHECK (LoaiFile IN ('image','pdf','other')),
    KichThuocByte   INT NULL,
    NguoiUpload     INT NOT NULL,
    NgayUpload      DATETIME NOT NULL DEFAULT GETDATE(),
    IsDeleted       BIT NOT NULL DEFAULT 0,
    NgayXoa         DATETIME NULL,
    CONSTRAINT FK_TaiLieu_NguoiUpload FOREIGN KEY (NguoiUpload) REFERENCES TaiKhoan(MaTK)
)
GO
CREATE INDEX IX_TaiLieu_ThucThe ON TaiLieuDinhKem(LoaiThucThe, MaThucThe) WHERE IsDeleted = 0;
GO

CREATE OR ALTER PROCEDURE sp_TaiLieu_Upload
    @LoaiThucThe VARCHAR(20), @MaThucThe INT, @TenFileGoc NVARCHAR(255),
    @DuongDanFile NVARCHAR(500), @LoaiFile VARCHAR(20), @KichThuocByte INT,
    @NguoiUpload INT, @MaTaiLieu_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @LoaiThucThe NOT IN ('HopDong','BoiThuong','KhachHang')
        THROW 50010, N'Loại thực thể không hợp lệ', 1;

    IF @LoaiThucThe = 'HopDong'
       AND NOT EXISTS (SELECT 1 FROM HopDong WHERE MaHD = @MaThucThe AND IsDeleted = 0)
        THROW 50011, N'Hợp đồng không tồn tại', 1;

    IF @LoaiThucThe = 'BoiThuong'
       AND NOT EXISTS (SELECT 1 FROM BoiThuong WHERE MaBT = @MaThucThe AND IsDeleted = 0)
        THROW 50012, N'Yêu cầu bồi thường không tồn tại', 1;

    IF @LoaiThucThe = 'KhachHang'
       AND NOT EXISTS (SELECT 1 FROM KhachHang WHERE MaKH = @MaThucThe AND IsDeleted = 0)
        THROW 50013, N'Khách hàng không tồn tại', 1;

    INSERT INTO TaiLieuDinhKem
        (LoaiThucThe, MaThucThe, TenFileGoc, DuongDanFile, LoaiFile, KichThuocByte, NguoiUpload)
    VALUES
        (@LoaiThucThe, @MaThucThe, @TenFileGoc, @DuongDanFile, @LoaiFile, @KichThuocByte, @NguoiUpload);

    SET @MaTaiLieu_Moi = SCOPE_IDENTITY();

    INSERT INTO AuditLog (TenBang, MaBanGhi, HanhDong, DuLieuMoi, MaTK)
    VALUES
    ('TaiLieuDinhKem', @MaTaiLieu_Moi, 'INSERT',
     (SELECT @LoaiThucThe AS LoaiThucThe, @MaThucThe AS MaThucThe, @TenFileGoc AS TenFileGoc
      FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
     @NguoiUpload);
END
GO

CREATE OR ALTER PROCEDURE sp_TaiLieu_LayTheoThucThe
    @LoaiThucThe VARCHAR(20), @MaThucThe INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaTaiLieu, TenFileGoc, DuongDanFile, LoaiFile, KichThuocByte, NgayUpload
    FROM TaiLieuDinhKem
    WHERE LoaiThucThe = @LoaiThucThe AND MaThucThe = @MaThucThe AND IsDeleted = 0
    ORDER BY NgayUpload DESC;
END
GO

CREATE OR ALTER PROCEDURE sp_TaiLieu_XoaMem
    @MaTaiLieu INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE TaiLieuDinhKem SET IsDeleted = 1, NgayXoa = GETDATE() WHERE MaTaiLieu = @MaTaiLieu;
END
GO

-- Lịch thu phí định kỳ: mỗi dòng là 1 kỳ đóng phí của 1 hợp đồng
CREATE TABLE LichThuPhi
(
    MaLichThu       INT IDENTITY PRIMARY KEY,
    MaHD            INT NOT NULL,
    KyThu           INT NOT NULL,             -- kỳ số 1, 2, 3...
    NgayDenHan      DATE NOT NULL,
    SoTien          DECIMAL(18,2) NOT NULL CHECK (SoTien >= 0),
    TrangThai       NVARCHAR(50) NOT NULL DEFAULT N'Chưa đến hạn'
                    CHECK (TrangThai IN (N'Chưa đến hạn', N'Đã thanh toán', N'Quá hạn')),
    MaHDN           INT NULL,                 -- hóa đơn được sinh khi đến hạn
    DaCanhBao       BIT NOT NULL DEFAULT 0,   -- đã gửi cảnh báo sắp đến hạn chưa
    NgayTao         DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT UQ_LichThuPhi UNIQUE (MaHD, KyThu),
    CONSTRAINT FK_LichThuPhi_HopDong FOREIGN KEY (MaHD) REFERENCES HopDong(MaHD),
    CONSTRAINT FK_LichThuPhi_HoaDon FOREIGN KEY (MaHDN) REFERENCES HoaDon(MaHDN)
)
GO
CREATE INDEX IX_LichThuPhi_DenHan ON LichThuPhi(NgayDenHan, TrangThai);
GO

-- Sinh lịch thu phí khi tạo hợp đồng có chu kỳ định kỳ (gọi ngay sau sp_HopDong_TaoMoi)
CREATE OR ALTER PROCEDURE sp_LichThuPhi_SinhLichChoHopDong
    @MaHD INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ChuKy NVARCHAR(20), @NgayBatDau DATE, @NgayKetThuc DATE, @SoTien DECIMAL(18,2);
    SELECT @ChuKy = ChuKyDongPhi, @NgayBatDau = NgayBatDau, @NgayKetThuc = NgayKetThuc, @SoTien = SoTien
    FROM HopDong WHERE MaHD = @MaHD;

    IF @ChuKy = N'Một lần' RETURN;   -- hợp đồng trả 1 lần đã có hóa đơn từ sp_HopDong_TaoMoi

    DECLARE @SoThangMoiKy INT = CASE @ChuKy WHEN N'Hàng tháng' THEN 1 WHEN N'Hàng quý' THEN 3 ELSE 12 END;
    DECLARE @SoKy INT = DATEDIFF(MONTH, @NgayBatDau, @NgayKetThuc) / @SoThangMoiKy;
    DECLARE @SoTienMoiKy DECIMAL(18,2) = ROUND(@SoTien / NULLIF(@SoKy, 0), 2);

    ;WITH Ky AS (
        SELECT TOP (@SoKy) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
        FROM sys.all_objects
    )
    INSERT INTO LichThuPhi (MaHD, KyThu, NgayDenHan, SoTien)
    SELECT @MaHD, n, DATEADD(MONTH, n * @SoThangMoiKy, @NgayBatDau), @SoTienMoiKy
    FROM Ky;
END
GO
-- Sau khi bảng LichThuPhi và thủ tục sinh lịch đã tồn tại,
-- cập nhật lại thủ tục tạo hợp đồng để tự sinh lịch đóng phí định kỳ.
CREATE OR ALTER PROCEDURE sp_HopDong_TaoMoi
    @MaKH INT, @MaLoai INT, @MaNV INT, @NgayBatDau DATE, @NgayKetThuc DATE,
    @SoTien DECIMAL(18,2), @ChuKyDongPhi NVARCHAR(20) = N'Một lần',
    @NguoiTao INT, @MaHD_Moi INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @NgayKetThuc <= @NgayBatDau
        THROW 50001, N'Ngày kết thúc phải lớn hơn ngày bắt đầu', 1;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO HopDong
            (MaKH, MaLoai, MaNV, NgayBatDau, NgayKetThuc, SoTien, ChuKyDongPhi, NguoiTao)
        VALUES
            (@MaKH, @MaLoai, @MaNV, @NgayBatDau, @NgayKetThuc, @SoTien, @ChuKyDongPhi, @NguoiTao);

        SET @MaHD_Moi = SCOPE_IDENTITY();

        IF @ChuKyDongPhi = N'Một lần'
            INSERT INTO HoaDon (MaHD, NgayLap, SoTien, NguoiTao)
            VALUES (@MaHD_Moi, @NgayBatDau, @SoTien, @NguoiTao);
        ELSE
            EXEC sp_LichThuPhi_SinhLichChoHopDong @MaHD_Moi;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

-- Job chạy định kỳ (SQL Agent / background service gọi mỗi ngày):
-- 1) Sinh hóa đơn cho các kỳ vừa đến hạn  2) Đánh dấu quá hạn  3) Gửi cảnh báo sắp đến hạn
CREATE OR ALTER PROCEDURE sp_LichThuPhi_XuLyHangNgay
    @SoNgayCanhBaoTruoc INT = 7
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) Sinh hóa đơn cho các kỳ đã đến hạn hôm nay mà chưa có hóa đơn
    DECLARE @DenHan TABLE (MaLichThu INT, MaHD INT, NgayDenHan DATE, SoTien DECIMAL(18,2), rn INT);
    INSERT INTO @DenHan (MaLichThu, MaHD, NgayDenHan, SoTien, rn)
    SELECT MaLichThu, MaHD, NgayDenHan, SoTien,
           ROW_NUMBER() OVER (PARTITION BY MaHD, NgayDenHan, SoTien ORDER BY MaLichThu)
    FROM LichThuPhi
    WHERE NgayDenHan <= CAST(GETDATE() AS DATE) AND TrangThai = N'Chưa đến hạn' AND MaHDN IS NULL;

    DECLARE @HoaDonMoi TABLE (MaHDN INT, MaHD INT, NgayLap DATE, SoTien DECIMAL(18,2));
    INSERT INTO HoaDon (MaHD, NgayLap, SoTien, NguoiTao)
    OUTPUT inserted.MaHDN, inserted.MaHD, inserted.NgayLap, inserted.SoTien INTO @HoaDonMoi
    SELECT MaHD, NgayDenHan, SoTien, 1 FROM @DenHan;

    -- gán số thứ tự cho hóa đơn vừa sinh để khớp 1-1 với @DenHan (cùng MaHD/NgayLap/SoTien)
    ;WITH RankedHD AS (
        SELECT MaHDN, MaHD, NgayLap, SoTien,
               ROW_NUMBER() OVER (PARTITION BY MaHD, NgayLap, SoTien ORDER BY MaHDN) AS rn2
        FROM @HoaDonMoi
    )
    UPDATE ltp
    SET ltp.MaHDN = r.MaHDN
    FROM LichThuPhi ltp
    JOIN @DenHan dh ON dh.MaLichThu = ltp.MaLichThu
    JOIN RankedHD r ON r.MaHD = dh.MaHD AND r.NgayLap = dh.NgayDenHan AND r.SoTien = dh.SoTien AND r.rn2 = dh.rn;

    -- 2) Đánh dấu các kỳ quá hạn thanh toán (đã sinh hóa đơn nhưng hóa đơn vẫn Chưa thanh toán quá 15 ngày)
    UPDATE ltp
    SET ltp.TrangThai = N'Quá hạn'
    FROM LichThuPhi ltp
    JOIN HoaDon hdn ON ltp.MaHDN = hdn.MaHDN
    WHERE hdn.TrangThai = N'Chưa thanh toán' AND ltp.NgayDenHan < DATEADD(DAY, -15, CAST(GETDATE() AS DATE));

    UPDATE ltp SET ltp.TrangThai = N'Đã thanh toán'
    FROM LichThuPhi ltp JOIN HoaDon hdn ON ltp.MaHDN = hdn.MaHDN
    WHERE hdn.TrangThai = N'Đã thanh toán' AND ltp.TrangThai <> N'Đã thanh toán';

    -- 3) Cảnh báo sắp đến hạn: tạo thông báo cho khách hàng + nhân viên phụ trách
    INSERT INTO ThongBao (MaTK, TieuDe, NoiDung, Loai)
    SELECT kh.MaTK, N'Sắp đến hạn đóng phí bảo hiểm',
           N'Hợp đồng #' + CAST(hd.MaHD AS NVARCHAR(10)) + N' sẽ đến hạn đóng phí ngày ' + CONVERT(NVARCHAR(10), ltp.NgayDenHan, 103)
           + N', số tiền ' + CAST(ltp.SoTien AS NVARCHAR(20)),
           'CA_HAI'
    FROM LichThuPhi ltp
    JOIN HopDong hd ON ltp.MaHD = hd.MaHD
    JOIN KhachHang kh ON hd.MaKH = kh.MaKH
    WHERE ltp.DaCanhBao = 0
      AND ltp.NgayDenHan BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, @SoNgayCanhBaoTruoc, CAST(GETDATE() AS DATE))
      AND kh.MaTK IS NOT NULL;

    INSERT INTO ThongBao (MaTK, TieuDe, NoiDung, Loai)
    SELECT nvtk.MaTK,
           N'Cảnh báo kỳ đóng phí sắp đến hạn',
           N'Hợp đồng #' + CAST(hd.MaHD AS NVARCHAR(10)) +
           N' của khách hàng ' + kh.HoTen +
           N' đến hạn ngày ' + CONVERT(NVARCHAR(10), ltp.NgayDenHan, 103),
           'INAPP'
    FROM LichThuPhi ltp
    JOIN HopDong hd ON ltp.MaHD = hd.MaHD
    JOIN KhachHang kh ON hd.MaKH = kh.MaKH
    JOIN NhanVien nv ON hd.MaNV = nv.MaNV
    JOIN TaiKhoan nvtk ON nv.MaTK = nvtk.MaTK
    WHERE ltp.DaCanhBao = 0
      AND ltp.NgayDenHan BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, @SoNgayCanhBaoTruoc, CAST(GETDATE() AS DATE));

    UPDATE ltp SET ltp.DaCanhBao = 1
    FROM LichThuPhi ltp
    WHERE ltp.DaCanhBao = 0
      AND ltp.NgayDenHan BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, @SoNgayCanhBaoTruoc, CAST(GETDATE() AS DATE));

    -- 4) Cảnh báo hợp đồng sắp hết hạn (không phải phí định kỳ mà là hết hạn hợp đồng)
    INSERT INTO ThongBao (MaTK, TieuDe, NoiDung, Loai)
    SELECT kh.MaTK, N'Hợp đồng sắp hết hạn',
           N'Hợp đồng #' + CAST(hd.MaHD AS NVARCHAR(10)) + N' hết hạn ngày ' + CONVERT(NVARCHAR(10), hd.NgayKetThuc, 103)
           + N'. Vui lòng gia hạn nếu có nhu cầu tiếp tục.',
           'CA_HAI'
    FROM HopDong hd
    JOIN KhachHang kh ON hd.MaKH = kh.MaKH
    WHERE hd.TrangThai = N'Đang hoạt động' AND hd.IsDeleted = 0
      AND hd.NgayKetThuc BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, @SoNgayCanhBaoTruoc, CAST(GETDATE() AS DATE))
      AND kh.MaTK IS NOT NULL
      AND NOT EXISTS (
          SELECT 1 FROM ThongBao tb
          WHERE tb.MaTK = kh.MaTK AND tb.TieuDe = N'Hợp đồng sắp hết hạn'
            AND tb.NoiDung LIKE '%#' + CAST(hd.MaHD AS NVARCHAR(10)) + '%'
            AND tb.NgayTao > DATEADD(DAY, -@SoNgayCanhBaoTruoc, GETDATE())
      );
END
GO

CREATE OR ALTER PROCEDURE sp_LichThuPhi_GetPaged
    @MaHD INT = NULL, @TrangThai NVARCHAR(50) = NULL,
    @PageIndex INT = 1, @PageSize INT = 20, @TotalCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT @TotalCount = COUNT(*) FROM LichThuPhi
    WHERE (@MaHD IS NULL OR MaHD = @MaHD) AND (@TrangThai IS NULL OR TrangThai = @TrangThai);

    SELECT ltp.MaLichThu, ltp.MaHD, ltp.KyThu, ltp.NgayDenHan, ltp.SoTien, ltp.TrangThai, ltp.MaHDN
    FROM LichThuPhi ltp
    WHERE (@MaHD IS NULL OR MaHD = @MaHD) AND (@TrangThai IS NULL OR TrangThai = @TrangThai)
    ORDER BY ltp.NgayDenHan ASC
    OFFSET (@PageIndex - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO
USE QuanLyBaoHiem
GO
/* ============================================================
   File 09: Seed dữ liệu số lượng lớn
   - 2.000 khách hàng (+ tài khoản tương ứng)
   - 30 nhân viên
   - 3.000 hợp đồng, hóa đơn, một phần bồi thường & thanh toán
   Tổng số bản ghi sinh ra > 2.000 theo yêu cầu đề bài.
   ============================================================ */
SET NOCOUNT ON;

DECLARE @Ho TABLE (Id INT IDENTITY(1,1), Ten NVARCHAR(20));
INSERT INTO @Ho (Ten) VALUES
(N'Nguyễn'),(N'Trần'),(N'Lê'),(N'Phạm'),(N'Hoàng'),(N'Huỳnh'),(N'Phan'),
(N'Vũ'),(N'Võ'),(N'Đặng'),(N'Bùi'),(N'Đỗ'),(N'Hồ'),(N'Ngô'),(N'Dương');

DECLARE @Ten TABLE (Id INT IDENTITY(1,1), Ten NVARCHAR(20));
INSERT INTO @Ten (Ten) VALUES
(N'Văn An'),(N'Thị Bình'),(N'Văn Cường'),(N'Thị Dung'),(N'Văn Em'),
(N'Thị Hoa'),(N'Văn Hùng'),(N'Thị Lan'),(N'Văn Minh'),(N'Thị Nga'),
(N'Văn Phong'),(N'Thị Quyên'),(N'Văn Sơn'),(N'Thị Thảo'),(N'Văn Tuấn'),
(N'Thị Uyên'),(N'Văn Việt'),(N'Thị Xuân'),(N'Văn Yên'),(N'Thị Hằng');

DECLARE @DiaChi TABLE (Id INT IDENTITY(1,1), Ten NVARCHAR(50));
INSERT INTO @DiaChi (Ten) VALUES
(N'Hà Nội'),(N'Hưng Yên'),(N'Hải Phòng'),(N'Đà Nẵng'),(N'TP.HCM'),
(N'Bắc Ninh'),(N'Nam Định'),(N'Thái Bình'),(N'Quảng Ninh'),(N'Nghệ An');

DECLARE @HashDemo VARCHAR(255) = '$2y$10$1qNUL335jEKoJIUnEYoYaerHGA.xc23he3htM4zR2oTExEwymzXoO';

------------------------------------------------------------
-- 9.1 Sinh 2.000 tài khoản + khách hàng
------------------------------------------------------------
DECLARE @NewKhTK TABLE (MaTK INT);

;WITH Tally2000 AS (
    SELECT TOP (2000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.all_objects a CROSS JOIN sys.all_objects b
)
INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Email, MaQuyen, KichHoat, DaXacMinhEmail)
OUTPUT inserted.MaTK INTO @NewKhTK(MaTK)
SELECT
    'kh' + RIGHT('00000' + CAST(n + 100 AS VARCHAR(10)), 5),
    @HashDemo,
    'kh' + RIGHT('00000' + CAST(n + 100 AS VARCHAR(10)), 5) + '@example.com',
    3, 1, 1
FROM Tally2000;

;WITH Numbered AS (
    SELECT MaTK, ROW_NUMBER() OVER (ORDER BY MaTK) AS rn FROM @NewKhTK
)
INSERT INTO KhachHang (MaTK, HoTen, SoDienThoai, DiaChi, CCCD, NgaySinh, NguoiTao)
SELECT
    nu.MaTK,
    h.Ten + N' ' + t.Ten,
    '09' + RIGHT('00000000' + CAST(10000000 + nu.rn AS VARCHAR(10)), 8),
    d.Ten,
    RIGHT('000000000000' + CAST(100000000000 + nu.rn AS VARCHAR(20)), 12),
    DATEADD(YEAR, -(20 + nu.rn % 40), CAST(GETDATE() AS DATE)),
    1
FROM Numbered nu
JOIN @Ho h ON h.Id = (nu.rn % 15) + 1
JOIN @Ten t ON t.Id = (nu.rn % 20) + 1
JOIN @DiaChi d ON d.Id = (nu.rn % 10) + 1;
GO

------------------------------------------------------------
-- 9.2 Sinh thêm 30 nhân viên
------------------------------------------------------------
DECLARE @Ho TABLE (Id INT IDENTITY(1,1), Ten NVARCHAR(20));
INSERT INTO @Ho (Ten) VALUES (N'Nguyễn'),(N'Trần'),(N'Lê'),(N'Phạm'),(N'Hoàng'),(N'Vũ'),(N'Đặng'),(N'Bùi'),(N'Đỗ'),(N'Ngô');
DECLARE @Ten TABLE (Id INT IDENTITY(1,1), Ten NVARCHAR(20));
INSERT INTO @Ten (Ten) VALUES (N'Văn Đạt'),(N'Thị Huệ'),(N'Văn Khoa'),(N'Thị Linh'),(N'Văn Mạnh'),(N'Thị Nhung'),(N'Văn Phú'),(N'Thị Quỳnh'),(N'Văn Sang'),(N'Thị Trang');
DECLARE @DiaChi TABLE (Id INT IDENTITY(1,1), Ten NVARCHAR(50));
INSERT INTO @DiaChi (Ten) VALUES (N'Hà Nội'),(N'Hưng Yên'),(N'Hải Phòng'),(N'TP.HCM'),(N'Bắc Ninh');
DECLARE @HashDemo VARCHAR(255) = '$2y$10$1qNUL335jEKoJIUnEYoYaerHGA.xc23he3htM4zR2oTExEwymzXoO';
DECLARE @NewNvTK TABLE (MaTK INT);

;WITH Tally30 AS (
    SELECT TOP (30) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.all_objects
)
INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, Email, MaQuyen, KichHoat, DaXacMinhEmail)
OUTPUT inserted.MaTK INTO @NewNvTK(MaTK)
SELECT
    'nv' + RIGHT('000' + CAST(n + 10 AS VARCHAR(10)), 3),
    @HashDemo,
    'nv' + RIGHT('000' + CAST(n + 10 AS VARCHAR(10)), 3) + '@example.com',
    2, 1, 1
FROM Tally30;

;WITH Numbered AS (
    SELECT MaTK, ROW_NUMBER() OVER (ORDER BY MaTK) AS rn FROM @NewNvTK
)
INSERT INTO NhanVien (MaTK, HoTen, SoDienThoai, DiaChi, ChucVu, NguoiTao)
SELECT
    nu.MaTK, h.Ten + N' ' + t.Ten,
    '08' + RIGHT('00000000' + CAST(20000000 + nu.rn AS VARCHAR(10)), 8),
    d.Ten,
    CASE WHEN nu.rn % 10 = 0 THEN N'Trưởng nhóm' ELSE N'Nhân viên' END,
    1
FROM Numbered nu
JOIN @Ho h ON h.Id = (nu.rn % 10) + 1
JOIN @Ten t ON t.Id = (nu.rn % 10) + 1
JOIN @DiaChi d ON d.Id = (nu.rn % 5) + 1;
GO

------------------------------------------------------------
-- 9.3 Sinh 3.000 hợp đồng (kèm hóa đơn) cho khách hàng/nhân viên ngẫu nhiên
------------------------------------------------------------
DECLARE @MinKH INT = (SELECT MIN(MaKH) FROM KhachHang);
DECLARE @CountKH INT = (SELECT COUNT(*) FROM KhachHang);
DECLARE @MinNV INT = (SELECT MIN(MaNV) FROM NhanVien);
DECLARE @CountNV INT = (SELECT COUNT(*) FROM NhanVien);

;WITH Tally3000 AS (
    SELECT TOP (3000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n
    FROM sys.all_objects a CROSS JOIN sys.all_objects b
)
INSERT INTO HopDong (MaKH, MaLoai, MaNV, NgayBatDau, NgayKetThuc, SoTien, TrangThai, NguoiTao)
SELECT
    @MinKH + (t.n % @CountKH),
    lb.MaLoai,
    @MinNV + (t.n % @CountNV),
    DATEADD(DAY, -(t.n % 700), CAST(GETDATE() AS DATE)),
    DATEADD(DAY, 365 - (t.n % 700), CAST(GETDATE() AS DATE)),
    lb.MucPhi,
    CASE WHEN DATEADD(DAY, 365 - (t.n % 700), CAST(GETDATE() AS DATE)) < CAST(GETDATE() AS DATE)
         THEN N'Hết hạn' ELSE N'Đang hoạt động' END,
    1
FROM Tally3000 t
JOIN LoaiBaoHiem lb ON lb.MaLoai = (t.n % 3) + 1;
GO

------------------------------------------------------------
-- 9.4 Sinh hóa đơn cho các hợp đồng vừa tạo (bỏ qua 3 hợp đồng seed ban đầu)
------------------------------------------------------------
INSERT INTO HoaDon (MaHD, NgayLap, SoTien, TrangThai, NguoiTao)
SELECT MaHD, NgayBatDau, SoTien,
       CASE WHEN MaHD % 4 = 0 THEN N'Chưa thanh toán' ELSE N'Đã thanh toán' END,
       1
FROM HopDong
WHERE MaHD > 3;
GO

------------------------------------------------------------
-- 9.5 Sinh thanh toán cho các hóa đơn đã thanh toán
------------------------------------------------------------
INSERT INTO ThanhToan (MaHDN, NgayThanhToan, SoTien, PhuongThuc, NguoiTao)
SELECT MaHDN, NgayLap, SoTien,
       CASE WHEN MaHDN % 2 = 0 THEN N'Chuyển khoản' ELSE N'Tiền mặt' END,
       1
FROM HoaDon
WHERE TrangThai = N'Đã thanh toán' AND MaHDN > 3;
GO

------------------------------------------------------------
-- 9.6 Sinh yêu cầu bồi thường cho khoảng 1/5 số hợp đồng
------------------------------------------------------------
INSERT INTO BoiThuong (MaHD, MaNV, NgayYeuCau, LyDo, SoTien, TrangThai, NguoiTao)
SELECT hd.MaHD, hd.MaNV, DATEADD(DAY, 30, hd.NgayBatDau), N'Yêu cầu bồi thường (dữ liệu mẫu)',
       hd.SoTien * 0.2,
       CASE WHEN hd.MaHD % 3 = 0 THEN N'Đã duyệt'
            WHEN hd.MaHD % 3 = 1 THEN N'Từ chối'
            ELSE N'Đang xử lý' END,
       1
FROM HopDong hd
WHERE hd.MaHD % 5 = 0 AND hd.MaHD > 3;
GO

------------------------------------------------------------
-- 9.7 Kiểm tra tổng số bản ghi đã sinh
------------------------------------------------------------
SELECT 'KhachHang' AS Bang, COUNT(*) AS SoLuong FROM KhachHang
UNION ALL SELECT 'NhanVien', COUNT(*) FROM NhanVien
UNION ALL SELECT 'HopDong', COUNT(*) FROM HopDong
UNION ALL SELECT 'HoaDon', COUNT(*) FROM HoaDon
UNION ALL SELECT 'ThanhToan', COUNT(*) FROM ThanhToan
UNION ALL SELECT 'BoiThuong', COUNT(*) FROM BoiThuong
UNION ALL SELECT 'TaiKhoan', COUNT(*) FROM TaiKhoan;
GO