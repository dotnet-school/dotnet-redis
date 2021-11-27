using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ConsoleApp
{
    public static class JsonObjectsInRedis
    {
        public static async Task RunDemo(IDatabase db)
        {
            await SaveAndGetObject(db);
        }
        public static async Task SaveAndGetObject(IDatabase db)
        {
            var person1 = Person.Create("Person 1");
            var person2 = Person.Create("Person 2", new []{"red", "blue"}, new []{"Delhi", "Mumbai"});
            var person3 = Person.Create("Person 3", new []{"red", "blue"}, new []{"Mumbai", "Delhi"});
            var person4 = Person.Create("Person 3");
 
            await db.SaveObjectAsync("one", person1);
            await db.SaveObjectAsync("two", person2);
            await db.SaveObjectAsync("thee", person3);
            await db.SaveObjectAsync("four", person4);

            var person1Read = await db.GetObjectAsync<Person>("one");
            var person2Read = await db.GetObjectAsync<Person>("two");
            var person3Read = await db.GetObjectAsync<Person>("thee");
            var person4Read = await db.GetObjectAsync<Person>("four");
            var nonExisting = await db.GetObjectAsync<Person>("unknown-key");

            var allValuesEqual =
                person1.Equals(person1Read) &&
                person2.Equals(person2Read) &&
                person3.Equals(person3Read) &&
                person4.Equals(person4Read) &&
                nonExisting == null;
      
            Console.WriteLine(
                allValuesEqual ? "Successfully handled objects" : "Failed to correctly handle objects");
        }
    }
}