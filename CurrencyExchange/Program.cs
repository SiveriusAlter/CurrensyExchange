using CurrencyExchange.API.Extensions;
using CurrencyExchange.Application.Application;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using CurrencyExchange.Data;
using CurrencyExchange.Data.ExternalServices.Clients;
using CurrencyExchange.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using ServiceReference;

var builder = WebApplication.CreateBuilder(args);
var corsConfig = builder.Configuration.GetSection("CorsConfig:Origin").Get<string[]>()
                 ?? throw new ArgumentNullException(@"CorsConfig:Origin is required.");
var currencyReceiptPeriod = builder.Configuration.GetSection("CurrencyReceiptPeriod:Period").Get<int>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CurrencyExchangeDBContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(CurrencyExchangeDBContext)));
});
builder.Services.AddScoped<ICurrencyRepository<Currency>, CurrenciesRepository>();
builder.Services.AddScoped<ICurrencyExchangeService<ExchangeRate>, ExchangeService>();
builder.Services.AddScoped<IExchangeRateRepository<ExchangeRate>, ExchangeRatesRepository>();
builder.Services.AddScoped<DailyInfoSoapClient>(_ => 
    new DailyInfoSoapClient(DailyInfoSoapClient.EndpointConfiguration.DailyInfoSoap));
builder.Services.AddHostedService(provider =>
{
    var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
    return new CBRExchangeRate(scopeFactory, currencyReceiptPeriod);
});

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseExceptionHandlerMiddleware();

app.MapControllers();

app.UseCors(policy =>
    {
        policy.WithHeaders().AllowAnyHeader();
        policy.WithOrigins(corsConfig);
        policy.WithMethods().AllowAnyMethod();
    }
);

app.Run();