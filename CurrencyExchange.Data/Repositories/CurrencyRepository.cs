using CurrencyExchange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class CurrencyRepository(CurrencyDBContext dbContext)
    {
        private readonly CurrencyDBContext DbContext = dbContext;

        public async Task<List<Currency>> Get()
        {
            var currencyEntity = await DbContext.Currensies
                .AsNoTracking()
                .ToListAsync();

            var currencies = currencyEntity
                .Select(b => Currency.Create(b.ID, b.Code, b.FullName, b.Sign).currency)
                .ToList();
            return currencies;
        }
    }
}
