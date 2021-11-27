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
            HashEntry one = new HashEntry("key-one", "value-one");
            HashEntry two = new HashEntry("key-two", "value-two");
            await db.HashSetAsync("key-of-my-hash", new[] {one, two});
            var entries = await db.HashGetAllAsync("key-of-my-hash");
            foreach (var entry in entries)
            {
                Console.WriteLine($"{entry.Name} = {entry.Value}");
            }
        }
    }
}