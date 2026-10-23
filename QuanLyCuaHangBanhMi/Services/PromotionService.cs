using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Security;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class PromotionService
    {
        public static DataTable GetActiveBranches() => PromotionRepository.GetActiveBranches();
        public static DataTable GetAll() => PromotionRepository.GetAll();
        public static DataTable GetActiveForBranch(int maChiNhanh) => PromotionRepository.GetActiveForBranch(maChiNhanh);

        public static void Create(PromoSaveModel model)
        {
            AuthorizationGuard.Demand(PermissionCodes.MenuPromotion);
            PromotionRepository.Insert(model);
        }

        public static void Update(PromoSaveModel model)
        {
            AuthorizationGuard.Demand(PermissionCodes.MenuPromotion);
            PromotionRepository.Update(model);
        }

        public static void Deactivate(int promoId)
        {
            AuthorizationGuard.Demand(PermissionCodes.MenuPromotion);
            PromotionRepository.Deactivate(promoId);
        }
    }
}
