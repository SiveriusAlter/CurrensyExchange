namespace CurrencyExchange.Core.Abstractions
{
    public interface ICurrencyExchangeService<T>
        where T : class
    {
        Task<List<T>> GetAll();
        Task<T> Get(string code);
        Task<T> Insert(T item);
    }
}