using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ConsoleApp
{
  public class Program
  {
    public static async Task Main()
    {
      // Configuration for Redis
      var redisConfig = new ConfigurationOptions{EndPoints = { "localhost:6379"}};
      
      // Create connection with Redis
      var redis =  await ConnectionMultiplexer.ConnectAsync(redisConfig);
      var db = redis.GetDatabase();
      
      // Ping redis and check latency
      var pingResult = await db.PingAsync();
      Console.WriteLine($"Redis latency : {pingResult.TotalMilliseconds}ms");
      SetStringIfNotAlreadyPresent(db);
      SetStringIfNotAlreadyPresentWithTTL(db);
    }

    public static void SetStringIfNotAlreadyPresent(IDatabase db)
    {
      // Delete key if it exists
      db.KeyDelete("my-key");
      
      // First time should set value
      db.StringSetAsync("my-key", "first value", null, When.NotExists);
      
      var result = db.StringGet("my-key");
      Console.WriteLine($"Value after first set: {result}");
      
      // Second time should not overwrite first value
      db.StringSetAsync("my-key", "second value", null, When.NotExists);
      var result2 = db.StringGet("my-key");
      Console.WriteLine($"Value after second set: {result2}");
    }
    public static void SetStringIfNotAlreadyPresentWithTTL(IDatabase db)
    {
      // Delete key if it exists
      db.KeyDelete("my-key");
      
      // First time should set value
      db.StringSetAsync("my-key", "first value", TimeSpan.FromMilliseconds(100), When.NotExists);
      
      var result = db.StringGet("my-key");
      Console.WriteLine($"Value after first set: {result}");
      
      // Second time should not overwrite first value
      db.StringSetAsync("my-key", "second value", null, When.NotExists);
      Console.WriteLine($"Value after second set: {db.StringGet("my-key")}");
      
      Task.Delay(TimeSpan.FromMilliseconds(100)).GetAwaiter().GetResult();
      Console.WriteLine($"Value after TTL: {db.StringGet("my-key")}");
      
      // Third time should succeed as value is expired now
      db.StringSetAsync("my-key", "third value", null, When.NotExists);
      Console.WriteLine($"Value after third set: {db.StringGet("my-key")}");
    }
  }
}