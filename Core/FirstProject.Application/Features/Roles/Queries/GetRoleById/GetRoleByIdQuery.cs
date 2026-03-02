using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using FirstProject.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FirstProject.Application.Features.Roles.Queries.GetRoleById
{
    public record GetRoleByIdQuery : IRequest<Result<Guid>>, IMapFrom<Role>
    {
        public Guid? RoleId { get; init; }
    }

    internal class GetRoleByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoleByIdQuery, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(GetRoleByIdQuery query, CancellationToken cancellationToken)
        {
            if (query.RoleId is not { } roleId)
            {
                return Result<Guid>.Failure("Cần nhập RoleId.");
            }

            Role? role = await unitOfWork.Repository<Role>().Entities
                .FirstOrDefaultAsync(r => r.RoleId == roleId, cancellationToken);
            if (role is null)
            {
                return Result<Guid>.Failure("Role không tìm thấy");
            }

            return Result<Guid>.Success(role.RoleId);
        }
    }
}
