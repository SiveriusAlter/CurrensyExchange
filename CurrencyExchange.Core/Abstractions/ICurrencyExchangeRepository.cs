using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstrations
{
    public interface ICurrencyExchangeRepository<T>
        where T : class
    {
        Task<List<T>> GetAll();
    }
}