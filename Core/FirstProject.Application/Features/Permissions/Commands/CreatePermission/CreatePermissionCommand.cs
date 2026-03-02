using AutoMapper;
using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Auth;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
namespace FirstProject.Application.Features.Permissions.Commands.CreatePermission
{
    public record CreatePermissionCommand : IRequest<Result<Guid>>, IMapFrom<Permission>
    {
        public required string Code { get; init; }
        public required string Description { get; init; }

    }

    internal class CreatePermissionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreatePermissionCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<Guid>> Handle(CreatePermissionCommand command, CancellationToken cancellationToken)
        {
            Permission permission = new()
            {
                Code = command.Code,
                Description = command.Description,
            };
            _ = await _unitOfWork.Repository<Permission>().AddAsync(permission);
            permission.AddDomainEvent(new PermissionCreatedEvent(permission));
        _ = await _unitOfWork.Save(cancellationToken);
        return Result<Guid>.Success(permission.PermissionId);
    }
}
}