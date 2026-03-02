using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace FirstProject.Application.Features.Permissions.Commands.DeletePermission
{
    public record DeletePermissionCommand : IRequest<Result<Guid>>, IMapFrom<Permission>
    {
        public Guid? PermissionId { get; init; }
    }

    internal class DeletePermissionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeletePermissionCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(DeletePermissionCommand command, CancellationToken cancellationToken)
        {
            if (command.PermissionId is not { } permissionId)
            {
                return Result<Guid>.Failure("Cần nhập PermissionId.");
            }

            Permission? permission = await unitOfWork.Repository<Permission>().Entities
                .FirstOrDefaultAsync(p => p.PermissionId == permissionId, cancellationToken);
            if (permission is null)
            {
                return Result<Guid>.Failure("Permission không tìm thấy");
            }

            await unitOfWork.Repository<Permission>().DeleteAsync(permission);
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(permissionId);
        }
    }
}
