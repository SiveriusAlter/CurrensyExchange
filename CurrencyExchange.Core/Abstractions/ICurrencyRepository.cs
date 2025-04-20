using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Data.Repositories
{
    public interface ICurrencyRepository
    {
        Task<List<Currency>> Get();
    }
}