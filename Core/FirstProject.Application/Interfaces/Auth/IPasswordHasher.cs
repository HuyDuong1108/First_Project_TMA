namespace FirstProject.Application.Interfaces.Auth;

/// Đây là interface cho việc hash và verify password
public interface IPasswordHasher
{
    string Hash(string password);
    /// Đây là phương thức để verify password
    /// <param name="password">Mật khẩu cần verify</param>
    /// <param name="hash">Mật khẩu đã hash</param>
    /// <returns>True nếu password hợp lệ, false nếu không</returns>
    bool Verify(string password, string hash);
}
