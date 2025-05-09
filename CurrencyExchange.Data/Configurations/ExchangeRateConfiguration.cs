using CurrencyExchange.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CurrencyExchange.Data.Configurations
{
    internal class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRateEntity>
    {
        public void Configure(EntityTypeBuilder<ExchangeRateEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(b => b.BaseCurrencyId)
                .IsRequired();
            builder.Property(b => b.TargetCurrencyId)
                .IsRequired();
            builder.Property(b => b.BaseCurrency)
                .IsRequired();
            builder.Property(b => b.TargetCurrency)
                .IsRequired();
            builder.Property(b => b.Rate)
                .IsRequired();
        }
    }
}
