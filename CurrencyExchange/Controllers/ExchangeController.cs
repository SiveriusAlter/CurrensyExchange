using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExchangeController(
    ICurrencyExchangeService<ExchangeRate> exchange,
    ICurrencyRepository<Currency> currency,
    IExchangeRateRepository<ExchangeRate> rateRepository)
    : ControllerBase
{
    [HttpGet("from={baseCurrencyCode}&to={targetCurrencyCode}&amount={amount:float}")]
    public async Task<ActionResult<ExchangeDTO>> GetConverted(string baseCurrencyCode, string targetCurrencyCode,
        float amount)
    {
        var baseCurrency = await currency.Get(baseCurrencyCode)
                           ?? throw new ArgumentException(
                               "Не удалось найти валюту");
        var targetCurrency = await currency.Get(targetCurrencyCode)
                             ?? throw new ArgumentException(
                                 "Не удалось найти валюту");
        ;

        var exchangeRate = await rateRepository.Get(baseCurrency, targetCurrency)
                           ?? await rateRepository.GetAndSaveRevers(baseCurrency, targetCurrency)
                           ?? await rateRepository.GetAndSaveCross(baseCurrency, targetCurrency)
                           ?? throw new InvalidOperationException(
                               "Не удалось найти или создать подходящий обменный курс");

        exchange.Calculate(exchangeRate, amount);

        var response = new ExchangeDTO(exchange.ExchangeRate.BaseCurrency, exchange.ExchangeRate.TargetCurrency, exchange.ExchangeRate.Rate,
            amount, exchange.RecalculateAmount);

        return Ok(response);
    }
}