using Application.Extensions;
using CurrencyService.Application.Favorites.Commands.AddFavorite;
using CurrencyService.Application.Favorites.Commands.RemoveFavorite;
using CurrencyService.Application.Favorites.Queries.GetFavorites;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CurrencyService.gRPC.Services
{
    [Authorize]
    public class FavoriteServer : FavoriteService.FavoriteServiceBase
    {
        readonly ISender _sender;

        public FavoriteServer(ISender sender)
        {
            _sender = sender;
        }

        public override async Task<Empty> AddFaforite(
            FaforiteCommadRequest request,
            ServerCallContext context
        )
        {
            Guid userId = context.GetHttpContext().User.Claims.GetUserId();

            AddFavoriteCommand command = new(
                request.Body.CurrencyIds.Select(x => new Guid(x)).ToArray(),
                userId
            );

            await _sender.Send(command, context.CancellationToken);

            return new Empty();
        }

        public override async Task<Empty> DeleteFavorite(
            FaforiteCommadRequest request,
            ServerCallContext context
        )
        {
            Guid userId = context.GetHttpContext().User.Claims.GetUserId();

            RemoveFavoriteCommand command = new(
                request.Body.CurrencyIds.Select(x => new Guid(x)).ToArray(),
                userId
            );

            await _sender.Send(command, context.CancellationToken);

            return new Empty();
        }

        public override async Task<GetFavoritesResponse> GetFavorites(
            Empty request,
            ServerCallContext context
        )
        {
            Guid userId = context.GetHttpContext().User.Claims.GetUserId();

            GetFavoritesQuery query = new(userId);

            FavoriteItem[] items = await _sender.Send(query, context.CancellationToken);

            GetFavoritesResponse response = new();

            response.Items.AddRange(items.Select(x => new FavoriteDataItem
                {
                    Id = x.Id.ToString(),
                    CurrencyId = x.CurrencyId.ToString(),
                    Name = x.Name,
                    Rate = x.Rate.ToString()
                }).ToArray()
            );

            return response;
        }
    }
}
