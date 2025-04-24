using CurrencyExchange.Core.Abstrations;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class CurrencyService(ICurrencyExchangeRepository<Currency> currencyRepository) : ICurrencyExchangeService<Currency>
    {
        private readonly ICurrencyExchangeRepository<Currency> _currencyRepository = currencyRepository;

        public async Task<List<Currency>> GetAll()
        {
            return await _currencyRepository.GetAll();
        }
    }
}
