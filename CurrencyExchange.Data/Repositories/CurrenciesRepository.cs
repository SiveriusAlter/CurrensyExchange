using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class CurrenciesRepository(CurrencyExchangeDBContext dbContext) : ICurrencyExchangeRepository<Currency>
    {
        private readonly CurrencyExchangeDBContext _dbContext = dbContext;

        public async Task<List<Currency>> GetAll()
        {
            var currencyEntity = await _dbContext.Currencies
                .ToListAsync();

            var currencies = currencyEntity
                .Select(b => Currency.Create(b.Id, b.Code, b.FullName, b.Sign).currency)
                .ToList();
            return currencies;
        }

        public async Task<Currency> Get(int id)
        {
            var currencyEntity = await _dbContext.Currencies
                .Where(b => b.Id == id).FirstAsync();

            var currency = Currency.Create(currencyEntity.Id, currencyEntity.Code, currencyEntity.FullName, currencyEntity.Sign).currency;
            return currency;
        }

    }
}
