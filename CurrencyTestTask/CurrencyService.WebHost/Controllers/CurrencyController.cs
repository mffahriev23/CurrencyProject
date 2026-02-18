using CurrencyService.Application.Currency.Queries.GetAllNames;
using CurrencyService.Contracts.Currency.GetAllNames;
using CurrencyService.WebHost.Mappings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.WebHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        readonly ISender _sender;

        public CurrencyController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("get-all")]
        [Authorize]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            NameItem[] result = await _sender.Send(new GetAllNamesQuery(), cancellationToken);

            GetAllNamesResponse response = result.Map();

            return Ok(response);
        }
    }
}
