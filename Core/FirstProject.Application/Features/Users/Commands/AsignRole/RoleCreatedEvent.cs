using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Users.Commands.AsignRole;

public class AsignRoleCreatedEvent(Role role, User user) : BaseEvent
{
    public Role Role { get; } = role;
    public User User { get; } = user;
}
