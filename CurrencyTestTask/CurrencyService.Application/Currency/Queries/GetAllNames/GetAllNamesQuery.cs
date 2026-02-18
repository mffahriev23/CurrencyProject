using MediatR;

namespace CurrencyService.Application.Currency.Queries.GetAllNames
{
    public class GetAllNamesQuery : IRequest<NameItem[]>;
}
