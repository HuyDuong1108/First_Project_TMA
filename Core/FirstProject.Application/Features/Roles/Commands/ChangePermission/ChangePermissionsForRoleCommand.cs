using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Roles.Commands.ChangePermission
{
    public record ChangePermissionsForRoleCommand : IRequest<Result<Guid>>, IMapFrom<Role>
    {
        public required Guid? RoleId { get; init; }
        public required IReadOnlyList<Guid> PermissionIds { get; init; }
    }

    internal class ChangePermissionsForRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangePermissionsForRoleCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(ChangePermissionsForRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.RoleId is not { } roleId)
            {
                return Result<Guid>.Failure("Cần nhập RoleId.");
            }

            Role? role = await unitOfWork.Repository<Role>().Entities
                .FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
            if (role is null)
            {
                return Result<Guid>.Failure("Role không tìm thấy.");
            }

            if (command.PermissionIds is not { Count: > 0 } permissionIds)
            {
                role.ClearPermissionIds();
                _ = await unitOfWork.Save(cancellationToken);
                return Result<Guid>.Success(roleId);
            }

            bool allPermissionsExist = await unitOfWork.Repository<Permission>().Entities
                .Where(p => permissionIds.Contains(p.PermissionId))
                .CountAsync(cancellationToken) == permissionIds.Count;
            if (!allPermissionsExist)
            {
                return Result<Guid>.Failure("Một hoặc nhiều PermissionId không tồn tại.");
            }

            role.ClearPermissionIds();
            foreach (Guid permissionId in permissionIds.Distinct())
            {
                role.AddPermissionId(new Role.PermissionId(permissionId));
            }

            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(roleId);
        }
    }
}
