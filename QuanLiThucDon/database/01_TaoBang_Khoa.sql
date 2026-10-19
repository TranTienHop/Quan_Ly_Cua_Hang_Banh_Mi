/*
================================================================================
  Hệ thống: Quản lý bán hàng & tồn nguyên liệu cửa hàng bánh mì (đa chi nhánh)
  DBMS   : Microsoft SQL Server (LocalDB / Express / đầy đủ)
  Nội dung: Tạo CSDL + bảng + khóa chính (PK) + khóa ngoại (FK) + UNIQUE
  Ghi chú : RBAC0 | PhiShip chỉ trên HoaDon | NhatKy_Kho dùng LoaiThamChieu+MaThamChieu
            35 bảng (gồm ThietBi, Combo_ChiTiet, KhuyenMai)
            DiaChi chỉ ghi cấp Quận/Huyện, Tỉnh/Thành phố (vd: Quận 1, TP.HCM)
================================================================================
*/

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;   -- bắt buộc cho cột tính toán PERSISTED (sqlcmd mặc định OFF)
GO

USE master;
GO

IF DB_ID(N'QuanLyBanhMi') IS NOT NULL
BEGIN
    ALTER DATABASE QuanLyBanhMi SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QuanLyBanhMi;
END
GO

CREATE DATABASE QuanLyBanhMi
    COLLATE SQL_Latin1_General_CP1_CI_AS;
GO

USE QuanLyBanhMi;
GO

/*------------------------------------------------------------------------------
  1. PHÂN QUYỀN (RBAC0)
------------------------------------------------------------------------------*/

CREATE TABLE dbo.VaiTro
(
    MaVaiTro   INT            NOT NULL IDENTITY(1,1),
    MaCode     NVARCHAR(30)   NOT NULL,          -- ADMIN / MANAGER / STAFF
    TenVaiTro  NVARCHAR(100)  NOT NULL,
    MoTa       NVARCHAR(255)  NULL,
    CONSTRAINT PK_VaiTro PRIMARY KEY (MaVaiTro),
    CONSTRAINT UQ_VaiTro_MaCode UNIQUE (MaCode)
);
GO

CREATE TABLE dbo.MenuChucNang
(
    MaMenu      INT            NOT NULL IDENTITY(1,1),
    MaCode      NVARCHAR(30)   NOT NULL,        -- vd: SYS_AUTH, POS_ORDER
    MaMenuCha   INT            NULL,
    TenHienThi  NVARCHAR(150)  NOT NULL,
    MaForm      NVARCHAR(100)  NULL,            -- vd: FrmBanHang, FrmKho
    Loai        NVARCHAR(20)   NOT NULL         -- NhomMenu / Trang
                    CONSTRAINT DF_Menu_Loai DEFAULT (N'Trang'),
    ThuTu       INT            NOT NULL
                    CONSTRAINT DF_Menu_ThuTu DEFAULT (0),
    CONSTRAINT PK_MenuChucNang PRIMARY KEY (MaMenu),
    CONSTRAINT UQ_Menu_MaCode UNIQUE (MaCode),
    CONSTRAINT FK_Menu_Cha
        FOREIGN KEY (MaMenuCha) REFERENCES dbo.MenuChucNang (MaMenu)
);
GO

CREATE TABLE dbo.VaiTro_Menu
(
    MaVaiTro INT NOT NULL,
    MaMenu   INT NOT NULL,
    CONSTRAINT PK_VaiTro_Menu PRIMARY KEY (MaVaiTro, MaMenu),
    CONSTRAINT FK_VaiTroMenu_VaiTro
        FOREIGN KEY (MaVaiTro) REFERENCES dbo.VaiTro (MaVaiTro),
    CONSTRAINT FK_VaiTroMenu_Menu
        FOREIGN KEY (MaMenu) REFERENCES dbo.MenuChucNang (MaMenu)
);
GO

/*------------------------------------------------------------------------------
  2. TỔ CHỨC – NHÂN SỰ – ĐĂNG NHẬP – CA
------------------------------------------------------------------------------*/

CREATE TABLE dbo.ChiNhanh
(
    MaChiNhanh    INT            NOT NULL IDENTITY(1,1),
    MaCode        NVARCHAR(20)   NOT NULL,       -- HN01, HN02
    TenChiNhanh   NVARCHAR(200)  NOT NULL,
    LoaiChiNhanh  TINYINT        NOT NULL,       -- 1=CuaHang, 2=LoKhoTong
    DiaChi        NVARCHAR(255)  NULL,           -- Quận/Huyện, Tỉnh/TP
    SoDienThoai   NVARCHAR(20)   NULL,
    DangHoatDong  BIT            NOT NULL
                    CONSTRAINT DF_ChiNhanh_Active DEFAULT (1),
    CONSTRAINT PK_ChiNhanh PRIMARY KEY (MaChiNhanh),
    CONSTRAINT UQ_ChiNhanh_MaCode UNIQUE (MaCode),
    CONSTRAINT CK_ChiNhanh_Loai CHECK (LoaiChiNhanh IN (1, 2))
);
GO

CREATE TABLE dbo.NhanVien
(
    MaNhanVien   INT            NOT NULL IDENTITY(1,1),
    MaCode       NVARCHAR(20)   NOT NULL,        -- vd: NV001
    MaChiNhanh   INT            NOT NULL,
    HoTen        NVARCHAR(150)  NOT NULL,
    ChucVu       NVARCHAR(100)  NULL,
    SoDienThoai  NVARCHAR(20)   NULL,
    Email        NVARCHAR(150)  NULL,
    DangHoatDong BIT            NOT NULL
                    CONSTRAINT DF_NhanVien_Active DEFAULT (1),
    CONSTRAINT PK_NhanVien PRIMARY KEY (MaNhanVien),
    CONSTRAINT UQ_NhanVien_MaCode UNIQUE (MaCode),
    CONSTRAINT FK_NhanVien_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh)
);
GO

CREATE TABLE dbo.TaiKhoan
(
    MaTaiKhoan       INT            NOT NULL IDENTITY(1,1),
    MaNhanVien       INT            NOT NULL,
    MaVaiTro         INT            NOT NULL,
    TenDangNhap      NVARCHAR(50)   NOT NULL,
    MatKhauBam       NVARCHAR(255)  NOT NULL,
    DangHoatDong     BIT            NOT NULL
                        CONSTRAINT DF_TaiKhoan_Active DEFAULT (1),
    LanDangNhapCuoi  DATETIME2(0)   NULL,
    CONSTRAINT PK_TaiKhoan PRIMARY KEY (MaTaiKhoan),
    CONSTRAINT UQ_TaiKhoan_NhanVien UNIQUE (MaNhanVien),
    CONSTRAINT UQ_TaiKhoan_TenDangNhap UNIQUE (TenDangNhap),
    CONSTRAINT FK_TaiKhoan_NhanVien
        FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien (MaNhanVien),
    CONSTRAINT FK_TaiKhoan_VaiTro
        FOREIGN KEY (MaVaiTro) REFERENCES dbo.VaiTro (MaVaiTro)
);
GO

CREATE TABLE dbo.NhatKy_CaLam
(
    MaCa          INT             NOT NULL IDENTITY(1,1),
    MaChiNhanh    INT             NOT NULL,
    MaNhanVien    INT             NOT NULL,
    GioMo         DATETIME2(0)    NOT NULL,
    GioDong       DATETIME2(0)    NULL,
    TienDauCa     DECIMAL(18,2)   NOT NULL CONSTRAINT DF_Ca_TienDau DEFAULT (0),
    TienCuoiCa    DECIMAL(18,2)   NULL,
    TienHeThong   DECIMAL(18,2)   NULL,
    TienThucDem   DECIMAL(18,2)   NULL,
    TrangThai     NVARCHAR(20)    NOT NULL       -- DangMo / DaDong / DaDuyet
                    CONSTRAINT DF_Ca_TrangThai DEFAULT (N'DangMo'),
    GhiChu        NVARCHAR(255)   NULL,
    CONSTRAINT PK_NhatKy_CaLam PRIMARY KEY (MaCa),
    CONSTRAINT FK_Ca_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_Ca_NhanVien
        FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien (MaNhanVien)
);
GO

/*------------------------------------------------------------------------------
  3. ĐƠN VỊ – THỰC ĐƠN – GIÁ THEO CHI NHÁNH
------------------------------------------------------------------------------*/

CREATE TABLE dbo.DonVi
(
    MaDonVi    INT           NOT NULL IDENTITY(1,1),
    TenDonVi   NVARCHAR(50)  NOT NULL,            -- g, kg, cái, ly...
    LoaiDonVi  NVARCHAR(20)  NOT NULL,            -- KhoiLuong / TheTich / Dem
    CONSTRAINT PK_DonVi PRIMARY KEY (MaDonVi),
    CONSTRAINT UQ_DonVi_Ten UNIQUE (TenDonVi)
);
GO

CREATE TABLE dbo.QuyDoi_DonVi
(
    MaQuyDoi       INT            NOT NULL IDENTITY(1,1),
    MaDonViNguon   INT            NOT NULL,
    MaDonViDich    INT            NOT NULL,
    HeSo           DECIMAL(18,6)  NOT NULL,       -- 1 nguồn = HeSo đích
    CONSTRAINT PK_QuyDoi_DonVi PRIMARY KEY (MaQuyDoi),
    CONSTRAINT UQ_QuyDoi UNIQUE (MaDonViNguon, MaDonViDich),
    CONSTRAINT FK_QuyDoi_Nguon
        FOREIGN KEY (MaDonViNguon) REFERENCES dbo.DonVi (MaDonVi),
    CONSTRAINT FK_QuyDoi_Dich
        FOREIGN KEY (MaDonViDich) REFERENCES dbo.DonVi (MaDonVi),
    CONSTRAINT CK_QuyDoi_HeSo CHECK (HeSo > 0),
    CONSTRAINT CK_QuyDoi_KhacNhau CHECK (MaDonViNguon <> MaDonViDich)
);
GO

CREATE TABLE dbo.DanhMuc
(
    MaDanhMuc    INT            NOT NULL IDENTITY(1,1),
    TenDanhMuc   NVARCHAR(100)  NOT NULL,
    DangHoatDong BIT            NOT NULL
                    CONSTRAINT DF_DanhMuc_Active DEFAULT (1),
    CONSTRAINT PK_DanhMuc PRIMARY KEY (MaDanhMuc)
);
GO

CREATE TABLE dbo.SanPham
(
    MaSanPham         INT            NOT NULL IDENTITY(1,1),
    MaDanhMuc         INT            NOT NULL,
    MaDonViBan        INT            NOT NULL,
    TenSanPham        NVARCHAR(200)  NOT NULL,
    MaSKU             NVARCHAR(50)   NULL,
    LoaiSanPham       NVARCHAR(30)   NOT NULL     -- ThanhPham / KhongTruKho / Combo
                        CONSTRAINT DF_SP_Loai DEFAULT (N'ThanhPham'),
    BatBuocCongThuc   BIT            NOT NULL
                        CONSTRAINT DF_SP_CT DEFAULT (1),
    DangHoatDong      BIT            NOT NULL
                        CONSTRAINT DF_SP_Active DEFAULT (1),
    CONSTRAINT PK_SanPham PRIMARY KEY (MaSanPham),
    CONSTRAINT FK_SanPham_DanhMuc
        FOREIGN KEY (MaDanhMuc) REFERENCES dbo.DanhMuc (MaDanhMuc),
    CONSTRAINT FK_SanPham_DonVi
        FOREIGN KEY (MaDonViBan) REFERENCES dbo.DonVi (MaDonVi),
    CONSTRAINT CK_SanPham_Loai CHECK (LoaiSanPham IN
        (N'ThanhPham', N'KhongTruKho', N'Combo')),
    -- combo trừ kho qua công thức của món thành phần, không có công thức riêng
    CONSTRAINT CK_SanPham_ComboCT CHECK (LoaiSanPham <> N'Combo' OR BatBuocCongThuc = 0)
);
GO

CREATE TABLE dbo.SanPham_ChiNhanh
(
    MaChiNhanh INT            NOT NULL,
    MaSanPham  INT            NOT NULL,
    GiaBan     DECIMAL(18,2)  NOT NULL,
    ConBan     BIT            NOT NULL
                    CONSTRAINT DF_SPCN_ConBan DEFAULT (1),
    CONSTRAINT PK_SanPham_ChiNhanh PRIMARY KEY (MaChiNhanh, MaSanPham),
    CONSTRAINT FK_SPCN_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_SPCN_SanPham
        FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham (MaSanPham),
    CONSTRAINT CK_SPCN_Gia CHECK (GiaBan >= 0)
);
GO

CREATE TABLE dbo.Combo_ChiTiet
(
    MaCombo    INT  NOT NULL,                    -- SanPham có LoaiSanPham = Combo
    MaSanPham  INT  NOT NULL,                    -- món thành phần (bánh mì, nước)
    SoLuong    INT  NOT NULL
                    CONSTRAINT DF_Combo_SoLuong DEFAULT (1),
    CONSTRAINT PK_Combo_ChiTiet PRIMARY KEY (MaCombo, MaSanPham),
    CONSTRAINT FK_Combo_Combo
        FOREIGN KEY (MaCombo) REFERENCES dbo.SanPham (MaSanPham),
    CONSTRAINT FK_Combo_SanPham
        FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham (MaSanPham),
    CONSTRAINT CK_Combo_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT CK_Combo_KhacNhau CHECK (MaCombo <> MaSanPham)
);
GO

/*------------------------------------------------------------------------------
  4. KHÁCH HÀNG – LOYALTY
------------------------------------------------------------------------------*/

CREATE TABLE dbo.CauHinh_HangThanhVien
(
    MaHang         INT            NOT NULL IDENTITY(1,1),
    TenHang        NVARCHAR(50)   NOT NULL,       -- Dong / Bac / Vang
    PhanTramGiam   DECIMAL(5,2)   NOT NULL
                        CONSTRAINT DF_Hang_Giam DEFAULT (0),
    DiemToiThieu   INT            NOT NULL
                        CONSTRAINT DF_Hang_Diem DEFAULT (0),
    CONSTRAINT PK_HangThanhVien PRIMARY KEY (MaHang),
    CONSTRAINT CK_Hang_Giam CHECK (PhanTramGiam >= 0 AND PhanTramGiam <= 100)
);
GO

CREATE TABLE dbo.KhachHang
(
    MaKhachHang   INT            NOT NULL IDENTITY(1,1),
    MaHang        INT            NOT NULL,
    HoTen         NVARCHAR(150)  NOT NULL,
    SoDienThoai   NVARCHAR(20)   NOT NULL,
    DiemHienTai   INT            NOT NULL           -- dẫn xuất có chủ đích
                    CONSTRAINT DF_KH_Diem DEFAULT (0),
    Email         NVARCHAR(150)  NULL,
    TheNhan       NVARCHAR(255)  NULL,
    DangHoatDong  BIT            NOT NULL
                    CONSTRAINT DF_KH_Active DEFAULT (1),
    CONSTRAINT PK_KhachHang PRIMARY KEY (MaKhachHang),
    CONSTRAINT UQ_KhachHang_SDT UNIQUE (SoDienThoai),
    CONSTRAINT FK_KhachHang_Hang
        FOREIGN KEY (MaHang) REFERENCES dbo.CauHinh_HangThanhVien (MaHang)
);
GO

/*------------------------------------------------------------------------------
  5. KHUYẾN MÃI – BÁN HÀNG – GIAO HÀNG – THANH TOÁN
------------------------------------------------------------------------------*/

CREATE TABLE dbo.KhuyenMai
(
    MaKhuyenMai   INT             NOT NULL IDENTITY(1,1),
    MaCode        NVARCHAR(30)    NOT NULL,      -- mã nhập tại quầy: KM-KHAITRUONG
    TenKhuyenMai  NVARCHAR(200)   NOT NULL,
    LoaiGiam      NVARCHAR(20)    NOT NULL,      -- PhanTram / SoTien
    GiaTriGiam    DECIMAL(18,2)   NOT NULL,
    GiamToiDa     DECIMAL(18,2)   NULL,          -- trần tiền giảm cho loại PhanTram
    DonToiThieu   DECIMAL(18,2)   NOT NULL
                    CONSTRAINT DF_KM_DonToiThieu DEFAULT (0),
    MaChiNhanh    INT             NULL,          -- NULL = áp dụng mọi chi nhánh
    NgayBatDau    DATETIME2(0)    NOT NULL,
    NgayKetThuc   DATETIME2(0)    NOT NULL,
    DangHoatDong  BIT             NOT NULL
                    CONSTRAINT DF_KM_Active DEFAULT (1),
    CONSTRAINT PK_KhuyenMai PRIMARY KEY (MaKhuyenMai),
    CONSTRAINT UQ_KhuyenMai_MaCode UNIQUE (MaCode),
    CONSTRAINT FK_KhuyenMai_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT CK_KM_Loai CHECK (LoaiGiam IN (N'PhanTram', N'SoTien')),
    CONSTRAINT CK_KM_GiaTri CHECK (GiaTriGiam > 0
        AND (LoaiGiam = N'SoTien' OR GiaTriGiam <= 100)),
    CONSTRAINT CK_KM_GiamToiDa CHECK (GiamToiDa IS NULL OR GiamToiDa > 0),
    CONSTRAINT CK_KM_DonToiThieu CHECK (DonToiThieu >= 0),
    CONSTRAINT CK_KM_Ngay CHECK (NgayKetThuc > NgayBatDau)
);
GO

CREATE TABLE dbo.HoaDon
(
    MaHoaDon              INT             NOT NULL IDENTITY(1,1),
    MaDonLocal            NVARCHAR(50)    NULL,
    MaChiNhanh            INT             NOT NULL,
    MaThuNgan             INT             NOT NULL,
    MaKhachHang           INT             NULL,     -- NULL = khách vãng lai
    MaCa                  INT             NULL,
    MaKhuyenMai           INT             NULL,     -- NULL = không áp khuyến mãi
    NguonDon              NVARCHAR(30)    NOT NULL   -- POS / Grab / ShopeeFood
                            CONSTRAINT DF_HD_Nguon DEFAULT (N'POS'),
    LoaiDon               NVARCHAR(20)    NOT NULL   -- TaiQuay / MangDi / GiaoHang
                            CONSTRAINT DF_HD_Loai DEFAULT (N'TaiQuay'),
    TongTienHang          DECIMAL(18,2)   NOT NULL   -- dẫn xuất có chủ đích
                            CONSTRAINT DF_HD_TienHang DEFAULT (0),
    TienGiam              DECIMAL(18,2)   NOT NULL   -- tiền giảm từ khuyến mãi
                            CONSTRAINT DF_HD_Giam DEFAULT (0),
    PhiShip               DECIMAL(18,2)   NOT NULL   -- NGUỒN CHÍNH phí ship
                            CONSTRAINT DF_HD_Ship DEFAULT (0),
    TongThanhToan         DECIMAL(18,2)   NOT NULL   -- dẫn xuất có chủ đích
                            CONSTRAINT DF_HD_Tong DEFAULT (0),
    DiemCong              INT             NOT NULL CONSTRAINT DF_HD_DiemCong DEFAULT (0),
    DiemTru               INT             NOT NULL CONSTRAINT DF_HD_DiemTru DEFAULT (0),
    TrangThaiDon          NVARCHAR(20)    NOT NULL
                            CONSTRAINT DF_HD_TTDon DEFAULT (N'XacNhan'),
    TrangThaiThanhToan    NVARCHAR(20)    NOT NULL
                            CONSTRAINT DF_HD_TTTT DEFAULT (N'ChuaTT'),
    NgayTao               DATETIME2(0)    NOT NULL
                            CONSTRAINT DF_HD_NgayTao DEFAULT (SYSUTCDATETIME()),
    NgayThanhToan         DATETIME2(0)    NULL,
    CONSTRAINT PK_HoaDon PRIMARY KEY (MaHoaDon),
    CONSTRAINT FK_HoaDon_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_HoaDon_ThuNgan
        FOREIGN KEY (MaThuNgan) REFERENCES dbo.NhanVien (MaNhanVien),
    CONSTRAINT FK_HoaDon_KhachHang
        FOREIGN KEY (MaKhachHang) REFERENCES dbo.KhachHang (MaKhachHang),
    CONSTRAINT FK_HoaDon_Ca
        FOREIGN KEY (MaCa) REFERENCES dbo.NhatKy_CaLam (MaCa),
    CONSTRAINT FK_HoaDon_KhuyenMai
        FOREIGN KEY (MaKhuyenMai) REFERENCES dbo.KhuyenMai (MaKhuyenMai),
    CONSTRAINT CK_HoaDon_Tien CHECK (
        TongTienHang >= 0 AND TienGiam >= 0 AND PhiShip >= 0 AND TongThanhToan >= 0
    )
);
GO

CREATE TABLE dbo.ChiTiet_HoaDon
(
    MaChiTiet        INT             NOT NULL IDENTITY(1,1),
    MaHoaDon         INT             NOT NULL,
    MaSanPham        INT             NOT NULL,
    TenSanPhamChup   NVARCHAR(200)   NOT NULL,     -- đóng băng tên
    SoLuong          DECIMAL(18,3)   NOT NULL,
    DonGia           DECIMAL(18,2)   NOT NULL,     -- đóng băng giá
    GiamDong         DECIMAL(18,2)   NOT NULL
                        CONSTRAINT DF_CTHD_Giam DEFAULT (0),
    ThanhTien        DECIMAL(18,2)   NOT NULL,
    GhiChu           NVARCHAR(255)   NULL,
    CONSTRAINT PK_ChiTiet_HoaDon PRIMARY KEY (MaChiTiet),
    CONSTRAINT FK_CTHD_HoaDon
        FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon (MaHoaDon),
    CONSTRAINT FK_CTHD_SanPham
        FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham (MaSanPham),
    CONSTRAINT CK_CTHD_SL CHECK (SoLuong > 0),
    CONSTRAINT CK_CTHD_Gia CHECK (DonGia >= 0 AND ThanhTien >= 0)
);
GO

CREATE TABLE dbo.ThanhToan_HoaDon
(
    MaThanhToan   INT             NOT NULL IDENTITY(1,1),
    MaHoaDon      INT             NOT NULL,
    HinhThuc      NVARCHAR(30)    NOT NULL,       -- TienMat / ChuyenKhoan / The / Vi
    SoTien        DECIMAL(18,2)   NOT NULL,
    ThoiGian      DATETIME2(0)    NOT NULL
                    CONSTRAINT DF_TTHD_TG DEFAULT (SYSUTCDATETIME()),
    MaThamChieu   NVARCHAR(100)   NULL,
    NguoiThu      INT             NULL,
    CONSTRAINT PK_ThanhToan_HoaDon PRIMARY KEY (MaThanhToan),
    CONSTRAINT FK_TTHD_HoaDon
        FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon (MaHoaDon),
    CONSTRAINT FK_TTHD_NguoiThu
        FOREIGN KEY (NguoiThu) REFERENCES dbo.NhanVien (MaNhanVien),
    CONSTRAINT CK_TTHD_SoTien CHECK (SoTien > 0)
);
GO

CREATE TABLE dbo.NhatKy_GiaoHang
(
    MaGiaoHang   INT            NOT NULL IDENTITY(1,1),
    MaHoaDon     INT            NOT NULL,          -- 1 HĐ tối đa 1 ship
    LoaiShip     NVARCHAR(30)   NOT NULL,          -- NoiBo / Grab / Shopee...
    TenShipper   NVARCHAR(100)  NULL,
    SoDienThoai  NVARCHAR(20)   NULL,
    DiaChi       NVARCHAR(255)  NOT NULL,          -- Quận/Huyện, Tỉnh/TP
    TrangThai    NVARCHAR(20)   NOT NULL
                    CONSTRAINT DF_GH_TT DEFAULT (N'ChoLay'),
    GioLay       DATETIME2(0)   NULL,
    GioGiao      DATETIME2(0)   NULL,
    -- KHÔNG có PhiShip (đã nằm trên HoaDon)
    CONSTRAINT PK_NhatKy_GiaoHang PRIMARY KEY (MaGiaoHang),
    CONSTRAINT UQ_GiaoHang_HoaDon UNIQUE (MaHoaDon),
    CONSTRAINT FK_GiaoHang_HoaDon
        FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon (MaHoaDon)
);
GO

CREATE TABLE dbo.GiaoDich_TichDiem
(
    MaGD             INT            NOT NULL IDENTITY(1,1),
    MaKhachHang      INT            NOT NULL,
    MaHoaDon         INT            NULL,
    SoDiemThayDoi    INT            NOT NULL,       -- + tích / − tiêu
    DiemSauGD        INT            NOT NULL,
    LyDo             NVARCHAR(30)   NOT NULL,       -- Tich / Tieu / DieuChinh
    NgayTao          DATETIME2(0)   NOT NULL
                        CONSTRAINT DF_GDDiem_Ngay DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_GiaoDich_TichDiem PRIMARY KEY (MaGD),
    CONSTRAINT FK_GDDiem_KH
        FOREIGN KEY (MaKhachHang) REFERENCES dbo.KhachHang (MaKhachHang),
    CONSTRAINT FK_GDDiem_HD
        FOREIGN KEY (MaHoaDon) REFERENCES dbo.HoaDon (MaHoaDon)
);
GO

/*------------------------------------------------------------------------------
  6. KHO – ĐỊNH LƯỢNG – SỔ KHO
------------------------------------------------------------------------------*/

CREATE TABLE dbo.NguyenLieu
(
    MaNguyenLieu        INT             NOT NULL IDENTITY(1,1),
    TenNguyenLieu       NVARCHAR(200)   NOT NULL,
    MaDonViGoc          INT             NOT NULL,
    TonToiThieuCanhBao  DECIMAL(18,3)   NOT NULL
                            CONSTRAINT DF_NL_TonMin DEFAULT (0),
    DangHoatDong        BIT             NOT NULL
                            CONSTRAINT DF_NL_Active DEFAULT (1),
    CONSTRAINT PK_NguyenLieu PRIMARY KEY (MaNguyenLieu),
    CONSTRAINT FK_NguyenLieu_DonVi
        FOREIGN KEY (MaDonViGoc) REFERENCES dbo.DonVi (MaDonVi)
);
GO

CREATE TABLE dbo.CongThuc
(
    MaSanPham           INT             NOT NULL,
    MaNguyenLieu        INT             NOT NULL,
    SoLuongTrenMotPhan  DECIMAL(18,6)   NOT NULL,
    CONSTRAINT PK_CongThuc PRIMARY KEY (MaSanPham, MaNguyenLieu),
    CONSTRAINT FK_CongThuc_SanPham
        FOREIGN KEY (MaSanPham) REFERENCES dbo.SanPham (MaSanPham),
    CONSTRAINT FK_CongThuc_NguyenLieu
        FOREIGN KEY (MaNguyenLieu) REFERENCES dbo.NguyenLieu (MaNguyenLieu),
    CONSTRAINT CK_CongThuc_SL CHECK (SoLuongTrenMotPhan > 0)
);
GO

CREATE TABLE dbo.TonKho_ChiNhanh
(
    MaChiNhanh   INT             NOT NULL,
    MaNguyenLieu INT             NOT NULL,
    SoLuongTon   DECIMAL(18,3)   NOT NULL
                    CONSTRAINT DF_Ton_SL DEFAULT (0),
    NgayCapNhat  DATETIME2(0)    NOT NULL
                    CONSTRAINT DF_Ton_Ngay DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT PK_TonKho_ChiNhanh PRIMARY KEY (MaChiNhanh, MaNguyenLieu),
    CONSTRAINT FK_Ton_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_Ton_NguyenLieu
        FOREIGN KEY (MaNguyenLieu) REFERENCES dbo.NguyenLieu (MaNguyenLieu)
);
GO

CREATE TABLE dbo.NhatKy_Kho
(
    MaNhatKy         INT             NOT NULL IDENTITY(1,1),
    MaChiNhanh       INT             NOT NULL,
    MaNguyenLieu     INT             NOT NULL,
    LoaiGiaoDich     NVARCHAR(30)    NOT NULL,
        -- NHAP | XUAT_BAN | CHUYEN_DI | CHUYEN_DEN | HUY | KIEM_KE
    SoLuongThayDoi   DECIMAL(18,3)   NOT NULL,     -- + nhập / − xuất
    TonSau           DECIMAL(18,3)   NULL,
    MaNhanVien       INT             NULL,
    NgayTao          DATETIME2(0)    NOT NULL
                        CONSTRAINT DF_NK_Ngay DEFAULT (SYSUTCDATETIME()),
    LoaiThamChieu    NVARCHAR(30)    NULL,
        -- HOADON | PHIEU_NHAP | DIEU_CHUYEN | HUY | KIEM_KHO
    MaThamChieu      INT             NULL,         -- mã phiếu (không FK cứng)
    GhiChu           NVARCHAR(255)   NULL,
    CONSTRAINT PK_NhatKy_Kho PRIMARY KEY (MaNhatKy),
    CONSTRAINT FK_NK_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_NK_NguyenLieu
        FOREIGN KEY (MaNguyenLieu) REFERENCES dbo.NguyenLieu (MaNguyenLieu),
    CONSTRAINT FK_NK_NhanVien
        FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien (MaNhanVien)
);
GO

/*------------------------------------------------------------------------------
  7. NHẬP HÀNG – ĐIỀU CHUYỂN – HỦY – KIỂM KHO
------------------------------------------------------------------------------*/

CREATE TABLE dbo.NhaCungCap
(
    MaNCC         INT            NOT NULL IDENTITY(1,1),
    TenNCC        NVARCHAR(200)  NOT NULL,
    SoDienThoai   NVARCHAR(20)   NULL,
    DiaChi        NVARCHAR(255)  NULL,           -- Quận/Huyện, Tỉnh/TP
    DangHoatDong  BIT            NOT NULL
                    CONSTRAINT DF_NCC_Active DEFAULT (1),
    CONSTRAINT PK_NhaCungCap PRIMARY KEY (MaNCC)
);
GO

CREATE TABLE dbo.Phieu_NhapHang
(
    MaPhieuNhap       INT             NOT NULL IDENTITY(1,1),
    MaChiNhanh        INT             NOT NULL,
    MaNCC             INT             NOT NULL,
    NgayDat           DATE            NOT NULL,
    NgayNhan          DATE            NULL,
    TongTien          DECIMAL(18,2)   NOT NULL CONSTRAINT DF_PN_Tong DEFAULT (0),
    DaThanhToan       DECIMAL(18,2)   NOT NULL CONSTRAINT DF_PN_DaTT DEFAULT (0),
    TrangThai         NVARCHAR(20)    NOT NULL CONSTRAINT DF_PN_TT DEFAULT (N'ChoNhap'),
    TrangThaiCongNo   NVARCHAR(20)    NOT NULL CONSTRAINT DF_PN_No DEFAULT (N'ChuaTT'),
    CONSTRAINT PK_Phieu_NhapHang PRIMARY KEY (MaPhieuNhap),
    CONSTRAINT FK_PN_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_PN_NCC
        FOREIGN KEY (MaNCC) REFERENCES dbo.NhaCungCap (MaNCC)
);
GO

CREATE TABLE dbo.ChiTiet_NhapHang
(
    MaChiTiet    INT             NOT NULL IDENTITY(1,1),
    MaPhieuNhap  INT             NOT NULL,
    MaNguyenLieu INT             NOT NULL,
    SoLuong      DECIMAL(18,3)   NOT NULL,
    DonGiaNhap   DECIMAL(18,2)   NOT NULL,
    ThanhTien    DECIMAL(18,2)   NOT NULL,
    CONSTRAINT PK_ChiTiet_NhapHang PRIMARY KEY (MaChiTiet),
    CONSTRAINT FK_CTNH_Phieu
        FOREIGN KEY (MaPhieuNhap) REFERENCES dbo.Phieu_NhapHang (MaPhieuNhap),
    CONSTRAINT FK_CTNH_NL
        FOREIGN KEY (MaNguyenLieu) REFERENCES dbo.NguyenLieu (MaNguyenLieu),
    CONSTRAINT CK_CTNH_SL CHECK (SoLuong > 0 AND DonGiaNhap >= 0)
);
GO

CREATE TABLE dbo.Phieu_DieuChuyen
(
    MaPhieu     INT            NOT NULL IDENTITY(1,1),
    MaKhoGui    INT            NOT NULL,
    MaKhoNhan   INT            NOT NULL,
    NgayGui     DATETIME2(0)   NOT NULL,
    NgayNhan    DATETIME2(0)   NULL,
    TrangThai   NVARCHAR(20)   NOT NULL CONSTRAINT DF_DC_TT DEFAULT (N'ChoGui'),
    NguoiGui    INT            NULL,
    NguoiNhan   INT            NULL,
    CONSTRAINT PK_Phieu_DieuChuyen PRIMARY KEY (MaPhieu),
    CONSTRAINT FK_DC_KhoGui
        FOREIGN KEY (MaKhoGui) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_DC_KhoNhan
        FOREIGN KEY (MaKhoNhan) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_DC_NguoiGui
        FOREIGN KEY (NguoiGui) REFERENCES dbo.NhanVien (MaNhanVien),
    CONSTRAINT FK_DC_NguoiNhan
        FOREIGN KEY (NguoiNhan) REFERENCES dbo.NhanVien (MaNhanVien),
    CONSTRAINT CK_DC_KhacKho CHECK (MaKhoGui <> MaKhoNhan)
);
GO

CREATE TABLE dbo.ChiTiet_DieuChuyen
(
    MaChiTiet            INT             NOT NULL IDENTITY(1,1),
    MaPhieu              INT             NOT NULL,
    MaNguyenLieu         INT             NOT NULL,
    SoLuongGui           DECIMAL(18,3)   NOT NULL,
    SoLuongNhanThucTe    DECIMAL(18,3)   NULL,
    CONSTRAINT PK_ChiTiet_DieuChuyen PRIMARY KEY (MaChiTiet),
    CONSTRAINT FK_CTDC_Phieu
        FOREIGN KEY (MaPhieu) REFERENCES dbo.Phieu_DieuChuyen (MaPhieu),
    CONSTRAINT FK_CTDC_NL
        FOREIGN KEY (MaNguyenLieu) REFERENCES dbo.NguyenLieu (MaNguyenLieu),
    CONSTRAINT CK_CTDC_SL CHECK (SoLuongGui > 0)
);
GO

CREATE TABLE dbo.Phieu_HuyHaoHut
(
    MaPhieu      INT            NOT NULL IDENTITY(1,1),
    MaChiNhanh   INT            NOT NULL,
    NguoiLap     INT            NOT NULL,
    NgayLap      DATETIME2(0)   NOT NULL
                    CONSTRAINT DF_Huy_Ngay DEFAULT (SYSUTCDATETIME()),
    LyDo         NVARCHAR(255)  NULL,
    TrangThai    NVARCHAR(20)   NOT NULL CONSTRAINT DF_Huy_TT DEFAULT (N'Draft'),
    CONSTRAINT PK_Phieu_HuyHaoHut PRIMARY KEY (MaPhieu),
    CONSTRAINT FK_Huy_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_Huy_NguoiLap
        FOREIGN KEY (NguoiLap) REFERENCES dbo.NhanVien (MaNhanVien)
);
GO

CREATE TABLE dbo.ChiTiet_HuyHaoHut
(
    MaChiTiet    INT             NOT NULL IDENTITY(1,1),
    MaPhieu      INT             NOT NULL,
    MaNguyenLieu INT             NOT NULL,
    SoLuong      DECIMAL(18,3)   NOT NULL,
    GhiChu       NVARCHAR(255)   NULL,
    CONSTRAINT PK_ChiTiet_HuyHaoHut PRIMARY KEY (MaChiTiet),
    CONSTRAINT FK_CTHuy_Phieu
        FOREIGN KEY (MaPhieu) REFERENCES dbo.Phieu_HuyHaoHut (MaPhieu),
    CONSTRAINT FK_CTHuy_NL
        FOREIGN KEY (MaNguyenLieu) REFERENCES dbo.NguyenLieu (MaNguyenLieu),
    CONSTRAINT CK_CTHuy_SL CHECK (SoLuong > 0)
);
GO

CREATE TABLE dbo.Phieu_KiemKho
(
    MaPhieu     INT            NOT NULL IDENTITY(1,1),
    MaChiNhanh  INT            NOT NULL,
    NgayKiem    DATETIME2(0)   NOT NULL
                    CONSTRAINT DF_KK_Ngay DEFAULT (SYSUTCDATETIME()),
    NguoiKiem   INT            NOT NULL,
    TrangThai   NVARCHAR(20)   NOT NULL CONSTRAINT DF_KK_TT DEFAULT (N'DangKiem'),
    GhiChu      NVARCHAR(255)  NULL,
    CONSTRAINT PK_Phieu_KiemKho PRIMARY KEY (MaPhieu),
    CONSTRAINT FK_KK_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT FK_KK_NguoiKiem
        FOREIGN KEY (NguoiKiem) REFERENCES dbo.NhanVien (MaNhanVien)
);
GO

CREATE TABLE dbo.ChiTiet_KiemKho
(
    MaChiTiet         INT             NOT NULL IDENTITY(1,1),
    MaPhieu           INT             NOT NULL,
    MaNguyenLieu      INT             NOT NULL,
    SoLuongHeThong    DECIMAL(18,3)   NOT NULL,
    SoLuongThucTe     DECIMAL(18,3)   NOT NULL,
    ChenhLech         AS (SoLuongThucTe - SoLuongHeThong) PERSISTED,
    CONSTRAINT PK_ChiTiet_KiemKho PRIMARY KEY (MaChiTiet),
    CONSTRAINT FK_CTKK_Phieu
        FOREIGN KEY (MaPhieu) REFERENCES dbo.Phieu_KiemKho (MaPhieu),
    CONSTRAINT FK_CTKK_NL
        FOREIGN KEY (MaNguyenLieu) REFERENCES dbo.NguyenLieu (MaNguyenLieu)
);
GO

/*------------------------------------------------------------------------------
  8. THIẾT BỊ (tài sản dùng lâu dài của chi nhánh, không trừ kho khi bán)
------------------------------------------------------------------------------*/

CREATE TABLE dbo.ThietBi
(
    MaThietBi    INT             NOT NULL IDENTITY(1,1),
    MaCode       NVARCHAR(20)    NOT NULL,       -- mã dán trên thiết bị: TB-CN01-01
    MaChiNhanh   INT             NOT NULL,
    TenThietBi   NVARCHAR(200)   NOT NULL,
    LoaiThietBi  NVARCHAR(30)    NOT NULL,       -- BepNuong / BaoQuan / PhaChe / QuayThuNgan / NoiThat
    SoLuong      INT             NOT NULL
                    CONSTRAINT DF_TB_SoLuong DEFAULT (1),
    NgayMua      DATE            NULL,
    DonGiaMua    DECIMAL(18,2)   NULL,
    TinhTrang    NVARCHAR(20)    NOT NULL        -- DangDung / BaoTri / HuHong / ThanhLy
                    CONSTRAINT DF_TB_TinhTrang DEFAULT (N'DangDung'),
    GhiChu       NVARCHAR(255)   NULL,
    CONSTRAINT PK_ThietBi PRIMARY KEY (MaThietBi),
    CONSTRAINT UQ_ThietBi_MaCode UNIQUE (MaCode),
    CONSTRAINT FK_ThietBi_ChiNhanh
        FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
    CONSTRAINT CK_ThietBi_Loai CHECK (LoaiThietBi IN
        (N'BepNuong', N'BaoQuan', N'PhaChe', N'QuayThuNgan', N'NoiThat')),
    CONSTRAINT CK_ThietBi_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT CK_ThietBi_Gia CHECK (DonGiaMua IS NULL OR DonGiaMua >= 0),
    CONSTRAINT CK_ThietBi_TinhTrang CHECK (TinhTrang IN
        (N'DangDung', N'BaoTri', N'HuHong', N'ThanhLy'))
);
GO

/*------------------------------------------------------------------------------
  INDEX hỗ trợ truy vấn thường dùng
------------------------------------------------------------------------------*/

CREATE INDEX IX_HoaDon_ChiNhanh_Ngay ON dbo.HoaDon (MaChiNhanh, NgayTao);
CREATE INDEX IX_ChiTietHD_HoaDon ON dbo.ChiTiet_HoaDon (MaHoaDon);
CREATE INDEX IX_NhatKyKho_CN_NL_Ngay ON dbo.NhatKy_Kho (MaChiNhanh, MaNguyenLieu, NgayTao);
CREATE INDEX IX_GiaoDichDiem_KH ON dbo.GiaoDich_TichDiem (MaKhachHang);
CREATE INDEX IX_ThietBi_ChiNhanh ON dbo.ThietBi (MaChiNhanh, LoaiThietBi);
CREATE INDEX IX_ComboChiTiet_SanPham ON dbo.Combo_ChiTiet (MaSanPham);
CREATE INDEX IX_KhuyenMai_Ngay ON dbo.KhuyenMai (NgayBatDau, NgayKetThuc);
GO

PRINT N'Đã tạo CSDL QuanLyBanhMi: bảng + PK + FK + UNIQUE + CHECK.';
GO

