using CurrencyExchange.Core.Models;
using CurrencyExchange.Data.Repositories;

namespace CurrencyExchange.Application.Application
{
    public class CurrencyService(ICurrencyRepository currencyRepository) : ICurrencyService
    {
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;

        public async Task<List<Currency>> GetAllCurrencies()
        {
            return await _currencyRepository.Get();
        }
    }
}
