using CurrencyExchange.Core.Models;

namespace CurrencyExchange.API.Contracts
{
    public record ExchangeRateResponse(
    int Id,
    Currency BaseCurrency,
    Currency TargetCurrency,
    float Rate
    );
}
