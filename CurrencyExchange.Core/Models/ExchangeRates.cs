namespace CurrencyExchange.Core.Models
{
    public class ExchangeRate(int baseCurrencyId, int targetCurrencyId, decimal rate)
    {
        public int BaseCurrencyID { get; } = baseCurrencyId;
        public int TargetCurrencyID { get; } = targetCurrencyId;
        public decimal Rate { get; } = rate;

        public static (ExchangeRate exchangeRate, string error) Create(int baseCurrencyId, int targetCurrencyId, decimal rate)
        {
            string error = string.Empty;
            if (baseCurrencyId == targetCurrencyId)
            {
                error = "Выбраны одинаковые валюты";
            }

            var exchangeRate = new ExchangeRate(baseCurrencyId, targetCurrencyId, rate);

            return (exchangeRate, error);
        }
    }
}
