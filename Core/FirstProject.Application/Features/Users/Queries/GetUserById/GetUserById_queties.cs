using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;    
namespace FirstProject.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery : IRequest<Result<User>>, IMapFrom<User>
    {
        public Guid UserId { get; init; }
    }

    internal class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, Result<User>>
    {
        public async Task<Result<User>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            User? user = await unitOfWork.Repository<User>().Entities
                .FirstOrDefaultAsync(u => u.UserId == query.UserId, cancellationToken);
            if (user is null)
            {
                return Result<User>.Failure("User không tìm thấy");
            }

            return Result<User>.Success(user);
        }
    }
}
