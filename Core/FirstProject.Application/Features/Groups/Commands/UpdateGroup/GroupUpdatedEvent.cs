using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;
namespace FirstProject.Application.Features.Groups.Commands.UpdateGroup;

public class GroupUpdatedEvent(Group group) : BaseEvent
{
    public Group Group { get; } = group;
}
