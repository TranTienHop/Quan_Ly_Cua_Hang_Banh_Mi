using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class IngredientService
    {
        public static DataTable GetAll(bool lowStockOnly) => IngredientRepository.GetAll(lowStockOnly);

        public static void Save(IngredientSaveModel model)
        {
            if (model.IngredientId > 0)
                IngredientRepository.Update(model);
            else
                IngredientRepository.Insert(model);
        }
    }
}
