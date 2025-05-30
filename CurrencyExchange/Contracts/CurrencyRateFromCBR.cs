namespace CurrencyExchange.API.Contracts;

public record CurrencyRateFromCBR(
    string Vname,
    int Vnom,
    float Vcurs,
    int Vcode,
    string VchCode,
    float VunitRate
    );