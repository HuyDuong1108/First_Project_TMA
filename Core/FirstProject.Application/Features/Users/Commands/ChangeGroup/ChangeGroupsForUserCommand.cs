using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Users.Commands.ChangeGroup
{
    public record ChangeGroupsForUserCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public required Guid? UserId { get; init; }
        public required IReadOnlyList<Guid> GroupIds { get; init; }
    }

    internal class ChangeGroupsForUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeGroupsForUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(ChangeGroupsForUserCommand command, CancellationToken cancellationToken)
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

            if (command.GroupIds is not { Count: > 0 } groupIds)
            {
                foreach (var gid in user.GroupIds.ToList())
                {
                    user.RemoveGroupId(gid);
                }
                _ = await unitOfWork.Save(cancellationToken);
                return Result<Guid>.Success(userId);
            }

            bool allGroupsExist = await unitOfWork.Repository<Group>().Entities
                .Where(g => groupIds.Contains(g.GroupId))
                .CountAsync(cancellationToken) == groupIds.Count;
            if (!allGroupsExist)
            {
                return Result<Guid>.Failure("Một hoặc nhiều GroupId không tồn tại.");
            }

            foreach (var gid in user.GroupIds.ToList())
            {
                user.RemoveGroupId(gid);
            }
            foreach (Guid groupId in groupIds.Distinct())
            {
                user.AddGroupId(new GroupId(groupId));
            }

            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(userId);
        }
    }
}
