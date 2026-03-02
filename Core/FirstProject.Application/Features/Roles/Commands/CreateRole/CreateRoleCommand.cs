using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;

namespace FirstProject.Application.Features.Roles.Commands.CreateRole
{
    public record CreateRoleCommand : IRequest<Result<Guid>>, IMapFrom<Role>
    {
        public required string RoleName { get; init; }
    }

    internal class CreateRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRoleCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Guid>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            Role role = new()
            {
                RoleName = command.RoleName
            };
            _ = await _unitOfWork.Repository<Role>().AddAsync(role);
            role.AddDomainEvent(new RoleCreatedEvent(role));
            _ = await _unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(role.RoleId);
        }
    }
}
