# Strings

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
