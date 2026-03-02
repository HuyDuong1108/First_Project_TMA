using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;    
namespace FirstProject.Application.Features.Users.Queries.GetUsersByRole
{
    public record GetUsersByRoleQuery : IRequest<Result<IReadOnlyList<User>>>, IMapFrom<User>
    {
        public required string RoleName { get; init; }
        
    }

    internal class GetUserByRolesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUsersByRoleQuery, Result<IReadOnlyList<User>>>
    {
        public async Task<Result<IReadOnlyList<User>>> Handle(GetUsersByRoleQuery query, CancellationToken cancellationToken)
        {
            Role? role = await unitOfWork.Repository<Role>().Entities
                .FirstOrDefaultAsync(r => r.RoleName == query.RoleName, cancellationToken);
            if (role is null)
            {
                return Result<IReadOnlyList<User>>.Failure("Role không tìm thấy");
            }
            IReadOnlyList<User> users = await unitOfWork.Repository<User>().Entities
                .Where(u => u.RoleIds.Any(r => r.Value == role.RoleId))
                .ToListAsync(cancellationToken);
            return Result<IReadOnlyList<User>>.Success(users);
        }
    }
}