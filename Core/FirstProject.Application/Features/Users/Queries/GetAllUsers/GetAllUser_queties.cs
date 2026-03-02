using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace FirstProject.Application.Features.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery : IRequest<Result<IReadOnlyList<User>>>, IMapFrom<User>
    {
    }

    internal class GetAllUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUsersQuery, Result<IReadOnlyList<User>>>
    {
        public async Task<Result<IReadOnlyList<User>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            IReadOnlyList<User> users = await unitOfWork.Repository<User>().Entities
                .ToListAsync(cancellationToken);
            return Result<IReadOnlyList<User>>.Success(users);
        }
    }
}
