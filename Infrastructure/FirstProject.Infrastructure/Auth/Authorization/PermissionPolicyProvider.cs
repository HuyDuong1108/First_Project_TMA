using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FirstProject.Infrastructure.Auth.Authorization;

/// <summary>
/// Policy provider động: policy name "Permission:&lt;code&gt;" (vd: "Permission:Users.Create")
/// sẽ tạo policy với PermissionAuthorizationRequirement(code).
/// Các policy khác delegate cho DefaultAuthorizationPolicyProvider.
/// </summary>
public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private const string PermissionPolicyPrefix = "Permission:";
    private readonly DefaultAuthorizationPolicyProvider _fallback;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallback = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => _fallback.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => _fallback.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(PermissionPolicyPrefix, StringComparison.OrdinalIgnoreCase))
        {
            var permissionCode = policyName[PermissionPolicyPrefix.Length..].Trim();
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new PermissionAuthorizationRequirement(permissionCode))
                .Build();
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallback.GetPolicyAsync(policyName);
    }
}
