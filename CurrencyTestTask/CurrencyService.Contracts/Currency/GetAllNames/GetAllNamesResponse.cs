namespace CurrencyService.Contracts.Currency.GetAllNames
{
    public record GetAllNamesResponse
    {
        public NameDataItem[] Items { get; init; }

        public GetAllNamesResponse()
        {
            Items = Array.Empty<NameDataItem>();
        }
    }
}
