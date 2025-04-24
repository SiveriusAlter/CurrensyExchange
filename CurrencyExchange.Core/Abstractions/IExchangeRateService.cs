using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstrations
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRate>> GetAllExchangeRate();
    }
}