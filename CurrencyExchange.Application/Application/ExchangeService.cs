using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class ExchangeService(
        IExtendedCurrencyExchangeRepository<ExchangeRate> exchangeRateRepository,
        ICurrencyExchangeRepository<Currency> currencyRepository) : ICurrencyExchangeService
    {
        private readonly IExtendedCurrencyExchangeRepository<ExchangeRate> _exchangeRateRepository =
            exchangeRateRepository;

        private readonly ICurrencyExchangeRepository<Currency> _currencyRepository = currencyRepository;

        public Currency? BaseCurrency { get; private set; }
        public Currency? TargetCurrency { get; private set; }
        public float Amount { get; private set; }
        public ExchangeRate? ExchangeRate { get; private set; }
        public float RecalculateAmount { get; private set; }


        public async Task Calculation(Currency baseCurrency, Currency targetCurrency,
            float amount)
        {
            ArgumentNullException.ThrowIfNull(baseCurrency);
            ArgumentNullException.ThrowIfNull(targetCurrency);
            if (amount < 0)
            {
                throw new ArgumentException("Сумма не может быть отрицательной", nameof(amount));
            }

            BaseCurrency = baseCurrency;
            TargetCurrency = targetCurrency;
            Amount = amount;

            ExchangeRate = await GetDirect()
                           ?? await GetAndSaveRevers()
                           ?? await GetAndSaveCross()
                           ?? throw new InvalidOperationException(
                               "Не удалось найти или создать подходящий обменный курс");

            RecalculateAmount = Convert(ExchangeRate.Rate, amount);
        }

        
        private async Task<ExchangeRate?> GetDirect()
        {
            var directRate = await _exchangeRateRepository.Get(BaseCurrency.Id, TargetCurrency.Id);
            return directRate;
        }

        
        private async Task<ExchangeRate?> GetAndSaveRevers()
        {
            var reverseRate = await _exchangeRateRepository.Get(TargetCurrency.Id, BaseCurrency.Id);

            if (reverseRate == null)
            {
                return null;
            }

            if (reverseRate.Rate <= 0)
            {
                throw new DivideByZeroException("Курс не может быть меньше или равен нулю!");
            }

            var newDirectRate = ExchangeRate
                .Create(0, reverseRate.TargetCurrency, reverseRate.BaseCurrency, 1 / reverseRate.Rate);
            return await _exchangeRateRepository.Insert(newDirectRate);
        }

        
        private async Task<ExchangeRate> Create()
        {
            var exchangeRate = await GetAndSaveRevers()
                               ?? await GetAndSaveCross()
                               ?? throw new Exception("Валютная пара не найдена");

            var newExchangeRate = await _exchangeRateRepository.Insert(exchangeRate);
            return newExchangeRate;
        }


        private async Task<ExchangeRate?> GetAndSaveCross()
        {
            var currencies = await _currencyRepository.GetAll();
            foreach (var currency in currencies)
            {
                var baseRate = await _exchangeRateRepository.Get(BaseCurrency.Id, currency.Id);
                var targetRate = await _exchangeRateRepository.Get(TargetCurrency.Id, currency.Id);

                if (baseRate is not null && targetRate is not null)
                {
                    var exchangeRate = ExchangeRate.Create(
                        0,
                        BaseCurrency,
                        TargetCurrency,
                        baseRate.Rate / targetRate.Rate
                    );
                    return await _exchangeRateRepository.Insert(exchangeRate);
                }

                baseRate = await _exchangeRateRepository.Get(currency.Id, BaseCurrency.Id);
                targetRate = await _exchangeRateRepository.Get(currency.Id, TargetCurrency.Id);

                if (baseRate is not null && targetRate is not null)
                {
                    var exchangeRate = ExchangeRate.Create(
                        0,
                        BaseCurrency,
                        TargetCurrency,
                        targetRate.Rate / baseRate.Rate
                    );
                    return await _exchangeRateRepository.Insert(exchangeRate);
                }
            }

            return null;
        }


        private static float Convert(float rate, float amount)
        {
            return (float)Math.Round(amount * rate, 2, MidpointRounding.ToZero);
        }
    }
}