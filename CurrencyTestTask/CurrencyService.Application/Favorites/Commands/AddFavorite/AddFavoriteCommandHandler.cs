using Application.UnitOfWork;
using CurrencyService.Application.Repositories;
using CurrencyService.Domain.Entities;
using MediatR;

namespace CurrencyService.Application.Favorites.Commands.AddFavorite
{
    public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IFavoriteRepository _favoriteRepository;

        public AddFavoriteCommandHandler(
            IUnitOfWork unitOfWork,
            IFavoriteRepository favoriteRepository
        )
        {
            _unitOfWork = unitOfWork;
            _favoriteRepository = favoriteRepository;
        }

        public async Task Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            Favorite[] favorites = request.CurrencyIds
                .Select(x => Favorite.Create(DateTime.UtcNow, request.UserId, x))
                .ToArray(); // TODO убрать UtcNow

            _favoriteRepository.AddRange(favorites);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
