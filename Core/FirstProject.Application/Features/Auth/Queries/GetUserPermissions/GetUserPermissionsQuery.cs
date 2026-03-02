using FirstProject.Application.Common;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Auth.Queries.GetUserPermissions;

/// <summary>
/// CQRS Query: lấy danh sách mã permission (Code) của user thông qua các role.
/// Dùng cho phân quyền theo permission (policy RequirePermission).
/// </summary>
public record GetUserPermissionsQuery : IRequest<Result<IReadOnlyList<string>>>
{
    public required Guid UserId { get; init; }
}

internal sealed class GetUserPermissionsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetUserPermissionsQuery, Result<IReadOnlyList<string>>>
{
    public async Task<Result<IReadOnlyList<string>>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
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

        var permissionIds = await unitOfWork
            .Repository<Role>()
            .Entities
            .Where(r => roleIds.Contains(r.RoleId))
            .SelectMany(r => r.PermissionIds)
            .Select(p => p.Value)
            .Distinct()
            .ToListAsync(cancellationToken);

        if (permissionIds.Count == 0)
            return Result<IReadOnlyList<string>>.Success(Array.Empty<string>());

        var codes = await unitOfWork
            .Repository<Permission>()
            .Entities
            .Where(p => permissionIds.Contains(p.PermissionId))
            .Select(p => p.Code)
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<string>>.Success(codes);
    }
}
