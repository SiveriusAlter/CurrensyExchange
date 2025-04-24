using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstrations
{
    public interface ICurrencyExchangeService<T> where T : class
    {
        Task<List<T>> GetAll();
    }
}