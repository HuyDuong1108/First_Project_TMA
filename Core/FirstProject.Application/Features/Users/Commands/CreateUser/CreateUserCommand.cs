using AutoMapper;
using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Auth;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
namespace FirstProject.Application.Features.Users.Commands.CreateUser
{
    public record CreateUserCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string FullName { get; init; }
        public required string Status { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    }

    internal class CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            User user = new()
            {
                Email = command.Email,
                Password = _passwordHasher.Hash(command.Password),
                FullName = command.FullName,
                Status = command.Status == "Active",
                CreatedAt = command.CreatedAt,

            };
            _ = await _unitOfWork.Repository<User>().AddAsync(user);
            user.AddDomainEvent(new UserCreatedEvent(user));
            _ = await _unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(user.UserId);
        }
    }
}