using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstractions
{
    public interface IExtendedCurrencyExchangeService<T> : ICurrencyExchangeService<T>
        where T : class
    {
        Task<T> Get(int baseCurrencyId, int targetCurrencyID);
        public float Convert(T rate, float amount);
    }
}