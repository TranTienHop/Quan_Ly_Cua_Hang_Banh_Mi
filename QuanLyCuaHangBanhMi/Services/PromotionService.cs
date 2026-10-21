using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class PromotionService
    {
        public static DataTable GetActiveBranches() => PromotionRepository.GetActiveBranches();
        public static DataTable GetAll() => PromotionRepository.GetAll();
        public static DataTable GetActiveForBranch(int maChiNhanh) => PromotionRepository.GetActiveForBranch(maChiNhanh);

        public static void Create(PromoSaveModel model) => PromotionRepository.Insert(model);
        public static void Update(PromoSaveModel model) => PromotionRepository.Update(model);
        public static void Deactivate(int promoId) => PromotionRepository.Deactivate(promoId);
    }
}
