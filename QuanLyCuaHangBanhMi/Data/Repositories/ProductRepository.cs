using System.Data;
using Microsoft.Data.SqlClient;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class ProductRepository
    {
        public static DataTable GetActiveCategories()
        {
            return DatabaseHelper.ExecuteQuery(
                "SELECT CategoryID, CategoryName FROM dbo.Categories WHERE IsActive = 1");
        }

        public static DataTable GetAllProducts()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT p.ProductID, p.ProductCode AS [Mã Món], p.ProductName AS [Tên Món],
                       c.CategoryName AS [Danh Mục], p.SellingPrice AS [Giá Bán (VNĐ)],
                       p.Unit AS [ĐVT], p.IsActive AS [Hoạt Động], p.CategoryID
                FROM dbo.Products p
                JOIN dbo.Categories c ON p.CategoryID = c.CategoryID
                ORDER BY p.ProductID");
        }

        public static void Insert(ProductSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                INSERT INTO dbo.Products (ProductCode, ProductName, CategoryID, SellingPrice, Unit, IsActive)
                VALUES (@Code, @Name, @CatID, @Price, @Unit, @IsActive)",
                new[]
                {
                    new SqlParameter("@Code", m.Code),
                    new SqlParameter("@Name", m.Name),
                    new SqlParameter("@CatID", m.CategoryId),
                    new SqlParameter("@Price", m.Price),
                    new SqlParameter("@Unit", m.Unit),
                    new SqlParameter("@IsActive", m.IsActive)
                });
        }

        public static void Update(ProductSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.Products
                SET ProductCode = @Code, ProductName = @Name, CategoryID = @CatID,
                    SellingPrice = @Price, Unit = @Unit, IsActive = @IsActive, UpdatedAt = SYSDATETIME()
                WHERE ProductID = @ProductID",
                new[]
                {
                    new SqlParameter("@ProductID", m.ProductId),
                    new SqlParameter("@Code", m.Code),
                    new SqlParameter("@Name", m.Name),
                    new SqlParameter("@CatID", m.CategoryId),
                    new SqlParameter("@Price", m.Price),
                    new SqlParameter("@Unit", m.Unit),
                    new SqlParameter("@IsActive", m.IsActive)
                });
        }

        public static void Deactivate(int productId)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE dbo.Products SET IsActive = 0 WHERE ProductID = @ProductID",
                new[] { new SqlParameter("@ProductID", productId) });
        }
    }
}
