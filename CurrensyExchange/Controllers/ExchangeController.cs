using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ExchangeController(ICurrencyExchangeService exchange, ICurrencyExchangeRepository<Currency> currency) : ControllerBase
    {
        private readonly ICurrencyExchangeService _exchangeRate = exchange;
        private readonly ICurrencyExchangeRepository<Currency> _currency = currency;

        [HttpGet("from={baseCurrencyCode}&to={targetCurrencyCode}&amount={amount}")]
        public async Task<ActionResult<ExchangeDTO>> GetConverted(string baseCurrencyCode, string targetCurrencyCode, float amount)
        {
            var baseCurrency = await _currency.Get(baseCurrencyCode);
            var targetCurrency = await _currency.Get(targetCurrencyCode);

            await exchange.Recalculate(baseCurrency, targetCurrency, amount);

            var response = new ExchangeDTO(exchange.BaseCurrency, exchange.TargetCurrency, exchange.ExchangeRate.Rate, amount, exchange.RecalculateAmount);

            return Ok(response);
        }
    }
}
