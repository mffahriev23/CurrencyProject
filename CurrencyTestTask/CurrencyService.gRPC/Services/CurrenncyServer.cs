using CurrencyService.Application.Currency.Queries.GetAllNames;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CurrencyService.gRPC.Services
{
    [Authorize]
    public class CurrenncyServer : CurrencyService.CurrencyServiceBase
    {
        readonly ISender _sender;

        public CurrenncyServer(ISender sender)
        {
            _sender = sender;
        }

        public override async Task<GetAllReply> GetAll(
            Empty empty,
            ServerCallContext context
        )
        {
            NameItem[] result = await _sender.Send(
                new GetAllNamesQuery(),
                context.CancellationToken
            );

            NameDataItem[] items = result
                .Select(x => new NameDataItem
                {
                    Name = x.Name,
                    Id = x.Id.ToString()
                })
                .ToArray();

            GetAllReply response = new();

            response.Items.AddRange(items);

            return response;
        }
    }
}
