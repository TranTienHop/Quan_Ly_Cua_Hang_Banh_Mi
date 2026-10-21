using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class IngredientRepository
    {
        public static DataTable GetAll(bool lowStockOnly)
        {
            string query = lowStockOnly
                ? @"
                    SELECT IngredientID, IngredientCode AS [Mã NL], IngredientName AS [Tên Nguyên Liệu],
                           Unit AS [ĐVT], CurrentStock AS [Tồn Kho], MinimumStock AS [Tồn Tối Thiểu], CostPrice AS [Giá Vốn]
                    FROM dbo.Ingredients
                    WHERE CurrentStock <= MinimumStock AND IsActive = 1
                    ORDER BY CurrentStock ASC"
                : @"
                    SELECT IngredientID, IngredientCode AS [Mã NL], IngredientName AS [Tên Nguyên Liệu],
                           Unit AS [ĐVT], CurrentStock AS [Tồn Kho], MinimumStock AS [Tồn Tối Thiểu], CostPrice AS [Giá Vốn]
                    FROM dbo.Ingredients
                    WHERE IsActive = 1
                    ORDER BY IngredientID";

            return DatabaseHelper.ExecuteQuery(query);
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
