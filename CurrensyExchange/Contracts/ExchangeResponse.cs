using CurrencyExchange.Core.Models;

namespace CurrencyExchange.API.Contracts
{
    public record ExchangeResponse(
    Currency BaseCurrency,
    Currency TargetCurrency,
    float Rate,
    float Amount,
    float convertedAmount
    );
}
