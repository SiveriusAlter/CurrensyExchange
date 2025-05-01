namespace CurrencyExchange.Core.Abstractions
{
    public interface ICurrencyExchangeRepository<T>
        where T : class
    {

        Task<List<T>> GetAll();
        Task<T> Get(string code);
        Task<T> Insert(T value);
        Task Update(T value);
        Task Delete(string code);
        bool CheckContain(string code);
    }
}