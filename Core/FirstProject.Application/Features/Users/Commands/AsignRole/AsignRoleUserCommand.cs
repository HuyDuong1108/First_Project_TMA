using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Users.Commands.AsignRole
{
    public record AsignRoleCommand : IRequest<Result<Guid>>, IMapFrom<User>
    {
        public required Guid? RoleId { get; init; }
        public required Guid? UserId { get; init; }
    }

    internal class AsignRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AsignRoleCommand, Result<Guid>>
    {

        public async Task<Result<Guid>> Handle(AsignRoleCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId is not { } userId)
            {
                return Result<Guid>.Failure("Cần nhập UserId.");
            }
            if (command.RoleId is not { } roleId)
            {
                return Result<Guid>.Failure("Cần nhập RoleId.");
            }
            User? user = await unitOfWork.Repository<User>().Entities
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
            if (user is null)
            {
                return Result<Guid>.Failure("User không tìm thấy");
            }
            user.AddRoleId(new RoleId(roleId));
            _ = await unitOfWork.Save(cancellationToken);
            return Result<Guid>.Success(userId);
        }
    }






}
