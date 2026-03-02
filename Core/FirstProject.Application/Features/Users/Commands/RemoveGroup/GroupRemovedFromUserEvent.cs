using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Users.Commands.RemoveGroup;

public class GroupRemovedFromUserEvent(User user, Group group) : BaseEvent
{
    public User User { get; } = user;
    public Group Group { get; } = group;
}
