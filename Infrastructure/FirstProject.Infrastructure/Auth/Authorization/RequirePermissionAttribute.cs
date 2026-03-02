using Microsoft.AspNetCore.Authorization;

namespace FirstProject.Infrastructure.Auth.Authorization;

/// <summary>
/// Attribute phân quyền theo permission: chỉ user có permission với Code trùng mới được gọi action.
/// Ví dụ: [RequirePermission("Users.Create")] tương đương [Authorize(Policy = "Permission:Users.Create")].
/// </summary>
public sealed class RequirePermissionAttribute : AuthorizeAttribute
{
    private const string PolicyPrefix = "Permission:";

    public RequirePermissionAttribute(string permissionCode)
        : base(PolicyPrefix + permissionCode) { }
}
