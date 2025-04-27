namespace CurrencyExchange.Core.Abstractions
{
    public interface IExtendedCurrencyExchangeRepository<T> : ICurrencyExchangeRepository<T>
        where T : class
    {
        Task<T> Get(int baseCurrencyId, int targetCurrencyId);
    }
}
