using CurrencyExchange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class CurrencyRepository(CurrencyDBContext dbContext) : ICurrencyRepository
    {
        private readonly CurrencyDBContext _dbContext = dbContext;

        public async Task<List<Currency>> Get()
        {
            var currencyEntity = await _dbContext.Currencies
                .AsNoTracking()
                .ToListAsync();

            var currencies = currencyEntity
                .Select(b => Currency.Create(b.Id, b.Code, b.FullName, b.Sign).currency)
                .ToList();
            return currencies;
        }
    }
}
