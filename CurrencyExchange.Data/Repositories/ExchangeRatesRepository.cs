using CurrencyExchange.Core.Abstrations;
using CurrencyExchange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class ExchangeRatesRepository(CurrencyExchangeDBContext dbContext) : ICurrencyExchangeRepository<ExchangeRate>
    {
        private readonly CurrencyExchangeDBContext _dbContext = dbContext;

        public async Task<List<ExchangeRate>> GetAll()
        {

            var exchangeEntity = await (from exchangeRate in _dbContext.ExchangeRates
                                 join baseCurrency in _dbContext.Currencies on exchangeRate.BaseCurrencyId equals baseCurrency.Id
                                 join targetCurrency in _dbContext.Currencies on exchangeRate.TargetCurrencyId equals targetCurrency.Id
                                 select new {
                                 exchangeRate.Id,
                                 exchangeRate.BaseCurrencyId,
                                 exchangeRate.TargetCurrencyId,
                                 baseCurrency,
                                 targetCurrency,
                                 exchangeRate.Rate})
                                 .ToArrayAsync();


            var exchanges = exchangeEntity
                .Select(b => ExchangeRate.Create(b.Id, b.BaseCurrencyId, b.TargetCurrencyId, b.baseCurrency, b.targetCurrency, b.Rate).exchangeRate)
                .ToList();
            return exchanges;
        }
    }
}
