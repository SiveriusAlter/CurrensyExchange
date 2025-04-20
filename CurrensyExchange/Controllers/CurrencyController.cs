using CurrencyExchange.API.Contracts;
using CurrencyExchange.Application.Application;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyExchange.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class CurrencyController(ICurrencyService currencyService) : ControllerBase
    {
        private readonly ICurrencyService currencyService = currencyService;

        [HttpGet]
        public async Task<ActionResult<List<CurrenciesResponse>>> GetCurrensies()
        {
            var currencies = await currencyService.GetAllCurrencies();

            var response = currencies.Select(b => new CurrenciesResponse(b.Id, b.Code, b.FullName, b.Sign));

            return Ok(response);
        }
    }
}
