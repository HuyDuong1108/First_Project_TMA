using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Users.Commands.ChangeRole
{
    public record ChangeRolesForUserCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public required Guid? UserId { get; init; }
        public required IReadOnlyList<Guid> RoleIds { get; init; }
    }

    internal class ChangeRolesForUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeRolesForUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(ChangeRolesForUserCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId is not { } userId)
            {
                return Result<Guid>.Failure("Cần nhập UserId.");
            }

            User? user = await unitOfWork.Repository<User>().Entities
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            if (user is null)
            {
                return Result<Guid>.Failure("User không tìm thấy.");
            }

            if (command.RoleIds is not { Count: > 0 } roleIds)
            {
                user.ClearRoleIds();
                _ = await unitOfWork.Save(cancellationToken);
                return Result<Guid>.Success(userId);
            }

            bool allRolesExist = await unitOfWork.Repository<Role>().Entities
                .Where(r => roleIds.Contains(r.RoleId))
                .CountAsync(cancellationToken) == roleIds.Count;
            if (!allRolesExist)
            {
                return Result<Guid>.Failure("Một hoặc nhiều RoleId không tồn tại.");
            }

            user.ClearRoleIds();
            foreach (Guid roleId in roleIds.Distinct())
            {
                user.AddRoleId(new RoleId(roleId));
            }

            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(userId);
        }
    }
}
