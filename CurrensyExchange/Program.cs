using CurrencyExchange.Application.Application;
using CurrencyExchange.Core.Abstractions;
using CurrencyExchange.Core.Models;
using CurrencyExchange.Data;
using CurrencyExchange.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CurrencyExchangeDBContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(CurrencyExchangeDBContext)));
    });

builder.Services.AddScoped<ICurrencyExchangeRepository<Currency>, CurrenciesRepository>();
builder.Services.AddScoped<ICurrencyExchangeService, ExchangeService>();
builder.Services.AddScoped<IExtendedCurrencyExchangeRepository<ExchangeRate>, ExchangeRatesRepository>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

//app.UseExceptionHandlerMiddleware();

app.MapControllers();

app.UseCors(policy =>
    {
        policy.WithHeaders().AllowAnyHeader();
        policy.WithOrigins("http://localhost:4200");
        policy.WithMethods().AllowAnyMethod();
    }
    );

app.Run();