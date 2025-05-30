using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstractions;

public interface ICurrencyExchangeService<T> where T : class
{
    float Amount { get; }
    ExchangeRate ExchangeRate { get; }
    float RecalculateAmount { get; }

    void Calculate(T rate, float amount);
}