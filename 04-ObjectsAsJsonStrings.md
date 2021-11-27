# Storing Objects in Redis

- Saving your custom objects is not out-of-box supported with Redis. As we need serialize and deserialize C# types to Redis types and vice versa.

  There can be two approaches to dealing with this : 

  - Use `Json` or `Protobuf`  to represent your object as string/binary in redis values
  - Map your object yourself to Redis types like [Hashes](https://redis.io/topics/data-types#hashes), List, Set etc.

- We will use the Json serializer, and create an extension to `IDatabasee` interface to add two new methods for saving and reading objects from Redis: 

  ```csharp
  using System.Text.Json;
  using System.Threading.Tasks;
  using StackExchange.Redis;
  
  namespace ConsoleApp
  {
    public static class RedisJsonStorageExtension
    {
      public static Task SaveObjectAsync<T>(this IDatabase db, string key, T data)
      {
        var json = JsonSerializer.Serialize(data);
        return db.StringSetAsync(key, json);
      }
      
      public static async Task<T?> GetObjectAsync<T>(this IDatabase db, string key)
      {
        var json = await db.StringGetAsync(key);
        if (json.HasValue)
        {
          return JsonSerializer.Deserialize<T>(json.ToString());
        }
  
        return default(T);
      }
    }
  }
  ```

  

- Now use this to save and fetch objects from Redis

  ```csharp
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
    var nonExisting = await db.GetObjectAsync<Person>("unknown-key");
  
    var allValuesEqual =
      person1.Equals(person1Read) &&
      person2.Equals(person2Read) &&
      person3.Equals(person3Read) &&
      person4.Equals(person4Read) &&
      nonExisting == null;
  
    Console.WriteLine(
      allValuesEqual ? "Successfully handled objects" : "Failed to correctly handle objects");
  }
  
  ```

