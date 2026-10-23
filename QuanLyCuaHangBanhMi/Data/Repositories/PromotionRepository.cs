using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class PromotionRepository
    {
        public static DataTable GetActiveBranches()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaChiNhanh, TenChiNhanh
                FROM dbo.ChiNhanh
                WHERE LoaiChiNhanh = 1 AND DangHoatDong = 1
                ORDER BY TenChiNhanh");
        }

        public static DataTable GetAll()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT km.MaKhuyenMai, km.MaCode AS [Mã], km.TenKhuyenMai AS [Tên KM],
                       CASE km.LoaiGiam WHEN N'PhanTram' THEN N'%' ELSE N'Tiền' END AS [Loại],
                       km.GiaTriGiam AS [Giá trị], km.GiamToiDa AS [Trần], km.DonToiThieu AS [Đơn TT],
                       ISNULL(cn.TenChiNhanh, N'Tất cả') AS [Chi nhánh],
                       km.NgayBatDau AS [Bắt đầu], km.NgayKetThuc AS [Kết thúc],
                       km.DangHoatDong AS [HĐ], km.LoaiGiam, km.MaChiNhanh
                FROM dbo.KhuyenMai km
                LEFT JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = km.MaChiNhanh
                ORDER BY km.NgayBatDau DESC");
        }

        public static DataTable GetActiveForBranch(int maChiNhanh)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaKhuyenMai, MaCode, TenKhuyenMai, LoaiGiam, GiaTriGiam, GiamToiDa, DonToiThieu, NgayKetThuc
                FROM dbo.KhuyenMai
                WHERE DangHoatDong = 1
                  AND (MaChiNhanh IS NULL OR MaChiNhanh = @MaChiNhanh)
                  AND SYSDATETIME() BETWEEN NgayBatDau AND NgayKetThuc
                ORDER BY TenKhuyenMai",
                new[] { DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, maChiNhanh) });
        }

        public static void Insert(PromoSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                INSERT INTO dbo.KhuyenMai
                    (MaCode, TenKhuyenMai, LoaiGiam, GiaTriGiam, GiamToiDa, DonToiThieu,
                     MaChiNhanh, NgayBatDau, NgayKetThuc, DangHoatDong)
                VALUES
                    (@Code, @Ten, @Loai, @GiaTri, @GiamToiDa, @DonTT,
                     @MaCN, @BatDau, @KetThuc, @Active)",
                BuildParams(m));
        }

        public static void Update(PromoSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.KhuyenMai
                SET MaCode = @Code, TenKhuyenMai = @Ten, LoaiGiam = @Loai, GiaTriGiam = @GiaTri,
                    GiamToiDa = @GiamToiDa, DonToiThieu = @DonTT, MaChiNhanh = @MaCN,
                    NgayBatDau = @BatDau, NgayKetThuc = @KetThuc, DangHoatDong = @Active
                WHERE MaKhuyenMai = @ID",
                BuildParams(m, includeId: true));
        }

        public static void Deactivate(int promoId)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE dbo.KhuyenMai SET DangHoatDong = 0 WHERE MaKhuyenMai = @ID",
                new[] { DatabaseHelper.Param("@ID", SqlDbType.Int, promoId) });
        }

        private static SqlParameter[] BuildParams(PromoSaveModel m, bool includeId = false)
        {
            var list = new List<SqlParameter>
            {
                DatabaseHelper.Param("@Code", SqlDbType.NVarChar, m.Code, 30),
                DatabaseHelper.Param("@Ten", SqlDbType.NVarChar, m.Name, 200),
                DatabaseHelper.Param("@Loai", SqlDbType.NVarChar, m.LoaiGiam, 20),
                DatabaseHelper.Param("@GiaTri", SqlDbType.Decimal, m.GiaTri),
                DatabaseHelper.Param("@GiamToiDa", SqlDbType.Decimal, (object?)m.GiamToiDa ?? DBNull.Value),
                DatabaseHelper.Param("@DonTT", SqlDbType.Decimal, m.DonToiThieu),
                DatabaseHelper.Param("@MaCN", SqlDbType.Int, (object?)m.MaChiNhanh ?? DBNull.Value),
                DatabaseHelper.Param("@BatDau", SqlDbType.DateTime2, m.NgayBatDau),
                DatabaseHelper.Param("@KetThuc", SqlDbType.DateTime2, m.NgayKetThuc),
                DatabaseHelper.Param("@Active", SqlDbType.Bit, m.DangHoatDong)
            };
            if (includeId)
                list.Insert(0, DatabaseHelper.Param("@ID", SqlDbType.Int, m.PromoId));
            return list.ToArray();
        }
    }
}
