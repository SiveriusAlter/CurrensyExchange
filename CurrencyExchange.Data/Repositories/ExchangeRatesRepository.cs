using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class ExchangeRatesRepository(CurrencyExchangeDBContext dbContext) : IExtendedCurrencyExchangeRepository<ExchangeRate>
    {
        private readonly CurrencyExchangeDBContext _dbContext = dbContext;

        public async Task<List<ExchangeRate>> GetAll()
        {

            var exchangeEntities = await (from exchangeRate in _dbContext.ExchangeRates
                                          join baseCurrency in _dbContext.Currencies on exchangeRate.BaseCurrencyId equals baseCurrency.Id
                                          join targetCurrency in _dbContext.Currencies on exchangeRate.TargetCurrencyId equals targetCurrency.Id
                                          select new
                                          {
                                              exchangeRate.Id,
                                              exchangeRate.BaseCurrencyId,
                                              exchangeRate.TargetCurrencyId,
                                              baseCurrency,
                                              targetCurrency,
                                              exchangeRate.Rate
                                          })
                                 .ToArrayAsync();


            var exchanges = exchangeEntities
                .Select(b => ExchangeRate.Create(b.Id, b.BaseCurrencyId, b.TargetCurrencyId, b.baseCurrency, b.targetCurrency, b.Rate).exchangeRate)
                .ToList();

            return exchanges;
        }

        public async Task<ExchangeRate> Get(string code)
        {
            var exchangeEntity = await (from rate in _dbContext.ExchangeRates
                                        join baseCurrency in _dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                                        join targetCurrency in _dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
                                        where rate.BaseCurrency.Code == code || rate.TargetCurrency.Code == code
                                        select new
                                        {
                                            rate.Id,
                                            rate.BaseCurrencyId,
                                            rate.TargetCurrencyId,
                                            baseCurrency,
                                            targetCurrency,
                                            rate.Rate
                                        })
                                        .FirstAsync();


            var exchange = ExchangeRate
                .Create(exchangeEntity.Id,
                exchangeEntity.BaseCurrencyId,
                exchangeEntity.TargetCurrencyId,
                exchangeEntity.baseCurrency,
                exchangeEntity.targetCurrency,
                exchangeEntity.Rate)
                .exchangeRate;

            return exchange;

        }

        public async Task<ExchangeRate> Get(int baseCurrencyId, int targetCurrencyId)
        {
            ExchangeRate result;
            if (baseCurrencyId == targetCurrencyId)
            {
                var currency = await _dbContext.Currencies.Where(b => b.Id == baseCurrencyId).FirstAsync();
                result = ExchangeRate.Create(0, baseCurrencyId, targetCurrencyId, currency, currency, 1).exchangeRate;
                return result;
            }

            var exchangeRates = await (from rate in _dbContext.ExchangeRates
                                       join baseCurrency in _dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                                       join targetCurrency in _dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
                                       where rate.BaseCurrencyId == baseCurrencyId && rate.TargetCurrencyId == targetCurrencyId
                                       || rate.BaseCurrencyId == targetCurrencyId && rate.TargetCurrencyId == baseCurrencyId
                                       select new
                                       {
                                           rate.Id,
                                           rate.BaseCurrencyId,
                                           rate.TargetCurrencyId,
                                           baseCurrency,
                                           targetCurrency,
                                           rate.Rate
                                       })
                                        .ToListAsync();

            foreach (var rate in exchangeRates)
            {
                if (rate.BaseCurrencyId
                    == baseCurrencyId)
                {
                    result = ExchangeRate.Create(rate.Id, rate.BaseCurrencyId, rate.TargetCurrencyId, rate.baseCurrency, rate.targetCurrency, rate.Rate).exchangeRate;
                    return result;
                }
            };

            result = ExchangeRate
                .Create(exchangeRates[0].Id, exchangeRates[0].TargetCurrencyId, exchangeRates[0].BaseCurrencyId,
                exchangeRates[0].targetCurrency, exchangeRates[0].baseCurrency, 1 / exchangeRates[0].Rate).exchangeRate;
            return result;
        }

        public Task<ExchangeRate> Insert(ExchangeRate exchangeRate)
        {
            throw new NotImplementedException();
        }

        public Task Update(ExchangeRate value)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string code)
        {
            throw new NotImplementedException();
        }

        public bool CheckContain(string code)
        {
            throw new NotImplementedException();
        }
    }
}
