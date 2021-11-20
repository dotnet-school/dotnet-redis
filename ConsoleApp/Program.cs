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
  }
}