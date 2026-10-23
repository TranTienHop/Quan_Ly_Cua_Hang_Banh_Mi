using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Security;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class CustomerService
    {
        public static DataTable GetMembershipTiers() => CustomerRepository.GetMembershipTiers();
        public static DataTable Search(string keyword) => CustomerRepository.Search(keyword ?? "");
        public static DataTable FindByPhone(string phone) => CustomerRepository.FindByPhone(phone);

        public static void Create(CustomerSaveModel model)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosCustomer);
            CustomerRepository.Insert(model);
        }

        public static void Update(CustomerSaveModel model)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosCustomer);
            CustomerRepository.Update(model);
        }

        public static void Deactivate(int customerId)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosCustomer);
            CustomerRepository.Deactivate(customerId);
        }
    }
}
