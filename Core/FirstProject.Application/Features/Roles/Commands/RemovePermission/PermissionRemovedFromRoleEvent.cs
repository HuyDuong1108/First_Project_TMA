using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Roles.Commands.RemovePermission;

public class PermissionRemovedFromRoleEvent(Role role, Permission permission) : BaseEvent
{
    public Role Role { get; } = role;
    public Permission Permission { get; } = permission;
}
