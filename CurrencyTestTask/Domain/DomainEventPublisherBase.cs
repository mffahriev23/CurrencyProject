
namespace Domain
{
    public abstract class DomainEventPublisherBase : IDomainEventPublisher
    {
        private readonly List<IDomainEvent> _events;

        public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

        public DomainEventPublisherBase()
        {
            _events = new List<IDomainEvent>();
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public void AddEvent(IDomainEvent @event)
        {
            _events.Add(@event);
        }

        public void AddEventsRange(IDomainEvent[] events)
        {
            _events?.AddRange(events);
        }
    }
}
