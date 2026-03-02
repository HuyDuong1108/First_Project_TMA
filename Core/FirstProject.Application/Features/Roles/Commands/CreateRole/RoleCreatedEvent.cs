using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Roles.Commands.CreateRole;

public class RoleCreatedEvent(Role role) : BaseEvent
{
    public Role Role { get; } = role;
}
