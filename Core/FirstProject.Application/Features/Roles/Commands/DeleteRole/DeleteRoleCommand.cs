using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Roles.Commands.DeleteRole
{
    public record DeleteRoleCommand : IRequest<Result<Guid>>, IMapFrom<Role>
    {
        public Guid? RoleId { get; init; }
    }

    internal class DeleteRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteRoleCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.RoleId is not { } roleId)
            {
                return Result<Guid>.Failure("Cần nhập RoleId.");
            }

            Role? role = await unitOfWork.Repository<Role>().Entities
                .FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
            if (role is null)
            {
                return Result<Guid>.Failure("Role không tìm thấy");
            }

            await unitOfWork.Repository<Role>().DeleteAsync(role);
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(roleId);
        }
    }
}
