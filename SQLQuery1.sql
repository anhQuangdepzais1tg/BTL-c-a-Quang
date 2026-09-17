CREATE DATABASE QuanLyBaoHiem
GO

USE QuanLyBaoHiem
GO

--tạo bảng 
CREATE TABLE Quyen
(
    MaQuyen INT IDENTITY PRIMARY KEY,
    TenQuyen NVARCHAR(50)
)
GO

CREATE TABLE ChucNang
(
    MaChucNang INT IDENTITY PRIMARY KEY,
    TenChucNang NVARCHAR(100)
)
GO

CREATE TABLE TaiKhoan
(
    MaTK INT IDENTITY PRIMARY KEY,
    TenDangNhap VARCHAR(50),
    MatKhau VARCHAR(100),
    Email VARCHAR(100),
    MaQuyen INT
)
GO

CREATE TABLE PhanQuyen
(
    MaPhanQuyen INT IDENTITY PRIMARY KEY,
    MaQuyen INT,
    MaChucNang INT,
    DuocXem BIT,
    DuocThem BIT,
    DuocSua BIT,
    DuocXoa BIT
)
GO

CREATE TABLE KhachHang
(
    MaKH INT IDENTITY PRIMARY KEY,
    MaTK INT,
    HoTen NVARCHAR(100),
    SoDienThoai VARCHAR(20),
    DiaChi NVARCHAR(200),
    CCCD VARCHAR(20)
)
GO

CREATE TABLE NhanVien
(
    MaNV INT IDENTITY PRIMARY KEY,
    MaTK INT,
    HoTen NVARCHAR(100),
    SoDienThoai VARCHAR(20),
    DiaChi NVARCHAR(200),
    ChucVu NVARCHAR(50)
)
GO

CREATE TABLE LoaiBaoHiem
(
    MaLoai INT IDENTITY PRIMARY KEY,
    TenLoai NVARCHAR(100),
    MoTa NVARCHAR(200),
    MucPhi DECIMAL(18,2)
)
GO

CREATE TABLE HopDong
(
    MaHD INT IDENTITY PRIMARY KEY,
    MaKH INT,
    MaLoai INT,
    MaNV INT,
    NgayBatDau DATE,
    NgayKetThuc DATE,
    SoTien DECIMAL(18,2),
    TrangThai NVARCHAR(50)
)
GO

CREATE TABLE BoiThuong
(
    MaBT INT IDENTITY PRIMARY KEY,
    MaHD INT,
    MaNV INT,
    NgayYeuCau DATE,
    LyDo NVARCHAR(200),
    SoTien DECIMAL(18,2),
    TrangThai NVARCHAR(50)
)
GO

CREATE TABLE HoaDon
(
    MaHDN INT IDENTITY PRIMARY KEY,
    MaHD INT,
    NgayLap DATE,
    SoTien DECIMAL(18,2),
    TrangThai NVARCHAR(50)
)
GO

CREATE TABLE ThanhToan
(
    MaTT INT IDENTITY PRIMARY KEY,
    MaHDN INT,
    NgayThanhToan DATE,
    SoTien DECIMAL(18,2),
    PhuongThuc NVARCHAR(50)
)
GO

--khóa ngoại 
ALTER TABLE TaiKhoan
ADD FOREIGN KEY (MaQuyen)
REFERENCES Quyen(MaQuyen)
GO

ALTER TABLE PhanQuyen
ADD FOREIGN KEY (MaQuyen)
REFERENCES Quyen(MaQuyen)
GO

ALTER TABLE PhanQuyen
ADD FOREIGN KEY (MaChucNang)
REFERENCES ChucNang(MaChucNang)
GO

ALTER TABLE KhachHang
ADD FOREIGN KEY (MaTK)
REFERENCES TaiKhoan(MaTK)
GO

ALTER TABLE NhanVien
ADD FOREIGN KEY (MaTK)
REFERENCES TaiKhoan(MaTK)
GO

ALTER TABLE HopDong
ADD FOREIGN KEY (MaKH)
REFERENCES KhachHang(MaKH)
GO

ALTER TABLE HopDong
ADD FOREIGN KEY (MaLoai)
REFERENCES LoaiBaoHiem(MaLoai)
GO

ALTER TABLE HopDong
ADD FOREIGN KEY (MaNV)
REFERENCES NhanVien(MaNV)
GO

ALTER TABLE BoiThuong
ADD FOREIGN KEY (MaHD)
REFERENCES HopDong(MaHD)
GO

ALTER TABLE BoiThuong
ADD FOREIGN KEY (MaNV)
REFERENCES NhanVien(MaNV)
GO

ALTER TABLE HoaDon
ADD FOREIGN KEY (MaHD)
REFERENCES HopDong(MaHD)
GO

ALTER TABLE ThanhToan
ADD FOREIGN KEY (MaHDN)
REFERENCES HoaDon(MaHDN)
GO

INSERT INTO Quyen
VALUES
(N'Admin'),
(N'Nhân viên'),
(N'Khách hàng')
GO

INSERT INTO ChucNang
VALUES
(N'Quản lý khách hàng'),
(N'Quản lý nhân viên'),
(N'Quản lý loại bảo hiểm'),
(N'Quản lý hợp đồng'),
(N'Quản lý bồi thường'),
(N'Quản lý hóa đơn'),
(N'Quản lý thanh toán'),
(N'Xem báo cáo')
GO

INSERT INTO TaiKhoan
VALUES
('admin','123456','admin@gmail.com',1),
('nhanvien01','123456','nv01@gmail.com',2),
('nhanvien02','123456','nv02@gmail.com',2),
('khachhang01','123456','kh01@gmail.com',3),
('khachhang02','123456','kh02@gmail.com',3)
GO

INSERT INTO KhachHang
VALUES
(4,N'Nguyễn Văn An','0912345678',N'Hưng Yên','001234567890'),
(5,N'Trần Văn Bình','0987654321',N'Hà Nội','001234567891')
GO

INSERT INTO NhanVien
VALUES
(2,N'Lê Văn Nam','0901234567',N'Hưng Yên',N'Nhân viên'),
(3,N'Phạm Văn Minh','0909876543',N'Hà Nội',N'Quản lý')
GO

INSERT INTO LoaiBaoHiem
VALUES
(N'Bảo hiểm xe',N'Bảo hiểm xe máy và ô tô',500000),
(N'Bảo hiểm nhà',N'Bảo hiểm nhà ở',3000000),
(N'Bảo hiểm sức khỏe',N'Bảo hiểm khám chữa bệnh',2000000)
GO

INSERT INTO HopDong
VALUES
(1,1,1,'2026-01-01','2026-12-31',500000,N'Đang hoạt động'),

(1,2,2,'2026-02-01','2027-01-31',3000000,N'Đang hoạt động'),

(2,3,1,'2026-03-01','2027-02-28',2000000,N'Đang hoạt động')
GO

INSERT INTO BoiThuong
VALUES
(1,1,'2026-05-10',N'Tai nạn xe',200000,N'Đang xử lý'),

(2,2,'2026-06-15',N'Cháy nhà',1000000,N'Đã duyệt')
GO

INSERT INTO HoaDon
VALUES
(1,'2026-01-01',500000,N'Đã thanh toán'),

(2,'2026-02-01',3000000,N'Đã thanh toán'),

(3,'2026-03-01',2000000,N'Chưa thanh toán')
GO

INSERT INTO ThanhToan
VALUES
(1,'2026-01-01',500000,N'Chuyển khoản'),

(2,'2026-02-01',3000000,N'Tiền mặt')
GO

SELECT * FROM Quyen
SELECT * FROM ChucNang
SELECT * FROM PhanQuyen
SELECT * FROM TaiKhoan
SELECT * FROM KhachHang
SELECT * FROM NhanVien
SELECT * FROM LoaiBaoHiem
SELECT * FROM HopDong
SELECT * FROM BoiThuong
SELECT * FROM HoaDon
SELECT * FROM ThanhToan
GO