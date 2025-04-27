using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ExchangeRateController(IExtendedCurrencyExchangeService<ExchangeRate> exchangeRate) : ControllerBase
    {
        private readonly ICurrencyExchangeService<ExchangeRate> _exchangeRate = exchangeRate;

        [HttpGet]
        public async Task<ActionResult<List<ExchangeRatesResponse>>> GetRates()
        {

            var currencies = await _exchangeRate.GetAll();

            var response = currencies.Select(b => new ExchangeRatesResponse(b.Id, b.BaseCurrency, b.TargetCurrency, b.Rate));

            return Ok(response);
        }
    }
}
