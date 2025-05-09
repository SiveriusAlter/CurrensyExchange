using CurrencyExchange.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data
{
    public class CurrencyExchangeDBContext(DbContextOptions<CurrencyExchangeDBContext> options) : DbContext(options)
    {
        public DbSet<CurrencyEntity> Currencies { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
    }
}
