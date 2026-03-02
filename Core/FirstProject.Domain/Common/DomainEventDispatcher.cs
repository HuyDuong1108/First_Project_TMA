using FirstProject.Domain.Common.Interfaces;
using MediatR;

namespace FirstProject.Domain.Common
{
    public class DomainEventDispatcher(IPublisher publisher) : IDomainEventDispatcher
    {
        private readonly IPublisher _publisher = publisher;

        public async Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents)
        {
            foreach (BaseEntity entity in entitiesWithEvents)
            {
                foreach (BaseEvent domainEvent in entity.DomainEvents)
                {
                    await _publisher.Publish(domainEvent).ConfigureAwait(false);
                }
                entity.ClearDomainEvents();
            }
        }
    }
}
