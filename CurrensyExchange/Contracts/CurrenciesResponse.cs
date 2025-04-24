using CurrencyExchange.Core.Models;

namespace CurrencyExchange.API.Contracts
{
    public record CurrenciesResponse(
        int ID,
        string Code,
        string FullName,
        string Sign
        );
}
