using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class CustomerService
    {
        public static DataTable GetMembershipTiers() => CustomerRepository.GetMembershipTiers();
        public static DataTable Search(string keyword) => CustomerRepository.Search(keyword ?? "");
        public static DataTable FindByPhone(string phone) => CustomerRepository.FindByPhone(phone);

        public static void Create(CustomerSaveModel model) => CustomerRepository.Insert(model);
        public static void Update(CustomerSaveModel model) => CustomerRepository.Update(model);
        public static void Deactivate(int customerId) => CustomerRepository.Deactivate(customerId);
    }
}
