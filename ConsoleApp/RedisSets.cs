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
            var onePlusTwoKey = new RedisKey("set-one-plus-two");
            var oneUnionTwo = new RedisKey("set-one-union-two");

            // Delete if it exists
            await db.KeyDeleteAsync(new []{ setOneKey, setTwoKey, setThreeKey, onePlusTwoKey, oneUnionTwo });

            var setOneItems = new[] {"one", "one-two", "one-two-three"};
            var setTwoItems = new[] {"two", "one-two", "one-two-three"};
            var setThreeItems = new[] {"three", "x", "one-two-three"};
            var toRedisValues = new Func<string, RedisValue>(s => new RedisValue(s));

            await db.SetAddAsync(setOneKey, setOneItems.Select(toRedisValues).ToArray());
            await db.SetAddAsync(setTwoKey, setTwoItems.Select(toRedisValues).ToArray());
            await db.SetAddAsync(setThreeKey, setThreeItems.Select(toRedisValues).ToArray());
            
            // SMOVE: Move from a set to another (if found in original set)
            await db.SetMoveAsync(setThreeKey, setOneKey, "x");
            
            // SMOVE : Does nothing as setThree dot have "x" any more
            await db.SetMoveAsync(setThreeKey, setOneKey, "x");
           
            // SMOVE : Does nothing as setThree dot have "x" any more
            await db.SetMoveAsync(setThreeKey, setOneKey, "x");

            // SMEMBERS: Get all values in a set
            var values = await db.SetMembersAsync(setOneKey);
            Console.WriteLine($"items in one : {string.Join(", ", values.Select(v => v.ToString()))}");
            
            // SPOP : Remove and get a random member from set
            var popped = await db.SetPopAsync(setOneKey);
            Console.WriteLine($"Popped from one : {popped}");
            
            // Subtract two from one and store result in new SORTED SET
            await db.SortedSetCombineAndStoreAsync(SetOperation.Union, onePlusTwoKey, setOneKey, setTwoKey);
            var result = await db.SortedSetRangeByValueAsync(onePlusTwoKey);
            Console.WriteLine($"one union two : {string.Join(", ", result.Select(v => v.ToString()))}");
            
            await db.SortedSetCombineAndStoreAsync(SetOperation.Intersect, oneUnionTwo, setOneKey, setTwoKey);
            // var result = await db.SortedSetRangeByValueAsync("set-one-plus-two");
            // Console.WriteLine($"items in set-one-plus-two : {string.Join(", ", result.Select(v => v.ToString()))}");

            
        }
    }
}