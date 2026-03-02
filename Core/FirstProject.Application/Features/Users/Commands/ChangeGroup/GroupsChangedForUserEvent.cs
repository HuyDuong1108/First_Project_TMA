using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Users.Commands.ChangeGroup;

public class GroupsChangedForUserEvent(User user) : BaseEvent
{
    public User User { get; } = user;
}
