using CurrencyService.Application.Currency.Queries.GetAllNames;
using Grpc.Core;
using MediatR;

namespace CurrencyService.gRPC.Services
{
    public class CurrenncyServer : CurrencyService.CurrencyServiceBase
    {
        readonly ISender _sender;

        public CurrenncyServer(ISender sender)
        {
            _sender = sender;
        }

        public async Task<GetAllReply> GetAll(
            Empty empty,
            ServerCallContext context,
            CancellationToken cancellationToken
        )
        {
            NameItem[] result = await _sender.Send(
                new GetAllNamesQuery(),
                cancellationToken
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
