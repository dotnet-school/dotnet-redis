using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ConsoleApp
{
    public static class RedisHasMaps
    {
        public static async Task RunDemo(IDatabase db)
        {
            await SimpleStringMaps(db);
        }

        public static async Task SimpleStringMaps(IDatabase db)
        {
            // We need to create key/value pairs in map one by one
            var name = new HashEntry("name", "Person 1");
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
        }
    }
}