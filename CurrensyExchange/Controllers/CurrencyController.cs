using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CurrencyController(ICurrencyExchangeService<Currency> currencyService) : ControllerBase
    {
        private readonly ICurrencyExchangeService<Currency> _currencyService = currencyService;

        [HttpGet]
        public async Task<ActionResult<List<CurrenciesResponse>>> GetCurrensies()
        {
            var currencies = await _currencyService.GetAll();

            var response = currencies.Select(b => new CurrenciesResponse(b.Id, b.Code, b.FullName, b.Sign));

            return Ok(response);
        }
    }
}
