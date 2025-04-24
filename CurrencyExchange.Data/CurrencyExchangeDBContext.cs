using CurrencyExchange.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data
{
    public class CurrencyExchangeDBContext(DbContextOptions<CurrencyExchangeDBContext> options) : DbContext(options)
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}
