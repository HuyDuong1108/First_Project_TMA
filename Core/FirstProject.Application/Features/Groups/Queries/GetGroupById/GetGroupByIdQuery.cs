using FirstProject.Application.Common;
using FirstProject.Application.Common.Interfaces;
using FirstProject.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FirstProject.Domain.Entities;
namespace FirstProject.Application.Features.Groups.Queries.GetGroupById
{
    public record GetGroupByIdQuery : IRequest<Result<Guid>>, IMapFrom<Group>
    {
        public Guid? GroupId { get; init; }
    }

    internal class GetGroupByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetGroupByIdQuery, Result<Guid>>
    {
        public async Task<Result<Guid>> Handle(GetGroupByIdQuery query, CancellationToken cancellationToken)
        {
            if (query.GroupId is not { } groupId)
            {
                return Result<Guid>.Failure("Cần nhập Id.");
            }

            Group? group = await unitOfWork.Repository<Group>().Entities
                .FirstOrDefaultAsync(g => g.GroupId == groupId, cancellationToken);
            if (group is null)
            {
                return Result<Guid>.Failure("Group không tìm thấy");
            }

            return Result<Guid>.Success(group.GroupId);
        }
    }
}
