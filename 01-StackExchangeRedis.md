# C# for Redis

### Choosing a client library
Following two seem to be most used and best supported libraries: 

- [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)

  > **Recommended** : We will use this one (version used here = `2.2.88`)

- [ServiceStack.Redis](https://github.com/ServiceStack/ServiceStack.Redis)

  > Not recommended as this is commercial and will create problem when project scales, thorowing up exceptions for use exceeded: https://servicestack.net/download#free-quotas.
  
  

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

