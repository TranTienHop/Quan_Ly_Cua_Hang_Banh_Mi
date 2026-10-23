using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class IngredientRepository
    {
        public static DataTable GetAll(bool lowStockOnly)
        {
            string query = @"
                SELECT nl.MaNguyenLieu AS IngredientID,
                       CONVERT(NVARCHAR(20), nl.MaNguyenLieu) AS [Mã NL],
                       nl.TenNguyenLieu AS [Tên Nguyên Liệu],
                       dv.TenDonVi AS [ĐVT],
                       ISNULL(tk.SoLuongTon, 0) AS [Tồn Kho],
                       nl.TonToiThieuCanhBao AS [Tồn Tối Thiểu],
                       gia.DonGiaNhap AS [Giá Vốn]
                FROM dbo.NguyenLieu nl
                JOIN dbo.DonVi dv ON dv.MaDonVi = nl.MaDonViGoc
                LEFT JOIN dbo.TonKho_ChiNhanh tk
                    ON tk.MaNguyenLieu = nl.MaNguyenLieu
                   AND tk.MaChiNhanh = @MaChiNhanh
                OUTER APPLY
                (
                    SELECT TOP (1) ctn.DonGiaNhap
                    FROM dbo.ChiTiet_NhapHang ctn
                    JOIN dbo.Phieu_NhapHang pn ON pn.MaPhieuNhap = ctn.MaPhieuNhap
                    WHERE ctn.MaNguyenLieu = nl.MaNguyenLieu
                      AND pn.MaChiNhanh = @MaChiNhanh
                    ORDER BY COALESCE(pn.NgayNhan, pn.NgayDat) DESC, pn.MaPhieuNhap DESC
                ) gia
                WHERE nl.DangHoatDong = 1
                  AND (@LowStockOnly = 0 OR ISNULL(tk.SoLuongTon, 0) <= nl.TonToiThieuCanhBao)
                ORDER BY CASE WHEN ISNULL(tk.SoLuongTon, 0) <= nl.TonToiThieuCanhBao THEN 0 ELSE 1 END,
                         nl.TenNguyenLieu";

            return DatabaseHelper.ExecuteQuery(query, new[]
            {
                DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, UserSession.MaChiNhanh),
                DatabaseHelper.Param("@LowStockOnly", SqlDbType.Bit, lowStockOnly)
            });
        }

        public static void Insert(IngredientSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                INSERT INTO dbo.Ingredients (IngredientCode, IngredientName, Unit, CurrentStock, MinimumStock, CostPrice, IsActive)
                VALUES (@Code, @Name, @Unit, @Current, @Min, @Cost, 1)",
                new[]
                {
                    new SqlParameter("@Code", m.Code),
                    new SqlParameter("@Name", m.Name),
                    new SqlParameter("@Unit", m.Unit),
                    new SqlParameter("@Current", m.CurrentStock),
                    new SqlParameter("@Min", m.MinimumStock),
                    new SqlParameter("@Cost", m.CostPrice)
                });
        }

        public static void Update(IngredientSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.Ingredients
                SET IngredientCode = @Code, IngredientName = @Name, Unit = @Unit,
                    CurrentStock = @Current, MinimumStock = @Min, CostPrice = @Cost
                WHERE IngredientID = @ID",
                new[]
                {
                    new SqlParameter("@ID", m.IngredientId),
                    new SqlParameter("@Code", m.Code),
                    new SqlParameter("@Name", m.Name),
                    new SqlParameter("@Unit", m.Unit),
                    new SqlParameter("@Current", m.CurrentStock),
                    new SqlParameter("@Min", m.MinimumStock),
                    new SqlParameter("@Cost", m.CostPrice)
                });
        }
    }
}
