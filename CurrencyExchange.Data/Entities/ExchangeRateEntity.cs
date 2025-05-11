using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Data.Entities
{
    public class ExchangeRateEntity()
    {

        public int Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public int TargetCurrencyId { get; set; }
        public CurrencyEntity BaseCurrency { get; set; }
        public CurrencyEntity TargetCurrency { get; set; }
        public float Rate { get; set; }
    }
}
