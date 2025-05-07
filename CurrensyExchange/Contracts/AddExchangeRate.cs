using CurrencyExchange.Core.Models;

namespace CurrencyExchange.API.Contracts
{
    public record AddExchangeRate
    (
        string BaseCurrencyCode,
        string TargetCurrencyCode,
        float Rate
    );
}
