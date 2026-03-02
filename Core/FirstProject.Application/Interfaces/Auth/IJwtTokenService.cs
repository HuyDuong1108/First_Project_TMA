using System.Security.Claims;

namespace FirstProject.Application.Interfaces.Auth;

/// là interface cho việc tạo và xác thực token JWT
public interface IJwtTokenService
{
    /// <param name="roles">Tên role (RoleName) dùng cho [Authorize(Roles = "...")].</param>
    /// <param name="permissionCodes">Mã permission (Permission.Code) dùng cho policy RequirePermission.</param>
    string GenerateAccessToken(Guid userId, string email, IReadOnlyList<string>? roles = null, IReadOnlyList<string>? permissionCodes = null);
    /// Đây là phương thức để xác thực token JWT
    /// <param name="token">Token JWT cần xác thực</param>
    /// <returns>ClaimsPrincipal nếu token hợp lệ, null nếu không</returns>
    ClaimsPrincipal? ValidateAccessToken(string token);
}
