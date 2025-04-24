using CurrencyExchange.Core.Abstrations;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class ExchangeRateService(ICurrencyExchangeRepository<ExchangeRate> exchangeRateRepository) : ICurrencyExchangeService<ExchangeRate>
    {
        private readonly ICurrencyExchangeRepository<ExchangeRate> _exchangeRateRepository = exchangeRateRepository;

        public async Task<List<ExchangeRate>> GetAll()
        {
            return await _exchangeRateRepository.GetAll();
        }
    }
}
