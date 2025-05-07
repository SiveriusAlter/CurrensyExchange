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
            return await (from rate in _dbContext.ExchangeRates
                          join baseCurrency in _dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                          join targetCurrency in _dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
                          select ExchangeRate.Create
                          (
                              rate.Id,
                              rate.BaseCurrencyId,
                              rate.TargetCurrencyId,
                              baseCurrency,
                              targetCurrency,
                              rate.Rate
                          ))
                          .ToListAsync();
        }

        public async Task<List<ExchangeRate>> Get(string code)
        {
            var exchangeEntities = await (from rate in _dbContext.ExchangeRates
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
                                        .ToListAsync();


            var exchanges = exchangeEntities
                .Select(b => ExchangeRate.Create(b.Id, b.BaseCurrencyId, b.TargetCurrencyId, b.baseCurrency, b.targetCurrency, b.Rate))
                .ToList();

            return exchanges;

        }

        public async Task<ExchangeRate?> Get(string baseCurrencyCode, string targetCurrencyCode)
        {
            if (baseCurrencyCode == targetCurrencyCode)
            {
                var currency = await _dbContext.Currencies
                    .Where(c => c.Code == baseCurrencyCode)
                    .FirstAsync();
                return ExchangeRate.Create(0, currency.Id, currency.Id, currency, currency, 1);
            }

            return await (from rate in _dbContext.ExchangeRates
                          join baseCurrency in _dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                          join targetCurrency in _dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
                          where baseCurrency.Code == baseCurrencyCode && targetCurrency.Code == targetCurrencyCode
                          select ExchangeRate.Create(
                              rate.Id,
                              rate.BaseCurrencyId,
                              rate.TargetCurrencyId,
                              baseCurrency,
                              targetCurrency,
                              rate.Rate
                          ))
                          .FirstOrDefaultAsync();
        }

        public async Task<ExchangeRate> Insert(ExchangeRate exchangeRate)
        {
            var checkExistence = await _dbContext.ExchangeRates
                .AnyAsync(er =>
                er.BaseCurrencyId == exchangeRate.BaseCurrencyId &&
                er.TargetCurrencyId == exchangeRate.TargetCurrencyId);
            if (checkExistence)
            {
                throw new InvalidOperationException("Валютная пара уже существует");
            }

            await _dbContext.AddAsync(exchangeRate);
            await _dbContext.SaveChangesAsync();
            return exchangeRate;
        }

        public async Task<ExchangeRate> Update(ExchangeRate exchangeRate)
        {
            var checkExistence = await Get(exchangeRate.BaseCurrency.Code, exchangeRate.TargetCurrency.Code) 
                ?? throw new InvalidOperationException("Валютная пара не найдена");

            await _dbContext.ExchangeRates
           .Where(b => exchangeRate.BaseCurrencyId == b.BaseCurrencyId && exchangeRate.TargetCurrencyId == b.TargetCurrencyId)
           .ExecuteUpdateAsync(set => set.SetProperty(s => s.Rate, exchangeRate.Rate));
            return exchangeRate;

        }

        Task<ExchangeRate> ICurrencyExchangeRepository<ExchangeRate>.Get(string code)
        {
            throw new NotImplementedException();
        }
    }
}
