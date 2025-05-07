using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ExchangeController(IExtendedCurrencyExchangeService<ExchangeRate> exchangeRate) : ControllerBase
    {
        private readonly IExtendedCurrencyExchangeService<ExchangeRate> _exchangeRate = exchangeRate;

        [HttpGet("from={baseCurrencyCode}&to={targetCurrencyCode}&amount={amount}")]
        public async Task<ActionResult<ExchangeResponse>> GetConverted(string baseCurrencyCode, string targetCurrencyCode, float amount)
        {
            var exchange = await _exchangeRate.GetAny(baseCurrencyCode, targetCurrencyCode);

            var convertedAmount = _exchangeRate.Convert(exchange.Rate, amount);

            var response = new ExchangeResponse(exchange.BaseCurrency, exchange.TargetCurrency, exchange.Rate, amount, convertedAmount);

            return Ok(response);
        }
    }
}
