using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class ExchangeService : ICurrencyExchangeService<ExchangeRate>
    {
        private const int DigitAfterDecimalPoint = 2;
        public float Amount { get; private set; }

        public ExchangeRate? ExchangeRate { get; private set; }

        public float RecalculateAmount { get; private set; }


        public void Calculate(ExchangeRate exchangeRate, float amount)
        {
            ExchangeRate = exchangeRate
                           ?? throw new ArgumentNullException(nameof(exchangeRate), "Не задан курс валют!");
            if (ValidateAmount(amount))
            {
                Amount = amount;
            }

            RecalculateAmount = Convert(ExchangeRate.Rate, Amount);
        }

        private static bool ValidateAmount(float amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), amount, "Сумма не может быть меньше нуля!");
            }

            return true;
        }

        private static float Convert(float rate, float amount)
        {
            return (float)Math.Round(amount * rate, DigitAfterDecimalPoint, MidpointRounding.ToZero);
        }
    }
}