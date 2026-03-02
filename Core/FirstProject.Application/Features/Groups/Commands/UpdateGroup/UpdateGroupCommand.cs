using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using MediatR;
using FirstProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Groups.Commands.UpdateGroup
{
    public record UpdateGroupCommand : IRequest<Result<Guid>>, IMapFrom<Group>
    {
        public Guid? GroupId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
    }

    internal class UpdateGroupCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateGroupCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdateGroupCommand command, CancellationToken cancellationToken)
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

            group.Name = command.Name;
            group.Description = command.Description;

            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(groupId);
        }
    }
}
