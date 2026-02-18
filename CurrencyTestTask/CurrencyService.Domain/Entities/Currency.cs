namespace CurrencyService.Domain.Entities
{
    public class Currency
    {
        private Guid _id;
        private string _name;
        private decimal _rate;

        public Guid Id => _id;

        public string Name => _name;

        public decimal Rate => _rate;

        public Currency(
            string name,
            decimal rate
        )
        {
            _name = name;
            _rate = rate;
            _id = Guid.NewGuid();
        }

        public void SetRate(decimal rate)
        {
            _rate = rate;
        }
    }
}
