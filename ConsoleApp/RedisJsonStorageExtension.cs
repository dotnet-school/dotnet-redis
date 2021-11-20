using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ConsoleApp
{
  public static class RedisJsonStorageExtension
  {
    public static Task SaveObjectAsync<T>(this IDatabase db, string key, T data)
    {
      var json = JsonSerializer.Serialize(data);
      return db.StringSetAsync(key, json);
    }
    
    public static async Task<T?> GetObjectAsync<T>(this IDatabase db, string key)
    {
      var json = await db.StringGetAsync(key);
      if (json.HasValue)
      {
        return JsonSerializer.Deserialize<T>(json.ToString());
      }

      return default(T);
    }
  }
}