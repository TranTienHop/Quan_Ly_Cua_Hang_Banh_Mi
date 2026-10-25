/*
    Draft migration for selected Login features.
    Applied on the configured LocalDB QuanLyBanhMi on 2026-10-23 after user approval.
    Back up the target database before any future re-run or production deployment.
    This script is additive and does not drop or overwrite existing data.
    It is never executed automatically by the application or build.
*/

SET XACT_ABORT ON;

IF DB_NAME() <> N'QuanLyBanhMi'
BEGIN
    THROW 50010, N'Phải chọn đúng database QuanLyBanhMi trước khi chạy migration.', 1;
END;

IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
   OR OBJECT_ID(N'dbo.NhanVien', N'U') IS NULL
   OR OBJECT_ID(N'dbo.ChiNhanh', N'U') IS NULL
BEGIN
    THROW 50011, N'Thiếu bảng nền TaiKhoan, NhanVien hoặc ChiNhanh; migration đã dừng.', 1;
END;

BEGIN TRY
    BEGIN TRANSACTION;

    IF COL_LENGTH(N'dbo.TaiKhoan', N'SoLanDangNhapThatBai') IS NULL
    BEGIN
        ALTER TABLE dbo.TaiKhoan
            ADD SoLanDangNhapThatBai TINYINT NOT NULL
                CONSTRAINT DF_TaiKhoan_SoLanDangNhapThatBai DEFAULT (0);
    END;

    IF COL_LENGTH(N'dbo.TaiKhoan', N'KhoaDen') IS NULL
    BEGIN
        ALTER TABLE dbo.TaiKhoan
            ADD KhoaDen DATETIME2(0) NULL;
    END;

    IF OBJECT_ID(N'dbo.DanhMuc_CaLam', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.DanhMuc_CaLam
        (
            MaCaLam       INT            NOT NULL IDENTITY(1,1),
            MaCode        NVARCHAR(30)   NOT NULL,
            TenCa         NVARCHAR(100)  NOT NULL,
            GioBatDau     TIME(0)        NOT NULL,
            GioKetThuc    TIME(0)        NOT NULL,
            DangHoatDong  BIT            NOT NULL
                CONSTRAINT DF_DanhMuc_CaLam_DangHoatDong DEFAULT (1),
            CONSTRAINT PK_DanhMuc_CaLam PRIMARY KEY (MaCaLam),
            CONSTRAINT UQ_DanhMuc_CaLam_MaCode UNIQUE (MaCode),
            CONSTRAINT CK_DanhMuc_CaLam_GioKhacNhau CHECK (GioBatDau <> GioKetThuc)
        );
    END
    ELSE IF COL_LENGTH(N'dbo.DanhMuc_CaLam', N'MaCaLam') IS NULL
         OR COL_LENGTH(N'dbo.DanhMuc_CaLam', N'MaCode') IS NULL
         OR COL_LENGTH(N'dbo.DanhMuc_CaLam', N'TenCa') IS NULL
         OR COL_LENGTH(N'dbo.DanhMuc_CaLam', N'GioBatDau') IS NULL
         OR COL_LENGTH(N'dbo.DanhMuc_CaLam', N'GioKetThuc') IS NULL
         OR COL_LENGTH(N'dbo.DanhMuc_CaLam', N'DangHoatDong') IS NULL
         OR OBJECT_ID(N'dbo.PK_DanhMuc_CaLam', N'PK') IS NULL
         OR OBJECT_ID(N'dbo.UQ_DanhMuc_CaLam_MaCode', N'UQ') IS NULL
         OR OBJECT_ID(N'dbo.CK_DanhMuc_CaLam_GioKhacNhau', N'C') IS NULL
    BEGIN
        THROW 50012, N'Bảng dbo.DanhMuc_CaLam đã tồn tại nhưng không đúng schema dự kiến; cần rà soát thủ công.', 1;
    END;

    IF OBJECT_ID(N'dbo.LichPhanCa', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.LichPhanCa
        (
            MaLich       INT            NOT NULL IDENTITY(1,1),
            MaCaLam      INT            NOT NULL,
            MaNhanVien   INT            NOT NULL,
            MaChiNhanh   INT            NOT NULL,
            NgayLam      DATE           NOT NULL,
            TrangThai    NVARCHAR(20)   NOT NULL
                CONSTRAINT DF_LichPhanCa_TrangThai DEFAULT (N'DaXep'),
            GhiChu       NVARCHAR(255)  NULL,
            CONSTRAINT PK_LichPhanCa PRIMARY KEY (MaLich),
            CONSTRAINT UQ_LichPhanCa_NhanVien_Ca_Ngay
                UNIQUE (MaNhanVien, MaCaLam, NgayLam),
            CONSTRAINT FK_LichPhanCa_CaLam
                FOREIGN KEY (MaCaLam) REFERENCES dbo.DanhMuc_CaLam (MaCaLam),
            CONSTRAINT FK_LichPhanCa_NhanVien
                FOREIGN KEY (MaNhanVien) REFERENCES dbo.NhanVien (MaNhanVien),
            CONSTRAINT FK_LichPhanCa_ChiNhanh
                FOREIGN KEY (MaChiNhanh) REFERENCES dbo.ChiNhanh (MaChiNhanh),
            CONSTRAINT CK_LichPhanCa_TrangThai
                CHECK (TrangThai IN (N'DaXep', N'HoanThanh', N'Huy'))
        );
    END
    ELSE IF COL_LENGTH(N'dbo.LichPhanCa', N'MaLich') IS NULL
         OR COL_LENGTH(N'dbo.LichPhanCa', N'MaCaLam') IS NULL
         OR COL_LENGTH(N'dbo.LichPhanCa', N'MaNhanVien') IS NULL
         OR COL_LENGTH(N'dbo.LichPhanCa', N'MaChiNhanh') IS NULL
         OR COL_LENGTH(N'dbo.LichPhanCa', N'NgayLam') IS NULL
         OR COL_LENGTH(N'dbo.LichPhanCa', N'TrangThai') IS NULL
         OR OBJECT_ID(N'dbo.PK_LichPhanCa', N'PK') IS NULL
         OR OBJECT_ID(N'dbo.UQ_LichPhanCa_NhanVien_Ca_Ngay', N'UQ') IS NULL
         OR OBJECT_ID(N'dbo.FK_LichPhanCa_CaLam', N'F') IS NULL
         OR OBJECT_ID(N'dbo.FK_LichPhanCa_NhanVien', N'F') IS NULL
         OR OBJECT_ID(N'dbo.FK_LichPhanCa_ChiNhanh', N'F') IS NULL
         OR OBJECT_ID(N'dbo.CK_LichPhanCa_TrangThai', N'C') IS NULL
    BEGIN
        THROW 50013, N'Bảng dbo.LichPhanCa đã tồn tại nhưng không đúng schema dự kiến; cần rà soát thủ công.', 1;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM sys.indexes
        WHERE object_id = OBJECT_ID(N'dbo.LichPhanCa')
          AND name = N'IX_LichPhanCa_Ngay_ChiNhanh'
    )
    BEGIN
        CREATE INDEX IX_LichPhanCa_Ngay_ChiNhanh
            ON dbo.LichPhanCa (NgayLam, MaChiNhanh)
            INCLUDE (MaNhanVien, MaCaLam, TrangThai);
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
