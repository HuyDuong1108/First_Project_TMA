using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public Guid? UserID { get; init; }
        public required string Email { get; init; }
        public required string Password { get; init; }
        public required string FullName { get; init; }
        public required string Status { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    }

    internal class UpdateUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserCommand, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
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

            user.Email = command.Email;
            user.Password = command.Password;
            user.FullName = command.FullName;
            user.Status = command.Status == "Active";

            // User đã được load từ DbSet nên đang tracked; chỉ cần Save, không gọi UpdateAsync (vì User dùng PK UserId/Guid, GenericRepository.UpdateAsync dùng entity.Id/int).
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(userId);
        }
    }
}