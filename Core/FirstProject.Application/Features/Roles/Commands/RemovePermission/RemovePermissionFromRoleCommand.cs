using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Roles.Commands.RemovePermission
{
    public record RemovePermissionFromRoleCommand : IRequest<Result<Guid>>, IMapFrom<Role>
    {
        public required Guid? PermissionId { get; init; }
        public required Guid? RoleId { get; init; }
    }

    internal class RemovePermissionFromRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemovePermissionFromRoleCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RemovePermissionFromRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.RoleId is not { } roleId)
            {
                return Result<Guid>.Failure("Cần nhập RoleId.");
            }
            if (command.PermissionId is not { } permissionId)
            {
                return Result<Guid>.Failure("Cần nhập PermissionId.");
            }

            Role? role = await unitOfWork.Repository<Role>().Entities
                .FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
            if (role is null)
            {
                return Result<Guid>.Failure("Role không tìm thấy.");
            }

            if (!role.PermissionIds.Any(pid => pid.Value == permissionId))
            {
                return Result<Guid>.Failure("Role không có Permission này.");
            }

            role.RemovePermissionId(new Role.PermissionId(permissionId));
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(roleId);
        }
    }
}
