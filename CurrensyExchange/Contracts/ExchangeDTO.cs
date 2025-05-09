using CurrencyExchange.Core.Models;

namespace CurrencyExchange.API.Contracts
{
    public record ExchangeDTO(
    Currency BaseCurrency,
    Currency TargetCurrency,
    float Rate,
    float Amount,
    float convertedAmount
    );
}
