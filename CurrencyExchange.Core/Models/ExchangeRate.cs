namespace CurrencyExchange.Core.Models;

public class ExchangeRate
{
    private ExchangeRate(int id, Currency baseCurrency, Currency targetCurrency, float rate)
    {
        Id = id;
        BaseCurrency = baseCurrency;
        TargetCurrency = targetCurrency;
        Rate = rate;
    }

    public const int MIN_RATE = 0;
    public const int MAX_RATE = 99999999;
    
    public int Id { get; }
    public Currency BaseCurrency { get; }
    public Currency TargetCurrency { get; }
    public float Rate { get; }

    public static ExchangeRate Create(int id, Currency baseCurrency, Currency targetCurrency, float rate)
    {
        Validate(rate);
        
        return new ExchangeRate(id, baseCurrency, targetCurrency, rate);
    }

    public static void Validate(float rate)
    {
        if (rate <= MIN_RATE) throw new ArgumentException($"Курс не может быть меньше или равен {MIN_RATE}!");
        if (rate > MAX_RATE) throw new ArgumentException($"Курс не может быть больше {MAX_RATE}!");
    }
}