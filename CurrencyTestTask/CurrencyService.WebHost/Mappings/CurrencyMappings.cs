using CurrencyService.Application.Currency.Queries.GetAllNames;
using CurrencyService.Contracts.Currency;
using CurrencyService.Contracts.Currency.GetAllNames;

namespace CurrencyService.WebHost.Mappings
{
    public static class CurrencyMappings
    {
        public static GetAllNamesResponse Map(this NameItem[] items)
        {
            return new()
            {
                Items = items
                    .Select(x => new NameDataItem { Name = x.Name, Id = x.Id })
                    .ToArray()
            };
        }
    }
}
