
using CurrencyCodesConsoleApp.Models;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");


// dotnet add package NRedisStack

ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("localhost");
IDatabase db = connectionMultiplexer.GetDatabase();

while (true)
{
    Console.Write("Podaj symbol waluty: ");
    var code = Console.ReadLine();


    var baseAddress = new Uri("https://api.nbp.pl/");

    const string table = "a";

    var url = $"api/exchangerates/rates/{table}/{code}?format=json";

    HttpClient client = new HttpClient();
    client.BaseAddress = baseAddress;

    var jsonResult = await client.GetStringAsync(url);


    IJsonCommands json = db.JSON();

    var key = $"exchangerates:rates:{table}:{code}";

    json.Set(key, "$", jsonResult);

    var result = json.Get(key);

    var exchangeRate = json.Get<ExchangeRate>(key);

    Console.WriteLine(exchangeRate);


    var rate = exchangeRate.rates[0];

    rate.effectiveDate = rate.effectiveDate.AddDays(1);
    rate.mid = rate.mid * 1.1m;

    json.ArrAppend(key, "$.rates", rate);


}


    