using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Users.Commands.ChangeRole;

public class RolesChangedForUserEvent(User user) : BaseEvent
{
    public User User { get; } = user;
}
