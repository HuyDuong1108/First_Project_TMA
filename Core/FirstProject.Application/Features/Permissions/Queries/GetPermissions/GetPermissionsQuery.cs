using FirstProject.Application.Common;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Permissions.Queries.GetPermissions;

public record PermissionItemDto(Guid PermissionId, string Code, string Description);

public record GetPermissionsQuery : IRequest<Result<IReadOnlyList<PermissionItemDto>>>;

internal sealed class GetPermissionsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetPermissionsQuery, Result<IReadOnlyList<PermissionItemDto>>>
{
    public async Task<Result<IReadOnlyList<PermissionItemDto>>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        var list = await unitOfWork
            .Repository<Permission>()
            .Entities
            .OrderBy(p => p.Code)
            .Select(p => new PermissionItemDto(p.PermissionId, p.Code, p.Description))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<PermissionItemDto>>.Success(list);
    }
}
