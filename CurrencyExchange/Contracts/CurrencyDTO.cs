namespace CurrencyExchange.API.Contracts;

public record CurrencyDTO(
    int ID,
    string Code,
    string FullName,
    string Sign
);