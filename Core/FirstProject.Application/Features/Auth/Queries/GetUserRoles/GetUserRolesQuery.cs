using FirstProject.Application.Common;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Auth.Queries.GetUserRoles;

/// <summary>
/// CQRS Query: lấy danh sách tên role của user (dùng cho JWT claims và [Authorize(Roles = "...")]).
/// </summary>
public record GetUserRolesQuery : IRequest<Result<IReadOnlyList<string>>>
{
    public required Guid UserId { get; init; }
}

internal sealed class GetUserRolesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserRolesQuery, Result<IReadOnlyList<string>>>
{
    public async Task<Result<IReadOnlyList<string>>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
    {
        var roleIds = await unitOfWork
            .Repository<User>()
            .Entities
            .Where(u => u.UserId == request.UserId)
            .SelectMany(u => u.RoleIds)
            .Select(r => r.Value)
            .ToListAsync(cancellationToken);

        if (roleIds.Count == 0)
            return Result<IReadOnlyList<string>>.Success(Array.Empty<string>());

        var roleNames = await unitOfWork
            .Repository<Role>()
            .Entities
            .Where(r => roleIds.Contains(r.RoleId))
            .Select(r => r.RoleName)
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<string>>.Success(roleNames);
    }
}
