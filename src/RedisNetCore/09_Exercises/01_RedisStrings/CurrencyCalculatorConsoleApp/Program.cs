
using CurrencyCalculatorConsoleApp.Models;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

// dotnet add package StackExchange.Redis

using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddDebug());

IConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("localhost:6379", options =>
{
   // options.ReconnectRetryPolicy = new LinearRetry(2000);
    options.ConnectRetry = 5;
    options.LoggerFactory = factory;
});

connectionMultiplexer.ConnectionFailed += (sender, e) =>
{
    if (e.ConnectionType == ConnectionType.Interactive)
        Console.WriteLine($"ConnectionFailed: {e.FailureType}");
};
connectionMultiplexer.ConnectionRestored += (sender, e) =>
{
    if (e.ConnectionType == ConnectionType.Interactive)
        Console.WriteLine($"ConnectionRestored: {e.FailureType}");
};

connectionMultiplexer.ErrorMessage += (sender, e) => Console.WriteLine(e.Message);

var db = connectionMultiplexer.GetDatabase();

const string table = "a";

while (true)
{
    Console.Write("Podaj kod waluty: ");
    var code = Console.ReadLine();

    Console.Write("Podaj kwotę: ");

    if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
    {
        Console.WriteLine("Nieprawidłowy format");

        continue;
    }

    // TODO: Pobierz z cache'a 
    string key = $"exchangerates:{table}:{code}";

    decimal rate;

    RedisValue result;

    try
    {
         result = await db.StringGetAsync(key);
    }
    catch (RedisConnectionException ex)
    {
        result = RedisValue.Null;
    }

    if (!result.HasValue)
    {
        var baseAddress = new Uri("https://api.nbp.pl/");

        var url = $"api/exchangerates/rates/{table}/{code}?format=json";

        HttpClient client = new HttpClient();
        client.BaseAddress = baseAddress;

        var exchangeRate = await client.GetFromJsonAsync<ExchangeRate>(url);

        Console.WriteLine(exchangeRate);

        rate = exchangeRate.rates[0].mid;

        if (connectionMultiplexer.IsConnected)
            await db.StringSetAsync(key, rate.ToString());
    }
    else
    {
        rate = decimal.Parse(result);
    }


    var convertedAmount = amount * rate;

    Console.WriteLine(convertedAmount);

}
