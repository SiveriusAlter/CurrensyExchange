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
            var currencies = await _dbContext.Currencies
                .ToListAsync();

            return currencies;
        }

        public async Task<Currency?> Get(string code)
        {
            var currency = await _dbContext.Currencies
                .Where(b => b.Code == code).FirstOrDefaultAsync();

            return currency;
        }

        public bool CheckContain(string code)
        {
            var currencyEntity = _dbContext.Currencies
                .Where(b => b.Code == code).FirstAsync();

            return currencyEntity != null;

        }

        public async Task<Currency> Insert(Currency currency)
        {
            if (Get(currency.Code).Result is null)
            {
                await _dbContext.Currencies.AddAsync(currency);
                await _dbContext.SaveChangesAsync();
                return currency;
            }
            else
            {
                throw new Exception("Валюта уже существует");
            }
        }

        public Task Update(Currency value)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string code)
        {
            throw new NotImplementedException();
        }
    }
}
