using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CurrentService.Infrastructure.DAL.Interceptors
{
    public class DomainEventInterceptor : ISaveChangesInterceptor
    {
        private readonly IMediator _mediator;

        public DomainEventInterceptor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default
        )
        {
            IDomainEventPublisher[] entitiesWithEvents = eventData.Context!.ChangeTracker.Entries<IDomainEventPublisher>()
                .Where(e => e.Entity.Events.Any())
                .Select(e => e.Entity)
                .ToArray();

            foreach (IDomainEventPublisher entity in entitiesWithEvents)
            {
                foreach (IDomainEvent domainEvent in entity.Events)
                {
                    await _mediator.Publish(domainEvent);
                }

                entity.ClearEvents();
            }

            return result;
        }
    }
}
