using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using CurrencyExchange.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class ExchangeRatesRepository(CurrencyExchangeDBContext dbContext)
        : IExtendedCurrencyExchangeRepository<ExchangeRate>
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
                        Currency.Create
                        (baseCurrency.Id,
                            baseCurrency.Code,
                            baseCurrency.FullName,
                            baseCurrency.Sign
                        ),
                        Currency.Create
                        (targetCurrency.Id,
                            targetCurrency.Code,
                            targetCurrency.FullName,
                            targetCurrency.Sign
                        ),
                        rate.Rate
                    ))
                .ToListAsync();
        }

        public async Task<List<ExchangeRate>> Get(string currencyCode)
        {
            return await (from rate in _dbContext.ExchangeRates
                    join baseCurrency in _dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                    join targetCurrency in _dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
                    where rate.BaseCurrency.Code == currencyCode || rate.TargetCurrency.Code == currencyCode
                    select ExchangeRate.Create
                    (
                        rate.Id,
                        Currency.Create
                        (baseCurrency.Id,
                            baseCurrency.Code,
                            baseCurrency.FullName,
                            baseCurrency.Sign
                        ),
                        Currency.Create
                        (targetCurrency.Id,
                            targetCurrency.Code,
                            targetCurrency.FullName,
                            targetCurrency.Sign
                        ),
                        rate.Rate
                    ))
                .ToListAsync();
        }

        public async Task<ExchangeRate?> Get(int baseCurrencyId, int targetCurrencyId)
        {
            if (baseCurrencyId == targetCurrencyId)
            {
                var currency = await _dbContext.Currencies
                    .Where(c => c.Id == baseCurrencyId)
                    .FirstAsync();
                return ExchangeRate.Create(
                    0,
                    Currency.Create
                    (currency.Id,
                        currency.Code,
                        currency.FullName,
                        currency.Sign
                    ),
                    Currency.Create
                    (currency.Id,
                        currency.Code,
                        currency.FullName,
                        currency.Sign
                    ),
                    1
                );
            }

            return await (from rate in _dbContext.ExchangeRates
                    join baseCurrency in _dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                    join targetCurrency in _dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
                    where baseCurrency.Id == baseCurrencyId && targetCurrency.Id == targetCurrencyId
                    select ExchangeRate.Create(
                        rate.Id,
                        Currency.Create
                        (baseCurrency.Id,
                            baseCurrency.Code,
                            baseCurrency.FullName,
                            baseCurrency.Sign
                        ),
                        Currency.Create
                        (targetCurrency.Id,
                            targetCurrency.Code,
                            targetCurrency.FullName,
                            targetCurrency.Sign
                        ),
                        rate.Rate
                    ))
                .FirstOrDefaultAsync();
        }


        public async Task<ExchangeRate> Insert(ExchangeRate exchangeRate)
        {
            var checkExistence = await CheckExist(exchangeRate);
            if (checkExistence)
            {
                throw new InvalidOperationException("Валютная пара уже существует");
            }

            var baseCurrencyEntity = new CurrencyEntity(
                exchangeRate.BaseCurrency.Id,
                exchangeRate.BaseCurrency.Code,
                exchangeRate.BaseCurrency.FullName,
                exchangeRate.BaseCurrency.Sign);

            var targetCurrencyEntity = new CurrencyEntity(
                exchangeRate.TargetCurrency.Id,
                exchangeRate.TargetCurrency.Code,
                exchangeRate.TargetCurrency.FullName,
                exchangeRate.TargetCurrency.Sign);

            var exchangeRateEntity = new ExchangeRateEntity()
            {
                BaseCurrencyId = exchangeRate.BaseCurrency.Id,
                TargetCurrencyId = exchangeRate.TargetCurrency.Id,
                Rate = exchangeRate.Rate
            };
            await _dbContext.AddAsync(exchangeRateEntity);
            await _dbContext.SaveChangesAsync();
            return exchangeRate;
        }


        public async Task<ExchangeRate> Update(ExchangeRate exchangeRate)
        {
            var checkExistence = await CheckExist(exchangeRate);
            if (!checkExistence)
            {
                throw new InvalidOperationException("Валютная пара не найдена");
            }

            await _dbContext.ExchangeRates
                .Where(b => exchangeRate.BaseCurrency.Id == b.BaseCurrencyId &&
                            exchangeRate.TargetCurrency.Id == b.TargetCurrencyId)
                .ExecuteUpdateAsync(set => set.SetProperty(s => s.Rate, exchangeRate.Rate));
            return exchangeRate;
        }


        public async Task<bool> CheckExist(ExchangeRate exchangeRate)
        {
            return await _dbContext.ExchangeRates
                .AnyAsync(er =>
                    er.BaseCurrencyId == exchangeRate.BaseCurrency.Id &&
                    er.TargetCurrencyId == exchangeRate.TargetCurrency.Id);
        }
    }
}