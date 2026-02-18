using MediatR;

namespace CurrencyService.Application.Favorites.Commands.RemoveFavorite
{
    public record RemoveFavoriteCommand(Guid[] CurrencyIds, Guid UserId) : IRequest;
}
