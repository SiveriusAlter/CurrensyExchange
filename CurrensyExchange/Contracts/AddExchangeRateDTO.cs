namespace CurrencyExchange.API.Contracts
{
    public record AddExchangeRateDTO
    (
        string BaseCurrencyCode,
        string TargetCurrencyCode,
        float Rate
    );
}
