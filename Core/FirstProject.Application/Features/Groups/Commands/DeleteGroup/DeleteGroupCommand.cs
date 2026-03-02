using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Groups.Commands.DeleteGroup
{
    public record DeleteGroupCommand : IRequest<Result<Guid>>, IMapFrom<Group>
    {
        public Guid? GroupId { get; init; }
    }

    internal class DeleteGroupCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteGroupCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(DeleteGroupCommand command, CancellationToken cancellationToken)
        {
            if (command.GroupId is not { } groupId)
            {
                return Result<Guid>.Failure("Cần nhập Id.");
            }

            Group? group = await unitOfWork.Repository<Group>().Entities
                .FirstOrDefaultAsync(g => g.GroupId == groupId, cancellationToken);
            if (group is null)
            {
                return Result<Guid>.Failure("Group không tìm thấy");
            }

            await unitOfWork.Repository<Group>().DeleteAsync(group);
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(groupId);
        }
    }
}
