using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstrations;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ExchangeRateController(ICurrencyExchangeService<ExchangeRate> exchangeRate) : ControllerBase
    {
        private readonly ICurrencyExchangeService<ExchangeRate> _exchangeRate = exchangeRate;

        [HttpGet]
        public async Task<ActionResult<List<ExchangeRateResponse>>> GetCurrensies()
        {
            var currencies = await _exchangeRate.GetAll();

            var response = currencies.Select(b => new ExchangeRateResponse(b.Id, b.BaseCurrency, b.TargetCurrency, b.Rate));

            return Ok(response);
        }
    }
}
