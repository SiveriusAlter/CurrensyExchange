using CurrencyExchange.Core.Models;

namespace CurrencyExchange.Core.Abstractions
{
    public interface ICurrencyExchangeService
    {
        Currency BaseCurrency { get; set; }
        Currency TargetCurrency { get; set; }
        float Amount { get; set; }
        ExchangeRate ExchangeRate { get; }
        float RecalculateAmount { get; }

        Task Recalculate(Currency baseCurrency, Currency targetCurrency, float amount);
    }
}