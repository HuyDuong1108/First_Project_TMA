using FirstProject.Application.Common;
using FirstProject.Application.Interfaces.Auth;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Auth.Queries.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser) : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserDto>>
{
    public async Task<Result<CurrentUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is not { } userId)
            return Result<CurrentUserDto>.Failure("Chưa đăng nhập.");

        User? user = await unitOfWork
            .Repository<User>()
            .Entities
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

        if (user is null)
            return Result<CurrentUserDto>.Failure("Người dùng không tồn tại.");

        var dto = new CurrentUserDto(user.UserId, user.Email, user.FullName, user.Status);
        return Result<CurrentUserDto>.Success(dto);
    }
}
