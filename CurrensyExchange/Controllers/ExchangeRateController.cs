using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ExchangeRateController(IExtendedCurrencyExchangeService<ExchangeRate> exchangeRateService, ICurrencyExchangeService<Currency> curencyService) : ControllerBase
    {
        private readonly IExtendedCurrencyExchangeService<ExchangeRate> _exchangeRateService = exchangeRateService;
        private readonly ICurrencyExchangeService<Currency> _curencyService = curencyService;

        [HttpGet]
        public async Task<ActionResult<List<ExchangeRatesResponse>>> GetRates()
        {

            var currencies = await _exchangeRateService.GetAll();

            var response = currencies.Select(b => new ExchangeRatesResponse(b.Id, b.BaseCurrency, b.TargetCurrency, b.Rate));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ExchangeRatesResponse>> InsertCurrency(AddExchangeRate addExchangeRate)
        {
            var baseCurrency = _curencyService.Get(addExchangeRate.BaseCurrencyCode).Result;
            var targetCurrency = _curencyService.Get(addExchangeRate.TargetCurrencyCode).Result;
            var exchangeRate = new ExchangeRate(0, baseCurrency.Id, targetCurrency.Id, baseCurrency, targetCurrency, addExchangeRate.Rate);
            var response = await _exchangeRateService.Insert(exchangeRate);
            return Ok(response);
        }
        [HttpPatch("{baseCurrencyCode}&{targetCurrencyCode}")]
        public async Task<ActionResult<ExchangeRatesResponse>> UpdateCurrency(string baseCurrencyCode, string targetCurrencyCode, UpdateRate updateRate)
        {
            var baseCurrency = _curencyService.Get(baseCurrencyCode).Result;
            var targetCurrency = _curencyService.Get(targetCurrencyCode).Result;
            var exchangeRate = new ExchangeRate(0, baseCurrency.Id, targetCurrency.Id, baseCurrency, targetCurrency, updateRate.Rate);
            var response = await _exchangeRateService.Update(exchangeRate);
            return Ok(response);
        }
    }
}
