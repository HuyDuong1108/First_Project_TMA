using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Roles.Commands.AsignPermission;

public class AsignPermissionCreatedEvent(Role role, Permission permission) : BaseEvent
{
    public Role Role { get; } = role;
    public Permission Permission { get; } = permission;
}
