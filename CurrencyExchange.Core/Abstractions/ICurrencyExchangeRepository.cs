namespace CurrencyExchange.Core.Abstractions
{
    public interface ICurrencyExchangeRepository<T>
        where T : class
    {

        Task<List<T>> GetAll();
        Task<T> Get(string code);
        Task<T> Insert(T value);
    }
}