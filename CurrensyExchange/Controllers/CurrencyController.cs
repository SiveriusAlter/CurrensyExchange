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
        public async Task<ActionResult<List<CurrenciesResponse>>> GetCurrencies()
        {
            var currencies = await _currencyService.GetAll();

            var response = currencies.Select(b => new CurrenciesResponse(b.Id, b.Code, b.FullName, b.Sign));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<CurrenciesResponse>> InsertCurrency(Currency currency)
        {
            var response = await _currencyService.Insert(currency);
            return Ok(response);
        }
    }
}
