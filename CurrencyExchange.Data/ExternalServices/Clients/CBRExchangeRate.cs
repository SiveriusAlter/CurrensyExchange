using System.Globalization;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using CurrencyExchange.Data.Entities;
using CurrencyExchange.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceReference;

namespace CurrencyExchange.Data.ExternalServices.Clients;

public class CBRExchangeRate(
    IServiceScopeFactory scopeFactory,
    int delaySeconds) : IHostedService, IAsyncDisposable
{
    private readonly Task _completedTask = Task.CompletedTask;

    private static Timer? _timer;
    private List<CBRExchangeRateEntity> CBRExchangeRates { get; } = [];

    public IServiceScopeFactory ScopeFactory { get; } = scopeFactory;

    private async Task GetFromCBR()
    {
        using var scope = ScopeFactory.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<DailyInfoSoapClient>();
        var daily = await client.GetCursOnDateAsync(DateTime.Now);
        var nodes = daily.Nodes[1].Element("ValuteData");
        if (nodes == null) return;
        foreach (var node in nodes.Elements("ValuteCursOnDate"))
        {
            var code = node.Element("VchCode").Value;
            var name = node.Element("Vname").Value.Trim();
            var rate = float.Parse(node.Element("VunitRate").Value, CultureInfo.InvariantCulture);
            CBRExchangeRates.Add(new CBRExchangeRateEntity(code, name, rate));
        }
    }


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(Update, null, TimeSpan.Zero, TimeSpan.FromSeconds(delaySeconds));
        return _completedTask;
    }

    private async void Update(object? state)
    {
        using var scope = ScopeFactory.CreateScope();
        var exchangeRateRepository = scope.ServiceProvider.GetRequiredService<IExchangeRateRepository<ExchangeRate>>();
        await GetFromCBR();
        if (CBRExchangeRates.Count == 0) return;
        foreach (var exchangeRateCBR in CBRExchangeRates)
        {
            var exist = await exchangeRateRepository
                .CheckExist(exchangeRateCBR.BaseCurrencyCode, exchangeRateCBR.TargetCurrencyCode);
            if (exist)
            {
                await exchangeRateRepository
                    .Update(exchangeRateCBR.BaseCurrencyCode, 
                        exchangeRateCBR.TargetCurrencyCode, 
                        exchangeRateCBR.Rate);
                Console.WriteLine("Обновил!");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return _completedTask;
    }


    public async ValueTask DisposeAsync()
    {
        if (_timer is IAsyncDisposable timer)
        {
            await timer.DisposeAsync();
        }

        _timer = null;
    }
}