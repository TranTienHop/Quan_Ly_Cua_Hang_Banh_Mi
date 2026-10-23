using System;
using System.Collections.Generic;

namespace QuanLyCuaHangBanhMi.Models
{
    public sealed class RoleCreateModel
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public IReadOnlyCollection<int> PermissionIds { get; init; } = Array.Empty<int>();
    }

    public sealed class RoleUpdateModel
    {
        public int RoleId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public IReadOnlyCollection<int> PermissionIds { get; init; } = Array.Empty<int>();
    }
}
