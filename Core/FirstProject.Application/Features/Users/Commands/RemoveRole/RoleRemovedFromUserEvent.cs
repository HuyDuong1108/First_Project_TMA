using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Users.Commands.RemoveRole;

public class RoleRemovedFromUserEvent(User user, Role role) : BaseEvent
{
    public User User { get; } = user;
    public Role Role { get; } = role;
}
