using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstractions;

public interface IExchangeRateRepository<T> : ICurrencyRepository<T>
    where T : class
{
    Task<T?> Get(int baseCurrencyId, int targetCurrencyId);
    Task<T?> Get(Currency baseCurrency, Currency targetCurrency);

    Task<T?> GetAndSaveRevers(Currency BaseCurrency, Currency TargetCurrency);

    Task<T?> GetAndSaveCross(Currency BaseCurrency, Currency TargetCurrency);
    Task<T> Update(T exchangeRate);
    Task Update(string baseCurrencyCode, string targetCurrencyCode, float rate);

    Task<bool> CheckExist(ExchangeRate exchangeRate);
    Task<bool> CheckExist(string baseCurrencyCode, string targetCurrencyCode);

}