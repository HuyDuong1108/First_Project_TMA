using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;
namespace FirstProject.Application.Features.Groups.Commands.CreateGroup;

public class GroupCreatedEvent(Group group) : BaseEvent
{
    public Group Group { get; } = group;
}
