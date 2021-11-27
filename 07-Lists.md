# Lists



There are sync/async API for all list operations,  like `RPUSH`, `LPUSH`, `RPOP`or `LPOP`

```csharp
var keyForList = "my-list-key";

// List is created if does not exist
await db.ListRightPushAsync(keyForList, "one");

// Append to list
await db.ListRightPushAsync(keyForList, "two");
await db.ListRightPushAsync(keyForList, "three");

// Prepend to list
await db.ListLeftPushAsync(keyForList, "zero");

// Get all values
var values = await db.ListRangeAsync(keyForList);

Console.WriteLine(string.Join(", ", values.Select(v => v)));

// Pop a value from end of list
Console.WriteLine(await db.ListRightPopAsync(keyForList));

// Pop a value from start of list
Console.WriteLine(await db.ListLeftPopAsync(keyForList));
```

Output: 

> zero, one, two, three
> three
> zero



Remember ***Blocking pop is not supported***