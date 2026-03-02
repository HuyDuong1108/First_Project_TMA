using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Roles.Commands.UpdateRole;

public class RoleUpdatedEvent(Role role) : BaseEvent
{
    public Role Role { get; } = role;
}
