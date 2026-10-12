using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Hirealdoor.Services.Service;

public class RedisService(
    string connectionString,
    string prefix
) : IRedisService
{
    private readonly JsonSerializerOptions _options = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
    };

    private readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect(connectionString);

    public async Task SetValueAsync(string key, dynamic value, TimeSpan? expiration = null)
    {
        var db = _redis.GetDatabase();
        var serializedValue = JsonSerializer.Serialize(value, _options);
        await db.StringSetAsync($"{prefix}:{key}", serializedValue, expiration);
    }

    public void SetValue(string key, dynamic value, TimeSpan? expiration = null)
    {
        var db = _redis.GetDatabase();
        var serializedValue = JsonSerializer.Serialize(value, _options);
        db.StringSet($"{prefix}:{key}", serializedValue, expiration);
    }

    public async Task<T?> GetValueAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var serializedValue = await db.StringGetAsync($"{prefix}:{key}");
        return string.IsNullOrEmpty(serializedValue)
            ? default
            : JsonSerializer.Deserialize<T>(serializedValue.ToString(), _options);
    }

    public T? GetValue<T>(string key)
    {
        var db = _redis.GetDatabase();
        var serializedValue = db.StringGet($"{prefix}:{key}");
        return string.IsNullOrEmpty(serializedValue)
            ? default
            : JsonSerializer.Deserialize<T>(serializedValue.ToString(), _options);
    }

    public bool KeyExists(string key)
    {
        var db = _redis.GetDatabase();
        return db.KeyExists($"{prefix}{key}");
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.KeyExistsAsync($"{prefix}{key}");
    }

    public async Task<bool> DeleteKeyAsync(string key)
    {
        var db = _redis.GetDatabase();
        return await db.KeyDeleteAsync($"{prefix}{key}");
    }


    public bool DeleteKey(string key)
    {
        var db = _redis.GetDatabase();
        return db.KeyDelete($"{prefix}{key}");
    }


    public async Task DeleteKeysAsync(string[] keys)
    {
        if (keys.Length > 0)
        {
            var db = _redis.GetDatabase();
            var redisKeys = Array.ConvertAll(keys, key => (RedisKey)key);
            await db.KeyDeleteAsync(redisKeys);
        }
    }


    public void DeleteKeys(string[] keys)
    {
        if (keys.Length <= 0) return;
        var db = _redis.GetDatabase();
        var redisKeys = Array.ConvertAll(keys, key => (RedisKey)key);
        db.KeyDelete(redisKeys);
    }

    public Dictionary<string, string> KeyValues(string pattern)
    {
        var db = _redis.GetDatabase();
        var server = _redis.GetServer(db.Multiplexer.GetEndPoints()[0]);
        var keys = server.Keys(-1, $"{prefix}{pattern}");

        var results = new Dictionary<string, string>();

        foreach (var key in keys)
        {
            var value = db.StringGet(key);
            results.Add(key!, value!);
        }

        return results;
    }
}