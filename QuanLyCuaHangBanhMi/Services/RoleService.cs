using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Nghiệp vụ quản lý vai trò và phân quyền menu.</summary>
    public static partial class RoleService
    {
        public static DataTable GetAll(string keyword)
        {
            EnsureCanManageRoles();
            string normalizedKeyword = (keyword ?? string.Empty).Trim();
            if (normalizedKeyword.Length > 100)
                throw new ArgumentException("Từ khóa tìm kiếm không được vượt quá 100 ký tự.", nameof(keyword));

            return RoleRepository.GetAll(normalizedKeyword);
        }

        public static DataTable GetMenus()
        {
            EnsureCanManageRoles();
            return RoleRepository.GetMenus();
        }

        public static IReadOnlyList<int> GetPermissionIds(int roleId)
        {
            EnsureCanManageRoles();
            ValidateRoleId(roleId);
            if (!RoleRepository.RoleExists(roleId))
                throw new InvalidOperationException("Vai trò không tồn tại.");

            return RoleRepository.GetPermissionIds(roleId);
        }

        public static int Create(RoleCreateModel model)
        {
            EnsureCanManageRoles();
            ArgumentNullException.ThrowIfNull(model);

            string code = NormalizeCode(model.Code);
            string name = NormalizeName(model.Name);
            string? description = NormalizeDescription(model.Description);
            int[] permissionIds = NormalizePermissionIds(model.PermissionIds);

            if (RoleRepository.CodeExists(code))
                throw new InvalidOperationException("Mã vai trò đã tồn tại.");

            return RoleRepository.Create(code, name, description, permissionIds);
        }

        public static void Update(RoleUpdateModel model)
        {
            EnsureCanManageRoles();
            ArgumentNullException.ThrowIfNull(model);
            ValidateRoleId(model.RoleId);

            string name = NormalizeName(model.Name);
            string? description = NormalizeDescription(model.Description);
            int[] permissionIds = NormalizePermissionIds(model.PermissionIds);
            RoleRepository.Update(model.RoleId, name, description, permissionIds);
        }

        private static void EnsureCanManageRoles()
        {
            if (!UserSession.HasPermission("SYS_ROLE"))
                throw new UnauthorizedAccessException("Bạn không có quyền quản lý vai trò.");
        }

        private static void ValidateRoleId(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("Vai trò không hợp lệ.", nameof(roleId));
        }

        private static string NormalizeCode(string code)
        {
            string normalized = (code ?? string.Empty).Trim().ToUpperInvariant();
            if (normalized.Length < 2 || normalized.Length > 30)
                throw new ArgumentException("Mã vai trò phải có từ 2 đến 30 ký tự.", nameof(code));
            if (!RoleCodePattern().IsMatch(normalized))
                throw new ArgumentException(
                    "Mã vai trò chỉ được chứa chữ cái không dấu, chữ số và dấu gạch dưới.", nameof(code));
            return normalized;
        }

        private static string NormalizeName(string name)
        {
            string normalized = (name ?? string.Empty).Trim();
            if (normalized.Length == 0 || normalized.Length > 100)
                throw new ArgumentException("Tên vai trò phải có từ 1 đến 100 ký tự.", nameof(name));
            return normalized;
        }

        private static string? NormalizeDescription(string? description)
        {
            string? normalized = description?.Trim();
            if (string.IsNullOrEmpty(normalized))
                return null;
            if (normalized.Length > 255)
                throw new ArgumentException("Mô tả không được vượt quá 255 ký tự.", nameof(description));
            return normalized;
        }

        private static int[] NormalizePermissionIds(IReadOnlyCollection<int>? permissionIds)
        {
            if (permissionIds is null)
                return Array.Empty<int>();
            if (permissionIds.Any(id => id <= 0))
                throw new ArgumentException("Danh sách quyền không hợp lệ.", nameof(permissionIds));
            return permissionIds.Distinct().ToArray();
        }

        [GeneratedRegex("^[A-Z0-9_]+$", RegexOptions.CultureInvariant)]
        private static partial Regex RoleCodePattern();
    }
}
