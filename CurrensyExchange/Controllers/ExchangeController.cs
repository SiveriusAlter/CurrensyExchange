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

        [HttpGet("from={baseCurrencyID}&to={targetCurrencyID}&amount={amount}")]
        public async Task<ActionResult<ExchangeResponse>> GetConverted(int baseCurrencyID, int targetCurrencyID, float amount)
        {
            var rate = await _exchangeRate.Get(baseCurrencyID, targetCurrencyID);
            var convertedAmount = _exchangeRate.Convert(rate, amount);

            var response = new ExchangeResponse(rate.BaseCurrency, rate.TargetCurrency, rate.Rate, amount, convertedAmount);

            return Ok(response);
        }
    }
}
