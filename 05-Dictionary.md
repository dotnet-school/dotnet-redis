# HashMaps

HasMap in Redis are useful to create a logical group of key-value pair. 

For e.g. when we stored person value as Json String, it works when we only need to either save or get the object. But what if we need to store some object whose fields can be written in parallel by different programs ? 

**Use dictionary when you need to store objects whose fields can be updated concurrently**

Though remember an important limitation **hasmaps cannot store lists or other data structures in it**

### Storing object in HashMaps

```csharp
// We need to create key/value pairs in map one by one
var name   = new HashEntry("name", "Person 1");
var salary = new HashEntry("salary", "23100.34m");
var isMale = new HashEntry("IsMale", "true");

var savedEntries = new[] {name, salary, isMale};

// Store entries as a hash map
await db.HashSetAsync("person-1", savedEntries);

// Get entries from hash map key
var entries = await db.HashGetAllAsync("person-1");

// Use .Name and .Value to read entries
foreach (var entry in entries) {
  Console.WriteLine($"{entry.Name} = {entry.Value}");
}
```

Output: 

> name = Person 1
> salary = 23100.34m
> IsMale = true

