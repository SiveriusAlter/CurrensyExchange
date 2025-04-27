using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class CurrencyService(ICurrencyExchangeRepository<Currency> currencyRepository) : ICurrencyExchangeService<Currency>
    {
        private readonly ICurrencyExchangeRepository<Currency> _currencyRepository = currencyRepository;

        public async Task<Currency> Get(int id)
        {
            return await _currencyRepository.Get(id);
        }

        public async Task<List<Currency>> GetAll()
        {
            return await _currencyRepository.GetAll();
        }
    }
}
