//File này là base class cho các event của entity trong application 
// Nó chỉ có 1 trường đó la thời gian xảy ra event
namespace FirstProject.Domain.Common
{
    public abstract class BaseEvent : MediatR.INotification
    {
        public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
    }
}