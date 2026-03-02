using Microsoft.AspNetCore.Authorization;

namespace FirstProject.Infrastructure.Auth.Authorization;

/// <summary>
/// Requirement cho policy phân quyền theo permission: user phải có claim "permission" trùng với PermissionCode.
/// Policy name dạng "Permission:Users.Create" → requirement với PermissionCode = "Users.Create".
/// </summary>
public sealed class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string PermissionCode { get; }

    public PermissionAuthorizationRequirement(string permissionCode)
    {
        PermissionCode = permissionCode ?? string.Empty;
    }
}

/// <summary>
/// Handler kiểm tra user có claim "permission" trùng với PermissionCode trong requirement.
/// Claims "permission" được thêm vào JWT khi login/refresh (từ GetUserPermissionsQuery).
/// </summary>
public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    private const string PermissionClaimType = "permission";

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        if (string.IsNullOrEmpty(requirement.PermissionCode))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        var hasPermission = context.User.Claims
            .Where(c => c.Type == PermissionClaimType)
            .Any(c => string.Equals(c.Value, requirement.PermissionCode, StringComparison.OrdinalIgnoreCase));

        if (hasPermission)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
