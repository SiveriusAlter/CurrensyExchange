using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstractions;

public interface ICurrencyExchangeService
{
    Currency BaseCurrency { get; }
    Currency TargetCurrency { get; }
    float Amount { get; }
    ExchangeRate ExchangeRate { get; }
    float RecalculateAmount { get; }

    Task Calculation(Currency baseCurrency, Currency targetCurrency, float amount);
}