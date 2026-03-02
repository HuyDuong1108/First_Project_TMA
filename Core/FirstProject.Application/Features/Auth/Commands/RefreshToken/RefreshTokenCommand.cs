using FirstProject.Application.Common;
using MediatR;

namespace FirstProject.Application.Features.Auth.Commands.RefreshToken;

/// <summary>
/// Re-issues an access token for the current user (valid existing token required).
/// </summary>
public record RefreshTokenCommand : IRequest<Result<RefreshTokenResponse>>;

public record RefreshTokenResponse(string AccessToken, DateTime ExpiresAt);
