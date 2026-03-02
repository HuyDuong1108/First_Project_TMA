using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Users.Commands.RemoveRole
{
    public record RemoveRoleFromUserCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public required Guid? RoleId { get; init; }
        public required Guid? UserId { get; init; }
    }

    internal class RemoveRoleFromUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveRoleFromUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RemoveRoleFromUserCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId is not { } userId)
            {
                return Result<Guid>.Failure("Cần nhập UserId.");
            }
            if (command.RoleId is not { } roleId)
            {
                return Result<Guid>.Failure("Cần nhập RoleId.");
            }

            User? user = await unitOfWork.Repository<User>().Entities
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            if (user is null)
            {
                return Result<Guid>.Failure("User không tìm thấy.");
            }

            if (!user.RoleIds.Any(rid => rid.Value == roleId))
            {
                return Result<Guid>.Failure("User không có Role này.");
            }

            user.RemoveRoleId(new RoleId(roleId));
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(userId);
        }
    }
}
