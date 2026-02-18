using CurrencyService.Application.Repositories;
using MediatR;

namespace CurrencyService.Application.Currency.Queries.GetAllNames
{
    public class GetAllNamesQueryHandler : IRequestHandler<GetAllNamesQuery, NameItem[]>
    {
        readonly ICurrencyRepository _currencyRepository;

        public GetAllNamesQueryHandler(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<NameItem[]> Handle(
            GetAllNamesQuery request,
            CancellationToken cancellationToken
        )
        {
            (Guid id, string name)[] data = await _currencyRepository.GetAllNames(
                cancellationToken
            );

            return data.Select(x => new NameItem(x.id, x.name))
                .ToArray();
        }
    }
}
