namespace Domain
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        /// <summary>
        /// Временная метка происхождения события
        /// </summary>
        private readonly DateTime _timestamp;

        public DateTime Timestamp => _timestamp;

        protected BaseDomainEvent(DateTime timestamp)
        {
            _timestamp = timestamp;
        }
    }
}
