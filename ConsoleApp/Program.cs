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
      TestObjects.Run(db);
      await SaveAndGetObject(db);
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

    public static async Task SaveAndGetObject(IDatabase db)
    {
      var person1 = Person.Create("Person 1");
      var person2 = Person.Create("Person 2", new []{"red", "blue"}, new []{"Delhi", "Mumbai"});
      var person3 = Person.Create("Person 3", new []{"red", "blue"}, new []{"Mumbai", "Delhi"});
      var person4 = Person.Create("Person 3");

      await db.SaveObjectAsync("one", person1);
      await db.SaveObjectAsync("two", person2);
      await db.SaveObjectAsync("thee", person3);
      await db.SaveObjectAsync("four", person4);

      var person1Read = await db.GetObjectAsync<Person>("one");
      var person2Read = await db.GetObjectAsync<Person>("two");
      var person3Read = await db.GetObjectAsync<Person>("thee");
      var person4Read = await db.GetObjectAsync<Person>("four");

      var allValuesEqual =
              person1.Equals(person1Read) &&
              person2.Equals(person2Read) &&
              person3.Equals(person3Read) &&
              person4.Equals(person4Read);
      
      Console.WriteLine(
              allValuesEqual ? "Successfully handled objects" : "Failed to correctly handle objects");
    }
  }
}