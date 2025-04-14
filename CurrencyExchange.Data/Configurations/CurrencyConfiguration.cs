using CurrencyExchange.Data.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyExchange.Data.Configurations
{
    internal class CurrencyConfiguration : IEntityTypeConfiguration<CurrencyEntity>
    {
        public void Configure(EntityTypeBuilder<CurrencyEntity> builder)
        {
            builder.HasKey(x=>x.ID);
            builder.Property(b => b.Code)
                .IsRequired();
            builder.Property(b => b.FullName)
                .IsRequired();
            builder.Property(b => b.Sign)
                .IsRequired();
        }
    }
}
