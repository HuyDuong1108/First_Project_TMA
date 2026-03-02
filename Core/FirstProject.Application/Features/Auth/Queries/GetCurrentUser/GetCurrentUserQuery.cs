using FirstProject.Application.Common;
using MediatR;

namespace FirstProject.Application.Features.Auth.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<Result<CurrentUserDto>>;

public record CurrentUserDto(Guid UserId, string Email, string FullName, bool Status);
