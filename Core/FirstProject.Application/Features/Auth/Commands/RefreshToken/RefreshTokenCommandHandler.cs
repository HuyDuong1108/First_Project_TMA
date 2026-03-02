using FirstProject.Application.Common;
using FirstProject.Application.Features.Auth.Queries.GetUserPermissions;
using FirstProject.Application.Features.Auth.Queries.GetUserRoles;
using FirstProject.Application.Interfaces.Auth;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Auth.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IJwtTokenService jwtTokenService,
    ICurrentUserService currentUser) : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
            return Result<RefreshTokenResponse>.Failure("Chưa đăng nhập.");

        User? user = await unitOfWork
            .Repository<User>()
            .Entities
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user is null)
            return Result<RefreshTokenResponse>.Failure("Người dùng không tồn tại.");

        if (!user.Status)
            return Result<RefreshTokenResponse>.Failure("Tài khoản đã bị vô hiệu hóa.");

        var rolesResult = await mediator.Send(new GetUserRolesQuery { UserId = user.UserId }, cancellationToken);
        var permissionsResult = await mediator.Send(new GetUserPermissionsQuery { UserId = user.UserId }, cancellationToken);
        var roleNames = rolesResult.IsSuccess ? (rolesResult.Value ?? Array.Empty<string>()) : Array.Empty<string>();
        var permissionCodes = permissionsResult.IsSuccess ? (permissionsResult.Value ?? Array.Empty<string>()) : Array.Empty<string>();

        string token = jwtTokenService.GenerateAccessToken(user.UserId, user.Email, roleNames, permissionCodes);
        var expiresAt = DateTime.UtcNow.AddMinutes(30);

        return Result<RefreshTokenResponse>.Success(new RefreshTokenResponse(token, expiresAt));
    }
}
