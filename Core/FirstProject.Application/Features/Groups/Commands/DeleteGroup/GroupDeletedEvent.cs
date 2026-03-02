using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;
namespace FirstProject.Application.Features.Groups.Commands.DeleteGroup
{
    public class GroupDeletedEvent(Group group) : BaseEvent
    {
        public Group Group { get; } = group;
    }
}
