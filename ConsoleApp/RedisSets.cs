using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ConsoleApp
{
    public class RedisSets
    {
        public static async Task RunDemo(IDatabase db)
        {
            await AddRemoveFromSet(db);
        }

        private static async Task AddRemoveFromSet(IDatabase db)
        {
            var myKey = "key-of-my-set";

            // Delete if it exists
            await db.KeyDeleteAsync(myKey);

            // SADD : Add to sets
            await db.SetAddAsync(myKey, "zero");
            await db.SetAddAsync(myKey, "one");
            await db.SetAddAsync(myKey, "two");
            await db.SetAddAsync(myKey, "three");

            // SCARD : get number of items in set
            var count = await db.SetLengthAsync(myKey);
            Console.WriteLine($"Count : {count}");

            // SPOP : get a random member from set
            var popped = await db.SetPopAsync(myKey);
            Console.WriteLine($"Popped : {popped}");
        }
    }
}