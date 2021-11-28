read: https://stackexchange.github.io/StackExchange.Redis/Configuration

# Connections and Configurations

- The redis client has several configuratino options. It is prefered to allow as many of them as relevant from config (`appsettings.json`/ `Env variables` )

- There are some simple flag based options that can be used as below : 

  ```csharp
  // Configuration for Redis
  var redisConfig = new ConfigurationOptions
  {
    EndPoints = { "localhost:6379"},
    Ssl = false,
    AbortOnConnectFail = false,
    SyncTimeout = 5000, // Timeout for sync calls
    //.. many more
  };
  
  // Create connection with Redis
  var redis =  await ConnectionMultiplexer.ConnectAsync(redisConfig);
  ```

- There are callabacks based configuration as well. These can be required for some tricky situations like working around SSL issues on certain OS: 

  ```csharp
  public class RedisClient
  {
    private readonly ConnectionMultiplexer _connection;
  
    public RedisClient()
    {
      // Configuration for Redis
      var redisConfig = new ConfigurationOptions{
        EndPoints = { "localhost:6379"},
        Ssl = false,
      };
  
      redisConfig.CertificateSelection += SelectCertifcate;
      redisConfig.CertificateValidation += ValidateRemoteCertificate;
      
      // Create connection with Redis
      _connection =  ConnectionMultiplexer.Connect(redisConfig);
    }
  
    private bool ValidateRemoteCertificate(
      object sender, 
      X509Certificate? certificate, 
      X509Chain? chain, 
      SslPolicyErrors sslpolicyerrors)
    {
      // Check if remote Redis server has valid certificate
      return true;
    }
  
    private X509Certificate SelectCertifcate(
      object sender, 
      string targethost, 
      X509CertificateCollection localcertificates, 
      X509Certificate? remotecertificate, 
      string[] acceptableissuers)
    {
      // Read your certificate
      return new X509Certificate("my/cert/path", "my_secret");
    }
  }
  ```

  

- Or you may need to explicitly log the events like connection disconnects/reconnects: 

  ```
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
      await db.PingAsync();
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

