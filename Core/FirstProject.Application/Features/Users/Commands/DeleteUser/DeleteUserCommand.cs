using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace FirstProject.Application.Features.Users.Commands.DeleteUser
{
    public record DeleteUserCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public Guid? UserID { get; init; }
    }

    internal class DeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            if (command.UserID is not { } userId)
            {
                return Result<Guid>.Failure("Cần nhập UserID.");
            }

            User? user = await unitOfWork.Repository<User>().Entities
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            if (user is null)
            {
                return Result<Guid>.Failure("User không tìm thấy");
            }

            await unitOfWork.Repository<User>().DeleteAsync(user);
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(userId);
        }
    }
}
