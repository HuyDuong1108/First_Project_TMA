using FirstProject.Domain.Common;
using FirstProject.Domain.Entities;
namespace FirstProject.Application.Features.Users.Commands.CreateUser;

public class UserCreatedEvent(User user) : BaseEvent
{
    public User User { get; } = user;
}