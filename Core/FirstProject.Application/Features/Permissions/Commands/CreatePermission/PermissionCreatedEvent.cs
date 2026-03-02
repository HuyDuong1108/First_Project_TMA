using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;
namespace FirstProject.Application.Features.Permissions.Commands.CreatePermission;

public class PermissionCreatedEvent(Permission permission) : BaseEvent
{
    public Permission Permission { get; } = permission;
}