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
    }
  }
}