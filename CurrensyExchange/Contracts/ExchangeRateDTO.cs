using CurrencyExchange.Core.Models;

namespace CurrencyExchange.API.Contracts;

public record ExchangeRateDTO(
    int Id,
    Currency BaseCurrency,
    Currency TargetCurrency,
    float Rate
);