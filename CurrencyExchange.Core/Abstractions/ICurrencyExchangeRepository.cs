namespace CurrencyExchange.Core.Abstractions;

public interface ICurrencyExchangeRepository<T>
    where T : class
{
    Task<List<T>> GetAll();

    Task<T?> Get(string code)
    {
        throw new NotSupportedException();
    }

    Task<List<T>> Find(string findText);
    
    Task<T> Insert(T value);
    Task<bool> CheckExist(T value);
}