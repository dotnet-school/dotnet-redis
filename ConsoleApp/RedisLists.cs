using System;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ConsoleApp
{
    public static class RedisLists
    {
        public static async Task RunDemo(IDatabase db)
        {
            var keyForList = "my-list-key";

            // Delete if exists
            db.KeyDelete(keyForList);
            
            // List is created if does not exist
            await db.ListRightPushAsync(keyForList, "one", When.Always);
            
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
        }
    }
}