using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using CurrencyExchange.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class ExchangeRatesRepository(CurrencyExchangeDBContext dbContext)
        : IExchangeRateRepository<ExchangeRate>
    {
        public async Task<List<ExchangeRate>> GetAll()
        {
            return await (from rate in dbContext.ExchangeRates
                    join baseCurrency in dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                    join targetCurrency in dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
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

        public async Task<List<ExchangeRate>> Find(string findText)
        {
            return await (from rate in dbContext.ExchangeRates
                    where EF.Functions.ILike(rate.BaseCurrency.Code,$"{findText}%") 
                          || EF.Functions.ILike(rate.TargetCurrency.Code,$"{findText}%")
                          || EF.Functions.ILike(rate.BaseCurrency.FullName,$"%{findText}%")
                          || EF.Functions.ILike(rate.TargetCurrency.FullName,$"%{findText}%")
                    select ExchangeRate.Create
                    (
                        rate.Id,
                        Currency.Create
                        (rate.BaseCurrency.Id,
                         rate.BaseCurrency.Code,
                         rate.BaseCurrency.FullName,
                         rate.BaseCurrency.Sign
                        ),
                        Currency.Create
                        (rate.TargetCurrency.Id,
                            rate.TargetCurrency.Code,
                            rate.TargetCurrency.FullName,
                            rate.TargetCurrency.Sign
                        ),
                        rate.Rate
                    ))
                .ToListAsync();
        }

        public async Task<ExchangeRate?> Get(int baseCurrencyId, int targetCurrencyId)
        {
            if (baseCurrencyId == targetCurrencyId)
            {
                var currency = await dbContext.Currencies
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

            return await (from rate in dbContext.ExchangeRates
                    join baseCurrency in dbContext.Currencies on rate.BaseCurrencyId equals baseCurrency.Id
                    join targetCurrency in dbContext.Currencies on rate.TargetCurrencyId equals targetCurrency.Id
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

            var exchangeRateEntity = new ExchangeRateEntity()
            {
                BaseCurrencyId = exchangeRate.BaseCurrency.Id,
                TargetCurrencyId = exchangeRate.TargetCurrency.Id,
                Rate = exchangeRate.Rate
            };
            await dbContext.AddAsync(exchangeRateEntity);
            await dbContext.SaveChangesAsync();
            return exchangeRate;
        }


        public async Task<ExchangeRate> Update(ExchangeRate exchangeRate)
        {
            var checkExistence = await CheckExist(exchangeRate);
            if (!checkExistence)
            {
                throw new InvalidOperationException("Валютная пара не найдена");
            }

            await dbContext.ExchangeRates
                .Where(b => exchangeRate.BaseCurrency.Id == b.BaseCurrencyId &&
                            exchangeRate.TargetCurrency.Id == b.TargetCurrencyId)
                .ExecuteUpdateAsync(set => set.SetProperty(s => s.Rate, exchangeRate.Rate));
            await dbContext.SaveChangesAsync();
            return exchangeRate;
        }

        public async Task Update(string baseCurrencyCode, string targetCurrencyCode, float rate)
        {
            var checkExistence = await CheckExist(baseCurrencyCode, targetCurrencyCode);
            if (!checkExistence)
            {
                throw new InvalidOperationException("Валютная пара не найдена");
            }

            var exchangeRate = await dbContext.ExchangeRates
                .Where(b => baseCurrencyCode == b.BaseCurrency.Code &&
                            targetCurrencyCode == b.TargetCurrency.Code)
                .ExecuteUpdateAsync(set => set.SetProperty(s => s.Rate, rate));
            await dbContext.SaveChangesAsync();
        }


        public async Task<bool> CheckExist(ExchangeRate exchangeRate)
        {
            return await dbContext.ExchangeRates
                .AnyAsync(er =>
                    er.BaseCurrencyId == exchangeRate.BaseCurrency.Id &&
                    er.TargetCurrencyId == exchangeRate.TargetCurrency.Id);
        }
        
        public async Task<bool> CheckExist(string baseCurrencyCode, string targetCurrencyCode)
        {
            return await dbContext.ExchangeRates
                .AnyAsync(er =>
                    er.BaseCurrency.Code == baseCurrencyCode &&
                    er.TargetCurrency.Code == targetCurrencyCode);
        }
        
        

        public async Task<ExchangeRate?> Get(Currency BaseCurrency, Currency TargetCurrency)
        {
            var directRate = await Get(BaseCurrency.Id, TargetCurrency.Id);
            return directRate;
        }


        public async Task<ExchangeRate?> GetAndSaveRevers(Currency BaseCurrency, Currency TargetCurrency)
        {
            var reverseRate = await Get(TargetCurrency.Id, BaseCurrency.Id);

            if (reverseRate == null)
            {
                return null;
            }

            if (reverseRate.Rate <= 0)
            {
                throw new DivideByZeroException("Курс не может быть меньше или равен нулю!");
            }

            var newDirectRate = ExchangeRate
                .Create(0, reverseRate.TargetCurrency, reverseRate.BaseCurrency, 1 / reverseRate.Rate);
            return await Insert(newDirectRate);
        }


        public async Task<ExchangeRate?> GetAndSaveCross(Currency BaseCurrency, Currency TargetCurrency)
        {
            var currencies = await GetAll();
            foreach (var currency in currencies)
            {
                var baseRate = await Get(BaseCurrency.Id, currency.Id);
                var targetRate = await Get(TargetCurrency.Id, currency.Id);

                if (baseRate is not null && targetRate is not null)
                {
                    var exchangeRate = ExchangeRate.Create(
                        0,
                        BaseCurrency,
                        TargetCurrency,
                        baseRate.Rate / targetRate.Rate
                    );
                    return await Insert(exchangeRate);
                }

                baseRate = await Get(currency.Id, BaseCurrency.Id);
                targetRate = await Get(currency.Id, TargetCurrency.Id);

                if (baseRate is not null && targetRate is not null)
                {
                    var exchangeRate = ExchangeRate.Create(
                        0,
                        BaseCurrency,
                        TargetCurrency,
                        targetRate.Rate / baseRate.Rate
                    );
                    return await Insert(exchangeRate);
                }
            }

            return null;
        }
        
    }
}