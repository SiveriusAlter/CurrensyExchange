using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Application.Application
{
    public class CurrencyService(ICurrencyExchangeRepository<Currency> currencyRepository) : ICurrencyExchangeService<Currency>
    {
        private readonly ICurrencyExchangeRepository<Currency> _currencyRepository = currencyRepository;

        public async Task<Currency> Get(string code)
        {
            return await _currencyRepository.Get(code);
        }

        public async Task<List<Currency>> GetAll()
        {
            return await _currencyRepository.GetAll();
        }

        public async Task<Currency> Insert(Currency currency)
        {
            return await _currencyRepository.Insert(currency);
        }

        public Task<Currency> Update(Currency item)
        {
            throw new NotImplementedException();
        }
    }
}
