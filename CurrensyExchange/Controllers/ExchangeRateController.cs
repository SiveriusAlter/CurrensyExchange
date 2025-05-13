using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ExchangeRateController(
    IExtendedCurrencyExchangeRepository<ExchangeRate> exchangeRate,
    ICurrencyExchangeRepository<Currency> currency) : ControllerBase
{
    private readonly IExtendedCurrencyExchangeRepository<ExchangeRate> _exchangeRateRepository = exchangeRate;
    private readonly ICurrencyExchangeRepository<Currency> _curencyService = currency;

    [HttpGet]
    public async Task<ActionResult<List<ExchangeRateDTO>>> GetRates()
    {
        var exchangeRates = await _exchangeRateRepository.GetAll();
        var response = exchangeRates
            .Select(b => new ExchangeRateDTO(b.Id, b.BaseCurrency, b.TargetCurrency, b.Rate));

        return Ok(response);
    }

    [HttpGet("{searchString}")]
    public async Task<ActionResult<List<ExchangeRateDTO>>> GetRate(string searchString)
    {
        var currencies = await _exchangeRateRepository.Find(searchString);
        var response = currencies
            .Select(b => new ExchangeRateDTO(b.Id, b.BaseCurrency, b.TargetCurrency, b.Rate));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ExchangeRateDTO>> InsertCurrency(AddExchangeRateDTO addExchangeRate)
    {
        ExchangeRate.Validate(addExchangeRate.Rate);

        var baseCurrency = await _curencyService.Get(addExchangeRate.BaseCurrencyCode);
        var targetCurrency = await _curencyService.Get(addExchangeRate.TargetCurrencyCode);

        var exchangeRate = ExchangeRate
            .Create(0, baseCurrency, targetCurrency, addExchangeRate.Rate);

        var response = await _exchangeRateRepository.Insert(exchangeRate);

        return Ok(response);
    }

    [HttpPatch("{baseCurrencyCode}&{targetCurrencyCode}")]
    public async Task<ActionResult<ExchangeRateDTO>> UpdateCurrency(string baseCurrencyCode, string targetCurrencyCode,
        UpdateRateDTO updateRate)
    {
        ExchangeRate.Validate(updateRate.Rate);

        var baseCurrency = await _curencyService.Get(baseCurrencyCode);
        var targetCurrency = await _curencyService.Get(targetCurrencyCode);

        var exchangeRate = ExchangeRate.Create(0, baseCurrency, targetCurrency, updateRate.Rate);
        var response = await _exchangeRateRepository.Update(exchangeRate);

        return Ok(response);
    }
}