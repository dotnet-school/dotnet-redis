

### Todo: 

- [ ] Setup docker-compose with redis

- [ ] Explain multiplexer vs getDatabase(), cheap pass thorugh and reusable

- [ ] Understand `RedisValue`

- [ ] Topics

  - [ ] Strings
  - [ ] Object as json strings
  - [ ] HashMap
  - [ ] List
  - [ ] listening to list
  - [ ] Set

- [ ] Best practices

  - [ ] Wrap IDatabase (for testing/mocking)
  - [ ] Create only one multiplexer

- [ ] Advanced

  - [ ] SSl with redis
  - [ ] mastter slave
  - [ ] kyspace notifications

- [x] Create a client 

- [x] Use `SETENX`

- [x] Save a POCO by its id in redis

- [x] Retrieve a POCO by its id in redis

- [x] Implement connection recovery

  

# Redis with C#



### Chossing a Redis client library : 

Following two seem to be most used and best supported : 

- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)

  > **Recommended** : We will use this one (version used here = `2.2.88`)

- [ServiceStack.Redis](https://github.com/ServiceStack/ServiceStack.Redis)

  > Not recommended as this is commercial and will create problem when project scales, thorowing up exceptions for use exceeded: https://servicestack.net/download#free-quotas.



### Pre-requisite

- Read basics about redis: 

  > https://redis.io/topics/introduction

- Ensure .NET6 is installed

  > Download : https://dotnet.microsoft.com/download/dotnet/6.0

- Ensure docker is installed ( v3.3.1 or above)

  > Download and install from https://www.docker.com/products/docker-desktop.

- CLI

  > Commands in this arcticle are created for bash. You can use most command in a poweshell. Its recommended to use something like `wsl` or `git bash` if you are running on windows.
  >
  > Download and install gitbash (if required) from : https://git-scm.com/downloads

  

### Run Redis using docker

- Run redis as a docker container 

  ```bash
  # Persist Redis data here
  mkdir redis-data
  
  docker run \
  		-p 6379:6379\
      -e ALLOW_EMPTY_PASSWORD=yes \
      -v $(pwd)/redis-data:/bitnami/redis/data \
      --name redis \
      -d   \
      bitnami/redis:latest
  ```

  

- To connect using redis cli : 

  ```bash
  docker exec -it redis redis-cli   
  ```

  

- If you want to stop your Redis container later on: 

  ```bash
  docker stop redis
  ```

  

- To re-start when you need to : 

  ```bash
  docker start redis
  ```

  

- If you want to delete the redis container 

  ```bash
  docker rm redis
  ```



### Create a .NET project

- Set the .NET version for our project : 

  ```bash
  dotnet new globaljson --sdk-version 6.0.100
  ```

  

- Create a gitignore file 

  ```bash
  dotnet new gitignore
  ```

  Open the `.gitignore` file and add `redis-data` dir to it.

  

- Create a new console project

  ```bash
  dotnet new console -o ConsoleApp
  ```

  

- Install the redis library to our project 

  ```bash
  dotnet add ConsoleApp/ConsoleApp.csproj package StackExchange.Redis
  ```



- Create a solution (if you want to )

  ```bash
  dotnet new sln
  dotnet sln add ConsoleApp/ConsoleApp.csproj
  ```

  

### Connect with Redis 

- Create following class in `ConsleApp/Program.cs`

  ```csharp
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
  ```

  If Redis is running, you will see output like : 

  > Redis latency : 2.3453ms



### Set and Get a value in Redis

- For each Redis command (https://redis.io/commands), you should be able to find correspding API in our client `StackExchange.Redis` here: https://stackexchange.github.io/StackExchange.Redis/

- For e.g. we will try to use the [`SETENX`](https://redis.io/commands/setnx) which will only set a value if it does not already exists in redis. Whenever using any command, ensure you check its complexity at https://redis.io/commands. For this case, complexity is `O(1)` which is the best you can have.

  ```
  SET "test.key" "test.value" NX EX 900
  ```

- In our `ConsoleApp/Program.cs`, lets add this method to check how to do this in Csharp 

  ```csharp
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
  ```

  On running the program, you should see folloing output : 

  > Value after first set: first value
  > Value after second set: first value

- Now lets modify above command to add a TTL 

  ```csharp
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
  ```

  On running the program, you should see folloing output : 

  > Value after first set: first value
  > Value after second set: first value
  > Value after TTL: 
  > Value after third set: third value



### Working with Objects

- Saving your custom objects is not straightforward with Redis. As we need serialize and deserialize C# types to Redis types and vice versa.

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



### Connection Recovery

- What happens when the connection to Redis is dropped ? Should we recreated the connection ? How do we check this ? 

- To check this behaviour, lets add following to our main function : 

  ```csharp
  while (true)
  {
    var i = Console.ReadKey();
    if (i.KeyChar == 'q') return;
  
    try{
      await SaveAndGetObject(db);
    }
    catch(Exception e){
      Console.WriteLine($"Failed with error {e}");
    }
  }
  ```

- Now lets start the program and enter some character to save and get value which should succeed.

- Now shutdown the redis cluster using command : 

  ```bash
  docker stop redis
  ```

  Now if we enter some char in CLI, our program will log error lik : 

  > Failed with error StackExchange.Redis.RedisConnectionException: No connection is active/available ......

- Now restart the Redis with command : 

  ```bash
  docker start redis
  ```

  Now enter some char again and the program shoud succeed in saving/getting values from cache.



### References: 

- Redis c# clients: https://docs.redis.com/latest/rs/references/client_references/client_csharp/

- StackExchangeRedis: https://github.com/StackExchange/StackExchange.Redis

- StackExchangeRedis Docs: https://stackexchange.github.io/StackExchange.Redis/

- StackExchangeRedis Best Practices : https://gist.github.com/JonCole/925630df72be1351b21440625ff2671f#stackexchangeredis

  

