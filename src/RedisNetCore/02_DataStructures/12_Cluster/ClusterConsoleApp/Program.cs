
using StackExchange.Redis;

Console.WriteLine("Hello, World!");

ConfigurationOptions options = new ConfigurationOptions
{
    EndPoints =
    {
        "173.18.0.2:6379",
        "173.18.0.3:6379",
        "173.18.0.4:6379",
        "173.18.0.5:6379",
        "173.18.0.6:6379",
        "173.18.0.7:6379",
    }
};

ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(options);
IDatabase db = connectionMultiplexer.GetDatabase();

Console.WriteLine(db.Ping());


IBatch batch = db.CreateBatch();

var tasks = Enumerable.Range(0, 5)
    .Select(i => batch.StringSetAsync("{test:}" + i, i.ToString()))
    .ToList();

// Task task = batch.StringSetAsync("message", "hello");

batch.Execute();

//  await task;

await Task.WhenAll(tasks);

db.StringSet("foo", "boo");

Console.WriteLine(db.StringGet("foo"));

Console.WriteLine("Finished.");

