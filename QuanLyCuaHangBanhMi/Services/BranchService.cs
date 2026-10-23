using System;
using System.Data;
using System.Text.RegularExpressions;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Nghiệp vụ quản lý chi nhánh.</summary>
    public static partial class BranchService
    {
        public static DataTable GetAll(string keyword, bool? active, byte? type)
        {
            EnsureCanManageBranches();
            string normalizedKeyword = (keyword ?? string.Empty).Trim();
            if (normalizedKeyword.Length > 100)
                throw new ArgumentException("Từ khóa tìm kiếm không được vượt quá 100 ký tự.", nameof(keyword));
            if (type.HasValue)
                ValidateType(type.Value, nameof(type));

            return BranchRepository.GetAll(normalizedKeyword, active, type);
        }

        public static int Create(BranchCreateModel model)
        {
            EnsureCanManageBranches();
            ArgumentNullException.ThrowIfNull(model);

            string code = NormalizeCode(model.Code);
            string name = NormalizeName(model.Name);
            ValidateType(model.Type, nameof(model.Type));
            string? address = NormalizeAddress(model.Address);
            string? phone = NormalizePhone(model.Phone);

            if (BranchRepository.CodeExists(code))
                throw new InvalidOperationException("Mã chi nhánh đã tồn tại.");

            return BranchRepository.Create(code, name, model.Type, address, phone, model.IsActive);
        }

        public static void Update(BranchUpdateModel model)
        {
            EnsureCanManageBranches();
            ArgumentNullException.ThrowIfNull(model);
            ValidateBranchId(model.BranchId);

            string name = NormalizeName(model.Name);
            ValidateType(model.Type, nameof(model.Type));
            string? address = NormalizeAddress(model.Address);
            string? phone = NormalizePhone(model.Phone);

            if (BranchRepository.Update(model.BranchId, name, model.Type, address, phone) == 0)
                throw new InvalidOperationException("Chi nhánh không tồn tại.");
        }

        public static bool ToggleStatus(int branchId)
        {
            EnsureCanManageBranches();
            ValidateBranchId(branchId);

            BranchToggleResult result = BranchRepository.ToggleStatus(branchId, UserSession.MaChiNhanh);
            return result.Outcome switch
            {
                BranchToggleOutcome.Success => result.NewStatus,
                BranchToggleOutcome.NotFound => throw new InvalidOperationException("Chi nhánh không tồn tại."),
                BranchToggleOutcome.CurrentBranch => throw new InvalidOperationException(
                    "Không thể khóa chi nhánh của tài khoản đang đăng nhập."),
                BranchToggleOutcome.OpenShift => throw new InvalidOperationException(
                    "Không thể khóa chi nhánh khi vẫn còn ca đang mở."),
                _ => throw new InvalidOperationException("Không thể thay đổi trạng thái chi nhánh.")
            };
        }

        private static void EnsureCanManageBranches()
        {
            if (!UserSession.HasPermission("SYS_BRANCH"))
                throw new UnauthorizedAccessException("Bạn không có quyền quản lý chi nhánh.");
        }

        private static void ValidateBranchId(int branchId)
        {
            if (branchId <= 0)
                throw new ArgumentException("Chi nhánh không hợp lệ.", nameof(branchId));
        }

        private static string NormalizeCode(string code)
        {
            string normalized = (code ?? string.Empty).Trim().ToUpperInvariant();
            if (normalized.Length < 2 || normalized.Length > 20)
                throw new ArgumentException("Mã chi nhánh phải có từ 2 đến 20 ký tự.", nameof(code));
            if (!BranchCodePattern().IsMatch(normalized))
                throw new ArgumentException(
                    "Mã chi nhánh chỉ được chứa chữ cái không dấu, chữ số, gạch ngang và gạch dưới.", nameof(code));
            return normalized;
        }

        private static string NormalizeName(string name)
        {
            string normalized = (name ?? string.Empty).Trim();
            if (normalized.Length == 0 || normalized.Length > 200)
                throw new ArgumentException("Tên chi nhánh phải có từ 1 đến 200 ký tự.", nameof(name));
            return normalized;
        }

        private static void ValidateType(byte type, string parameterName)
        {
            if (type is not 1 and not 2)
                throw new ArgumentException("Loại chi nhánh chỉ nhận 1 (cửa hàng) hoặc 2 (lò/kho tổng).", parameterName);
        }

        private static string? NormalizeAddress(string? address)
        {
            string? normalized = address?.Trim();
            if (string.IsNullOrEmpty(normalized))
                return null;
            if (normalized.Length > 255)
                throw new ArgumentException("Địa chỉ không được vượt quá 255 ký tự.", nameof(address));
            return normalized;
        }

        private static string? NormalizePhone(string? phone)
        {
            string? normalized = phone?.Trim();
            if (string.IsNullOrEmpty(normalized))
                return null;
            if (normalized.Length > 20 || !PhonePattern().IsMatch(normalized))
                throw new ArgumentException(
                    "Số điện thoại phải có 7-20 ký tự và chỉ chứa chữ số, khoảng trắng, +, -, (, ).", nameof(phone));
            return normalized;
        }

        [GeneratedRegex("^[A-Z0-9_-]+$", RegexOptions.CultureInvariant)]
        private static partial Regex BranchCodePattern();

        [GeneratedRegex(@"^(?=(?:\D*\d){7,})[0-9+().\s-]{7,20}$", RegexOptions.CultureInvariant)]
        private static partial Regex PhonePattern();
    }
}
