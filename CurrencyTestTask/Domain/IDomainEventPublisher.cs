namespace Domain
{
    public interface IDomainEventPublisher
    {
        public IReadOnlyCollection<IDomainEvent> Events { get; }

        public void ClearEvents();

        public void AddEvent(IDomainEvent @event);

        public void AddEventsRange(IDomainEvent[] events);
    }
}
