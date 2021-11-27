using System;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ConsoleApp
{
    public class RedisSets
    {
        public static async Task RunDemo(IDatabase db)
        {
            // await AddRemoveFromSet(db);
            await MultipleSetDemo(db);
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
        
        private static async Task MultipleSetDemo(IDatabase db)
        {
            var setOneKey   = new RedisKey("key-of-my-set-one");
            var setTwoKey   = new RedisKey("key-of-my-set-two");
            var setThreeKey = new RedisKey("key-of-my-set-three");

            // Delete if it exists
            await db.KeyDeleteAsync(new []{ setOneKey, setTwoKey, setThreeKey });

            var setOneItems = new[] {"one", "one-two", "one-two-three"};
            var setTwoItems = new[] {"two", "one-two", "one-two-three"};
            var setThreeItems = new[] {"three", "one-two-three"};
            var toRedisValues = new Func<string, RedisValue>(s => new RedisValue(s));

            await db.SetAddAsync(setOneKey, setOneItems.Select(toRedisValues).ToArray());
            await db.SetAddAsync(setTwoKey, setTwoItems.Select(toRedisValues).ToArray());
            await db.SetAddAsync(setThreeKey, setThreeItems.Select(toRedisValues).ToArray());
            
            
        }
    }
}