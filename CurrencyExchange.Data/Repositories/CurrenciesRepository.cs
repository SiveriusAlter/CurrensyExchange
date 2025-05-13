using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using CurrencyExchange.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data.Repositories
{
    public class CurrenciesRepository(CurrencyExchangeDBContext dbContext) : ICurrencyExchangeRepository<Currency>
    {
        private readonly CurrencyExchangeDBContext _dbContext = dbContext;

        public async Task<List<Currency>> GetAll()
        {
            var currencyEntities = await _dbContext.Currencies
                .ToListAsync();

            return currencyEntities.Select(c => Currency.Create(
                    c.Id,
                    c.Code,
                    c.FullName,
                    c.Sign))
                .ToList();
        }

        public async Task<Currency?> Get(string code)
        {
            var currencyEntity = await _dbContext.Currencies
                .Where(b => b.Code == code.ToUpperInvariant())
                .FirstAsync();

            return Currency
                .Create(currencyEntity.Id, currencyEntity.Code, currencyEntity.FullName, currencyEntity.Sign);
        }

        public async Task<List<Currency>> Find(string findText)
        {
            var currencyEntities = await _dbContext.Currencies
                .Where(b =>
                    EF.Functions.ILike(b.Code, $"%{findText}%")
                    || EF.Functions.ILike(b.FullName, $"%{findText}%")
                )
                .ToListAsync();

            return currencyEntities.Select(c => Currency
                    .Create(
                        c.Id,
                        c.Code,
                        c.FullName,
                        c.Sign))
                .ToList();
        }

        public async Task<Currency> Insert(Currency currency)
        {
            var exist = await CheckExist(currency);

            if (exist) throw new ArgumentException("Валюта уже существует!");

            CurrencyEntity currencyEntity = new(currency.Id, currency.Code, currency.FullName, currency.Sign);
            await _dbContext.Currencies.AddAsync(currencyEntity);
            await _dbContext.SaveChangesAsync();
            return currency;
        }

        public async Task<bool> CheckExist(Currency currency)
        {
            return await _dbContext.Currencies
                .AnyAsync(c => c.Code == currency.Code.ToUpperInvariant());
        }
    }
}