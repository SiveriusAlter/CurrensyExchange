namespace CurrencyExchange.Data.Entities;

public class CBRExchangeRateEntity(string code, string name, float rate)
{
    public string BaseCurrencyCode { get; } =  code;
    public string TargetCurrencyCode { get; set; } = "RUB";
    public string TargetCurrencyName { get; set; } = name;
    public float Rate { get; set; } = rate;
}