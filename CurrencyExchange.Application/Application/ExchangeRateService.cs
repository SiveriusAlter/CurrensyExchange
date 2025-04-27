using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class ExchangeRateService(IExtendedCurrencyExchangeRepository<ExchangeRate> exchangeRateRepository) : IExtendedCurrencyExchangeService<ExchangeRate>
    {
        private readonly IExtendedCurrencyExchangeRepository<ExchangeRate> _exchangeRateRepository = exchangeRateRepository;

        public async Task<ExchangeRate> Get(int id)
        {
            return await _exchangeRateRepository.Get(id);
        }

        public async Task<List<ExchangeRate>> GetAll()
        {
            return await _exchangeRateRepository.GetAll();
        }

        public async Task<ExchangeRate> Get(int baseCurrencyId, int targetCurrencyID)
        {
            return await _exchangeRateRepository.Get(baseCurrencyId, targetCurrencyID);
        }

        public float Convert(ExchangeRate rate, float amount)
        {
            return amount * rate.Rate;
        }
    }
}
