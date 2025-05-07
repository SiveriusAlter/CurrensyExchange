using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CurrencyExchange.Application.Application
{
    public class ExchangeRateService(IExtendedCurrencyExchangeRepository<ExchangeRate> exchangeRateRepository) : IExtendedCurrencyExchangeService<ExchangeRate>
    {
        private readonly IExtendedCurrencyExchangeRepository<ExchangeRate> _exchangeRateRepository = exchangeRateRepository;

        public async Task<List<ExchangeRate>> GetAll()
        {
            return await _exchangeRateRepository.GetAll();
        }

        public async Task<ExchangeRate> Get(string baseCurrencyCode, string targetCurrencyCode)
        {
            return await _exchangeRateRepository.Get(baseCurrencyCode, targetCurrencyCode);
        }

        public async Task<ExchangeRate> GetAny(string baseCurrencyCode, string targetCurrencyCode)
        {
            var directRate = _exchangeRateRepository.Get(baseCurrencyCode, targetCurrencyCode);

            if (directRate.Result is not null)
                return directRate.Result;

            var reverceRate = _exchangeRateRepository.Get(targetCurrencyCode, baseCurrencyCode);

            if (reverceRate.Result is not null)
            {
                if (reverceRate.Result.Rate == 0) 
                    throw new DivideByZeroException("Курс равен нулю. Курс не может быть равен нулю!");

                var rate = reverceRate.Result;
                var newRate = 1 / rate.Rate;
                rate.Rate = newRate;
                return rate;
            }

            return await GetCrossRate(baseCurrencyCode, targetCurrencyCode);
        }

        public float Convert(float rate, float amount)
        {

            return (float)Math.Round(amount * rate, 2, MidpointRounding.ToZero);

        }

        public async Task<ExchangeRate> Insert(ExchangeRate exchangeRate)
        {

            return await _exchangeRateRepository.Insert(exchangeRate);
        }

        public async Task<ExchangeRate> Update(ExchangeRate exchangeRate)
        {
            return await _exchangeRateRepository.Update(exchangeRate);
        }

        public async Task<ExchangeRate> GetCrossRate(string baseCurrencyCode, string targetCurrencyCode)
        {
            var usd = "USD";
            var rub = "RUB";

            var baseRate = await _exchangeRateRepository.Get(baseCurrencyCode, rub);
            var targetRate = await _exchangeRateRepository.Get(targetCurrencyCode, rub);

            if (baseRate is not null && targetRate is not null)
            {

                var result = ExchangeRate
               .Create(0, baseRate.BaseCurrencyId, targetRate.TargetCurrencyId,
               baseRate.BaseCurrency, targetRate.TargetCurrency, baseRate.Rate/targetRate.Rate);
                return result;
            }
            else
            {
                throw new Exception("Валютная пара не найдена");
            }
        }

        public Task<ExchangeRate> Get(string code)
        {
            throw new NotImplementedException();
        }
    }
}
