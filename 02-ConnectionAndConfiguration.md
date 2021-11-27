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

