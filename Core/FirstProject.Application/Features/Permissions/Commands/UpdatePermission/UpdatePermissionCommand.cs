using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Permissions.Commands.UpdatePermission
{
    public record UpdatePermissionCommand : IRequest<Result<Guid>>, IMapFrom<Permission>
    {
        public Guid? PermissionId { get; init; }
        public required string Code { get; init; }
        public required string Description { get; init; }
    }

    internal class UpdatePermissionCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePermissionCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdatePermissionCommand command, CancellationToken cancellationToken)
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

                permission.Code = command.Code;
            permission.Description = command.Description;

            // Permission đã được load từ DbSet nên đang tracked; chỉ cần Save, không gọi UpdateAsync (vì Permission dùng PK PermissionId/Guid, GenericRepository.UpdateAsync dùng entity.Id/int).
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(permissionId);
        }
    }
}