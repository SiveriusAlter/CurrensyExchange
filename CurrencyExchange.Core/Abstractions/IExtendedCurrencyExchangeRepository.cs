using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstractions
{
    public interface IExtendedCurrencyExchangeRepository<T> : ICurrencyExchangeRepository<T>
        where T : class
    {
        Task<T> Get(int baseCurrencyId, int targetCurrencyId);
        //       Task<List<ExchangeRate>> Get(string code);
        Task<ExchangeRate> Update(ExchangeRate exchangeRate);
    }
}
