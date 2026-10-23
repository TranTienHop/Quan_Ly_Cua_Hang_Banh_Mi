using System;
using System.Data;
using QuanLyCuaHangBanhMi.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Nghiệp vụ quản trị tài khoản. Mọi thao tác thay đổi đều chỉ dành cho ADMIN.</summary>
    public static class AccountService
    {
        public static DataTable GetAll(string keyword, bool? isActive)
        {
            EnsureAdmin();
            string normalizedKeyword = (keyword ?? string.Empty).Trim();
            if (normalizedKeyword.Length > 100)
                throw new ArgumentException("Từ khóa tìm kiếm không được vượt quá 100 ký tự.", nameof(keyword));

            return AccountRepository.GetAll(normalizedKeyword, isActive);
        }

        public static DataTable GetEmployeesWithoutAccount()
        {
            EnsureAdmin();
            return AccountRepository.GetEmployeesWithoutAccount();
        }

        public static DataTable GetRoles()
        {
            EnsureAdmin();
            return AccountRepository.GetRoles();
        }

        public static DataTable GetActiveBranches()
        {
            EnsureAdmin();
            return AccountRepository.GetActiveBranches();
        }

        public static int Create(AccountCreateModel model)
        {
            EnsureAdmin();
            ArgumentNullException.ThrowIfNull(model);

            string username = NormalizeUsername(model.Username);
            ValidatePassword(model.Password, nameof(model.Password));

            if (model.EmployeeId <= 0)
                throw new ArgumentException("Nhân viên không hợp lệ.", nameof(model.EmployeeId));
            if (model.RoleId <= 0)
                throw new ArgumentException("Vai trò không hợp lệ.", nameof(model.RoleId));
            if (model.BranchId.HasValue && model.BranchId.Value <= 0)
                throw new ArgumentException("Chi nhánh không hợp lệ.", nameof(model.BranchId));

            DataTable employee = AccountRepository.GetEmployeeForAccountCreation(model.EmployeeId);
            if (employee.Rows.Count == 0)
                throw new InvalidOperationException("Nhân viên không tồn tại.");

            DataRow row = employee.Rows[0];
            if (!Convert.ToBoolean(row["NhanVienDangHoatDong"]))
                throw new InvalidOperationException("Không thể cấp tài khoản cho nhân viên đã ngừng hoạt động.");
            if (!Convert.ToBoolean(row["ChiNhanhDangHoatDong"]))
                throw new InvalidOperationException("Không thể cấp tài khoản tại chi nhánh đã ngừng hoạt động.");
            if (Convert.ToBoolean(row["DaCoTaiKhoan"]))
                throw new InvalidOperationException("Nhân viên đã có tài khoản.");

            int employeeBranchId = Convert.ToInt32(row["MaChiNhanh"]);
            if (model.BranchId.HasValue)
            {
                if (!AccountRepository.ActiveBranchExists(model.BranchId.Value))
                    throw new InvalidOperationException("Chi nhánh không tồn tại hoặc đã ngừng hoạt động.");
                if (model.BranchId.Value != employeeBranchId)
                    throw new InvalidOperationException("Nhân viên không thuộc chi nhánh đã chọn.");
            }

            if (!AccountRepository.RoleExists(model.RoleId))
                throw new InvalidOperationException("Vai trò không tồn tại.");
            if (AccountRepository.UsernameExists(username))
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");

            string passwordHash = DatabaseHelper.HashPasswordSHA256(model.Password);
            return AccountRepository.Insert(
                model.EmployeeId, model.RoleId, username, passwordHash, model.IsActive);
        }

        public static void ResetPassword(int accountId, string newPassword)
        {
            EnsureAdmin();
            if (accountId <= 0)
                throw new ArgumentException("Tài khoản không hợp lệ.", nameof(accountId));
            ValidatePassword(newPassword, nameof(newPassword));

            string passwordHash = DatabaseHelper.HashPasswordSHA256(newPassword);
            if (AccountRepository.ResetPassword(accountId, passwordHash) == 0)
                throw new InvalidOperationException("Tài khoản không tồn tại.");
        }

        public static bool ToggleStatus(int accountId)
        {
            EnsureAdmin();
            if (accountId <= 0)
                throw new ArgumentException("Tài khoản không hợp lệ.", nameof(accountId));
            if (accountId == UserSession.UserID)
                throw new InvalidOperationException("Không thể khóa tài khoản đang đăng nhập.");

            bool? newStatus = AccountRepository.ToggleStatus(accountId);
            return newStatus ?? throw new InvalidOperationException("Tài khoản không tồn tại.");
        }

        private static void EnsureAdmin()
        {
            if (!UserSession.HasPermission("SYS_ACCOUNT"))
                throw new UnauthorizedAccessException("Bạn không có quyền quản lý tài khoản.");
        }

        private static string NormalizeUsername(string username)
        {
            string normalized = (username ?? string.Empty).Trim();
            if (normalized.Length < 3 || normalized.Length > 50)
                throw new ArgumentException("Tên đăng nhập phải có từ 3 đến 50 ký tự.", nameof(username));
            if (normalized.IndexOfAny(new[] { ' ', '\t', '\r', '\n' }) >= 0)
                throw new ArgumentException("Tên đăng nhập không được chứa khoảng trắng.", nameof(username));
            return normalized;
        }

        private static void ValidatePassword(string password, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                throw new ArgumentException("Mật khẩu phải có ít nhất 6 ký tự.", parameterName);
            if (password.Length > 128)
                throw new ArgumentException("Mật khẩu không được vượt quá 128 ký tự.", parameterName);
        }
    }
}
