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
            return await _dbContext.Currencies
                .ToListAsync();
        }

        public async Task<Currency?> Get(string code)
        {
            return await _dbContext.Currencies
                .Where(b => b.Code == code)
                .FirstOrDefaultAsync();

        }

        public async Task<Currency> Insert(Currency currency)
        {
            var exist = await _dbContext.Currencies
                .AnyAsync(c => c.Code == currency.Code);

            if (exist) throw new InvalidOperationException("Валюта уже существует!");

                await _dbContext.Currencies.AddAsync(currency);
                await _dbContext.SaveChangesAsync();
                return currency;
        }
    }
}
