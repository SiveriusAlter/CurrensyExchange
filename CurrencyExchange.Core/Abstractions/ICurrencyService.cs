using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetAllCurrencies();
    }
}