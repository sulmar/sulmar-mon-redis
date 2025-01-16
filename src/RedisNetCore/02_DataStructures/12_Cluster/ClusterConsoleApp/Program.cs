
using StackExchange.Redis;

Console.WriteLine("Hello, World!");

ConfigurationOptions options = new ConfigurationOptions
{
    EndPoints =
    {
        "192.168.104.85:7000",
        "192.168.104.85:7001",
        "192.168.104.85:7002",
        "192.168.104.85:7003",
        "192.168.104.85:7004",
        "192.168.104.85:7005",
    },

    AbortOnConnectFail = false,
    SyncTimeout = 5000,
};


string connectionString = "joe:secret@localhost";

ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect(options);
IDatabase db = connectionMultiplexer.GetDatabase();

Console.WriteLine(db.Ping());

db.StringSet("foo", "boo");

// Console.WriteLine(db.StringGet("foo"));

IBatch batch = db.CreateBatch();

var tasks = Enumerable.Range(0, 5)
    .Select(i => batch.StringSetAsync("foo" + i, i.ToString()))
    .ToList();

// Task task = batch.StringSetAsync("message", "hello");

batch.Execute();

//  await task;

await Task.WhenAll(tasks);



Console.WriteLine("Finished.");

