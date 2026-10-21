using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class ProductService
    {
        public static DataTable GetActiveCategories() => ProductRepository.GetActiveCategories();
        public static DataTable GetAllProducts() => ProductRepository.GetAllProducts();

        public static void Create(ProductSaveModel model) => ProductRepository.Insert(model);
        public static void Update(ProductSaveModel model) => ProductRepository.Update(model);
        public static void Deactivate(int productId) => ProductRepository.Deactivate(productId);
    }
}
