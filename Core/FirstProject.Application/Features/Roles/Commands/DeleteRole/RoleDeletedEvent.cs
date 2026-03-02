using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Roles.Commands.DeleteRole;

public class RoleDeletedEvent(Role role) : BaseEvent
{
    public Role Role { get; } = role;
}
