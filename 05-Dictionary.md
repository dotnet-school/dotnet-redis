# HashMaps

HasMap in Redis are useful to create a logical group of key-value pair. 

For e.g. when we stored person value as Json String, it works when we only need to either save or get the object. But what if we need to store some object whose fields can be written in parallel by different programs ? 

**Use dictionary when you need to store objects whose fields can be updated concurrently**



### Simple string key and values in dictionary

```csharp
// We need to create key/value pairs in map one by one
HashEntry one = new HashEntry("key-one", "value-one");
HashEntry two = new HashEntry("key-two", "value-two");

// Store entries as a hash map
await db.HashSetAsync("key-of-my-hash", new[] {one, two});

// Get entries from hash map key
var entries = await db.HashGetAllAsync("key-of-my-hash");

// Use .Name and .Value to read entries
foreach (var entry in entries){
  Console.WriteLine($"{entry.Name} = {entry.Value}");
}
```



### Saving a list as value in Map entry

```
```



### Using a HashMap for saving data

- Lets create a PersonRe