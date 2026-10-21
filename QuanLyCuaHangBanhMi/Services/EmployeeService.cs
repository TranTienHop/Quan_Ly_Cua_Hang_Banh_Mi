using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class EmployeeService
    {
        public static DataTable GetAllWithAccounts() => EmployeeRepository.GetAllWithAccounts();
    }
}
