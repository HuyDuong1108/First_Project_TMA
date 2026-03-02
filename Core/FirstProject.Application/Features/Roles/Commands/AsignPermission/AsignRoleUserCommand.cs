using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Roles.Commands.AsignPermission
{
    public record AsignPermissionCommand : IRequest<Result<Guid>>, IMapFrom<Role>
    {
        public required Guid? PermissionId { get; init; }
        public required Guid? RoleId { get; init; }
    }

    internal class AsignPermissionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AsignPermissionCommand, Result<Guid>>
    {

        public async Task<Result<Guid>> Handle(AsignPermissionCommand command, CancellationToken cancellationToken)
        {
            if (command.RoleId is not { } roleId)
            {
                return Result<Guid>.Failure("Cần nhập RoleId.");
            }
            if (command.PermissionId is not { } permissionId)
            {
                return Result<Guid>.Failure("Cần nhập PermissionId.");
            }

            // Kiểm tra Permission có tồn tại trong hệ thống
            bool permissionExists = await unitOfWork.Repository<Permission>().Entities
                .AnyAsync(p => p.PermissionId == permissionId, cancellationToken);
            if (!permissionExists)
            {
                return Result<Guid>.Failure("Permission không tồn tại.");
            }

            // Load Role (EF tự load owned collection PermissionIds)
            Role? role = await unitOfWork.Repository<Role>().Entities
                .FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
            if (role is null)
            {
                return Result<Guid>.Failure("Role không tìm thấy.");
            }

            // Một role có thể có nhiều permission — kiểm tra tránh gán trùng
            if (role.PermissionIds.Any(pid => pid.Value == permissionId))
            {
                return Result<Guid>.Failure("Permission này đã được gán cho Role.");
            }

            role.AddPermissionId(new Role.PermissionId(permissionId));
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(roleId);
        }
    }






}
