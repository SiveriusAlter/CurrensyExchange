using CurrencyExchange.Data.Entites;
using Microsoft.EntityFrameworkCore;

namespace CurrencyExchange.Data
{
    public class CurrencyDBContext : DbContext
    {
        public CurrencyDBContext(DbContextOptions<CurrencyDBContext> options) 
            : base(options) 
        {
            
        }

        public DbSet<CurrencyEntity> Currensies { get; set; }
    }
}
