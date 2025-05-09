namespace CurrencyExchange.Core.Models
{
    public class ExchangeRate
    {
        private ExchangeRate(int id, Currency baseCurrency, Currency targetCurrency, float rate)
        {
            Id = id;
            BaseCurrency = baseCurrency;
            TargetCurrency = targetCurrency;
            Rate = rate;
        }

        public int Id { get; }
        public Currency BaseCurrency { get; }
        public Currency TargetCurrency { get; }
        public float Rate { get; }

        public static ExchangeRate Create(int id, Currency baseCurrency, Currency targetCurrency, float rate)
        {

            if (rate == 0)
            {
                throw new ArgumentException("Курс не может быть равен нулю!");
            }
            rate = (float)Math.Round(rate, 6);

            return new ExchangeRate(id, baseCurrency, targetCurrency, rate);
        }

    }
}
