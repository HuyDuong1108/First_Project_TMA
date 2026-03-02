using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;

namespace FirstProject.Application.Features.Users.Commands.AsignGroup;

public class AsignGroupCreatedEvent(Group group, User user) : BaseEvent
{
    public Group Group { get; } = group;
    public User User { get; } = user;
}
