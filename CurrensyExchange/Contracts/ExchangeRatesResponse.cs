using CurrencyExchange.Core.Models;

namespace CurrencyExchange.API.Contracts
{
    public record ExchangeRatesResponse(
    int Id,
    Currency BaseCurrency,
    Currency TargetCurrency,
    float Rate
    );
}
