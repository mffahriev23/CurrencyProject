namespace CurrencyService.Application.Repositories
{
    public interface ICurrencyRepository
    {
        Task<(Guid id, string name)[]> GetAllNames(CancellationToken cancellationToken);
    }
}
