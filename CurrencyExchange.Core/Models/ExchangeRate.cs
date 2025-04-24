namespace CurrencyExchange.Core.Models
{
    public class ExchangeRate
    {
        public ExchangeRate(int Id, int BaseCurrencyId, int TargetCurrencyId, Currency baseCurrency, Currency targetCurrency, float Rate)
        {
            this.Id = Id;
            this.BaseCurrencyId = BaseCurrencyId;
            this.TargetCurrencyId = TargetCurrencyId;
            this.BaseCurrency = baseCurrency;
            this.TargetCurrency = targetCurrency;
            this.Rate = Rate;
        }

        public ExchangeRate(int Id, int BaseCurrencyId, int TargetCurrencyId, float Rate)
        {
            this.Id = Id;
            this.BaseCurrencyId = BaseCurrencyId;
            this.TargetCurrencyId = TargetCurrencyId;
            this.Rate = Rate;

        }

        public int Id { get; set; }
        public int BaseCurrencyId { get; set; }
        public int TargetCurrencyId { get; set; }
        public Currency BaseCurrency { get; }
        public Currency TargetCurrency { get; }
        public float Rate { get; set; }

        public static (ExchangeRate exchangeRate, string error) Create(int id, int baseCurrencyId, int targetCurrencyId, Currency baseCurrency, Currency targetCurrency, float rate)
        {
            string error = string.Empty;
            if (baseCurrencyId == targetCurrencyId)
            {
                error = "Выбраны одинаковые валюты";
            }

            var exchangeRate = new ExchangeRate(id, baseCurrencyId, targetCurrencyId, baseCurrency, targetCurrency, rate);

            return (exchangeRate, error);
        }
    }
}
