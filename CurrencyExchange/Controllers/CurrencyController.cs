using System.Globalization;
using CurrencyExchange.API.Contracts;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController(ICurrencyExchangeRepository<Currency> currency) : ControllerBase
{
    private readonly ICurrencyExchangeRepository<Currency> _currencyRepository = currency;

    [HttpGet]
    public async Task<ActionResult<List<CurrencyDTO>>> GetAll()
    {
        var currencies = await _currencyRepository.GetAll();

        var response = currencies
            .Select(b => new CurrencyDTO(b.Id, b.Code, b.FullName, b.Sign));

        return Ok(response);
    }

    [HttpGet("{searchString}")]
    public async Task<ActionResult<CurrencyDTO>> Get(string searchString)
    {
        Currency.Validate(searchString, 1, 24, @"[^A-Za-z() ]");
        var currencies = await _currencyRepository.Find(searchString);
        
        var response = currencies
            .Select(b => new CurrencyDTO(b.Id, b.Code, b.FullName, b.Sign));

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<CurrencyDTO>> Insert(CurrencyDTO currency)
    {
        var response = await _currencyRepository
            .Insert(Currency.Create(currency.ID, currency.Code, currency.FullName, currency.Sign));
        return Ok(response);
    }
}