using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Users.Commands.RemoveGroup
{
    public record RemoveGroupFromUserCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public required Guid? GroupId { get; init; }
        public required Guid? UserId { get; init; }
    }

    internal class RemoveGroupFromUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveGroupFromUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(RemoveGroupFromUserCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId is not { } userId)
            {
                return Result<Guid>.Failure("Cần nhập UserId.");
            }
            if (command.GroupId is not { } groupId)
            {
                return Result<Guid>.Failure("Cần nhập GroupId.");
            }

            User? user = await unitOfWork.Repository<User>().Entities
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            if (user is null)
            {
                return Result<Guid>.Failure("User không tìm thấy.");
            }

            if (!user.GroupIds.Any(gid => gid.Value == groupId))
            {
                return Result<Guid>.Failure("User không có Group này.");
            }

            user.RemoveGroupId(new GroupId(groupId));
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(userId);
        }
    }
}
