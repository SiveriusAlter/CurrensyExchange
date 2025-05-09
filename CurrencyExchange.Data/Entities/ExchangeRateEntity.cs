namespace CurrencyExchange.Data.Entities
{
    public class ExchangeRateEntity
    {
        public int Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public int TargetCurrencyId { get; set; }
        public required CurrencyEntity BaseCurrency { get; set; }
        public required CurrencyEntity TargetCurrency { get; set; }
        public float Rate { get; set; }
    }
}
