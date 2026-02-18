using MediatR;

namespace CurrencyService.Application.Favorites.Commands.AddFavorite
{
    public record AddFavoriteCommand(Guid[] CurrencyIds, Guid UserId) : IRequest;
}
