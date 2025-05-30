using Microsoft.Extensions.Hosting;

namespace CurrencyExchange.Core.Abstractions;

public interface IExchangeRateClient : IHostedService, IAsyncDisposable
{
    Task StartAsync(CancellationToken cancellationToken);
    Task StopAsync(CancellationToken cancellationToken);
    ValueTask DisposeAsync();
}