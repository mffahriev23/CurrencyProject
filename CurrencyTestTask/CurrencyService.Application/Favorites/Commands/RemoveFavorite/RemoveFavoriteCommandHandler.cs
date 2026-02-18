using CurrencyService.Application.Repositories;
using CurrencyService.Application.UnitOfWork;
using CurrencyService.Domain.Entities;
using MediatR;

namespace CurrencyService.Application.Favorites.Commands.RemoveFavorite
{
    public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IFavoriteRepository _favoriteRepository;

        public RemoveFavoriteCommandHandler(
            IUnitOfWork unitOfWork,
            IFavoriteRepository favoriteRepository
        )
        {
            _unitOfWork = unitOfWork;
            _favoriteRepository = favoriteRepository;
        }

        public async Task Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            Favorite[] favorites = await _favoriteRepository.GetByUserId(
                request.UserId,
                cancellationToken
            );

            foreach (Guid currencyId in request.CurrencyIds)
            {
                Favorite? favorite = favorites.FirstOrDefault(x => x.CurrencyId == currencyId);

                if (favorite is null)
                {
                    continue;
                }

                favorite.Delete(DateTime.UtcNow); //TODO
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
