using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class ExchangeService(IExtendedCurrencyExchangeRepository<ExchangeRate> exchangeRateRepository, ICurrencyExchangeRepository<Currency> currencyRepository)
        : ICurrencyExchangeService
    {
        private readonly IExtendedCurrencyExchangeRepository<ExchangeRate> _exchangeRateRepository = exchangeRateRepository;
        private readonly ICurrencyExchangeRepository<Currency> _currencyRepository = currencyRepository;

        public Currency BaseCurrency { get; set; }
        public Currency TargetCurrency { get; set; }
        public float Amount { get; set; }
        public ExchangeRate ExchangeRate { get; private set; }
        public float RecalculateAmount { get; private set; }


        public async Task Recalculate(Currency baseCurrency, Currency targetCurrency, float amount)
        {
            BaseCurrency = baseCurrency;
            TargetCurrency = targetCurrency;
            Amount = amount;
            ExchangeRate = await GetExchangeRate(baseCurrency, targetCurrency);
            RecalculateAmount = Convert(ExchangeRate.Rate, amount);
        }

        private async Task<ExchangeRate> GetExchangeRate(Currency baseCurrency, Currency targetCurrency)
        {
            var directRate = await _exchangeRateRepository.Get(baseCurrency.Id, targetCurrency.Id);

            if (directRate is not null)
                return directRate;

            var reverceRate = await _exchangeRateRepository.Get(baseCurrency.Id, targetCurrency.Id);

            if (reverceRate is not null)
            {
                if (reverceRate.Rate == 0)
                {
                    throw new DivideByZeroException("Курс не может быть равен нулю!");
                }
                return ExchangeRate.Create(
                    0,
                    reverceRate.BaseCurrency,
                    reverceRate.TargetCurrency,
                    1 / reverceRate.Rate);
            }

            return await GetCrossRate(baseCurrency.Id, targetCurrency.Id);
        }


        private async Task<ExchangeRate> GetCrossRate(int baseCurrencyId, int targetCurrencyId)
        {
            var currencies = await _currencyRepository.GetAll();
            foreach (var currency in currencies)
            {
                var baseRate = await _exchangeRateRepository.Get(BaseCurrency.Id, currency.Id);
                var targetRate = await _exchangeRateRepository.Get(TargetCurrency.Id, currency.Id);

                if (baseRate is not null && targetRate is not null)
                {

                    return ExchangeRate.Create(0,
                   baseRate.BaseCurrency,
                   targetRate.TargetCurrency,
                   baseRate.Rate / targetRate.Rate);

                }
            }
            throw new Exception("Валютная пара не найдена");
        }


        private static float Convert(float rate, float amount)
        {

            return (float)Math.Round(amount * rate, 2, MidpointRounding.ToZero);

        }
    }
}
