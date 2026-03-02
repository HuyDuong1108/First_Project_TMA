using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Roles.Commands.ChangePermission;

public class PermissionsChangedForRoleEvent(Role role) : BaseEvent
{
    public Role Role { get; } = role;
}
