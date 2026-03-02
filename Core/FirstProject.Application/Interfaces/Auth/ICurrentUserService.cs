namespace FirstProject.Application.Interfaces.Auth;

/// là interface cho việc lấy thông tin người dùng hiện tại từ request context
/// Implementation reads from HttpContext.User (Infrastructure/Host).
public interface ICurrentUserService
{
    Guid? UserId { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
}
