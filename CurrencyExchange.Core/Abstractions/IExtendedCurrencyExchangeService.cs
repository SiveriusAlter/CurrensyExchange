using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstractions
{
    public interface IExtendedCurrencyExchangeService<T> : ICurrencyExchangeService<T>
        where T : class
    {
        Task<T> Get(string baseCurrencyCode, string targetCurrencyCode);
        public float Convert(float rate, float amount);
        Task<ExchangeRate?> GetAny(string baseCurrencyCode, string targetCurrencyCode);
        Task<T> Update(T value);
    }
}