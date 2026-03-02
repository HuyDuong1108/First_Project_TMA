namespace FirstProject.Infrastructure.Auth;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "FirstProject";
    public string Audience { get; set; } = "FirstProject";
    public int ExpiryMinutes { get; set; } = 30;
}
