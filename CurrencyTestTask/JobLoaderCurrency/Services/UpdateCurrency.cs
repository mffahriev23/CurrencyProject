using CurrencyService.Domain.Entities;
using JobLoaderCurrency.Interfaces.CurrencyLoader.Dtos;
using JobLoaderCurrency.Interfaces.Repository;
using JobLoaderCurrency.Interfaces.Services;
using JobLoaderCurrency.Interfaces.UnitOfWork;

namespace JobLoaderCurrency.Services
{
    public class UpdateCurrency : IUpdateCurrency
    {
        readonly ICurrencyRepository _currencyRepository;
        readonly IUnitOfWork _unitOfWork;

        public UpdateCurrency(
            ICurrencyRepository currencyRepository,
            IUnitOfWork unitOfWork
        )
        {
            _currencyRepository = currencyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(ValuteDto[] actualData, CancellationToken cancellationToken)
        {
            Currency[] dbData = await _currencyRepository.GetAll(cancellationToken);

            if (dbData.Length == 0)
            {
                await Insert(
                    actualData,
                    cancellationToken
                );
            }
            else
            {
                await Update(
                    actualData,
                    dbData,
                    cancellationToken
                );
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private async Task Insert(
            ValuteDto[] actualData,
            CancellationToken cancellationToken
        )
        {
            Currency[] dbData = actualData
                .Select(x => new Currency(x.Name!, x.VunitRate))
                .ToArray();

            _currencyRepository.AddRange(dbData);
        }

        private async Task Update(
            ValuteDto[] actualData,
            Currency[] dbData,
            CancellationToken cancellationToken
        )
        {
            foreach (Currency currency in dbData)
            {
                ValuteDto? actualDataConcrete = actualData.FirstOrDefault(x => x.Name.Equals(
                        currency.Name,
                        StringComparison.CurrentCultureIgnoreCase
                    )
                );

                if (actualDataConcrete is null)
                {
                    continue;
                }

                currency.SetRate(actualDataConcrete.VunitRate);
            }
        }
    }
}
