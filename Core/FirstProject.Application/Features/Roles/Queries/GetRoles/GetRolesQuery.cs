using FirstProject.Application.Common;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Roles.Queries.GetRoles;

public record RoleItemDto(Guid RoleId, string RoleName);

public record GetRolesQuery : IRequest<Result<IReadOnlyList<RoleItemDto>>>;

internal sealed class GetRolesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetRolesQuery, Result<IReadOnlyList<RoleItemDto>>>
{
    public async Task<Result<IReadOnlyList<RoleItemDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var list = await unitOfWork
            .Repository<Role>()
            .Entities
            .OrderBy(r => r.RoleName)
            .Select(r => new RoleItemDto(r.RoleId, r.RoleName))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<RoleItemDto>>.Success(list);
    }
}
