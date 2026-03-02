using FirstProject.Application.Common;
using FirstProject.Application.Features.Auth.Queries.GetUserPermissions;
using FirstProject.Application.Features.Auth.Queries.GetUserRoles;
using FirstProject.Application.Interfaces.Auth;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FirstProject.Application.Features.Auth.Commands.Login;

public record LoginCommand : IRequest<Result<LoginResponse>>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public record LoginResponse(string AccessToken, Guid UserId, string Email, string FullName, DateTime ExpiresAt);
internal sealed class LoginCommandHandler(
    IUnitOfWork unitOfWork,
    IMediator mediator,
    IJwtTokenService jwtTokenService,
    IPasswordHasher passwordHasher) : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await unitOfWork
            .Repository<User>()
            .Entities
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null)
            return Result<LoginResponse>.Failure("Email hoặc mật khẩu không đúng.");

        if (!user.Status)
            return Result<LoginResponse>.Failure("Tài khoản đã bị vô hiệu hóa.");

        bool passwordValid = user.Password.StartsWith("$")
            ? passwordHasher.Verify(request.Password, user.Password)
            : user.Password == request.Password;

        if (!passwordValid)
            return Result<LoginResponse>.Failure("Email hoặc mật khẩu không đúng.");

        var rolesResult = await mediator.Send(new GetUserRolesQuery { UserId = user.UserId }, cancellationToken);
        var permissionsResult = await mediator.Send(new GetUserPermissionsQuery { UserId = user.UserId }, cancellationToken);
        var roleNames = rolesResult.IsSuccess ? (rolesResult.Value ?? Array.Empty<string>()) : Array.Empty<string>();
        var permissionCodes = permissionsResult.IsSuccess ? (permissionsResult.Value ?? Array.Empty<string>()) : Array.Empty<string>();

        string token = jwtTokenService.GenerateAccessToken(user.UserId, user.Email, roleNames, permissionCodes);
        var expiresAt = DateTime.UtcNow.AddMinutes(30); // should match JWT options

        var response = new LoginResponse(token, user.UserId, user.Email, user.FullName, expiresAt);
        return Result<LoginResponse>.Success(response);
    }
}