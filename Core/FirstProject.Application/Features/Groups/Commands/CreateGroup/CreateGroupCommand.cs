using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using MediatR;
using FirstProject.Domain.Entities;
namespace FirstProject.Application.Features.Groups.Commands.CreateGroup
{
    public record CreateGroupCommand : IRequest<Result<int>>, IMapFrom<Group>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
    }

    internal class CreateGroupCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateGroupCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<int>> Handle(CreateGroupCommand command, CancellationToken cancellationToken)
        {
            Group group = new()
            {
                Name = command.Name,
                Description = command.Description
            };
            _ = await _unitOfWork.Repository<Group>().AddAsync(group);
            group.AddDomainEvent(new GroupCreatedEvent(group));
            _ = await _unitOfWork.Save(cancellationToken);
            return Result<int>.Success(group.Id);
        }
    }
}
