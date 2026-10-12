namespace Hirealdoor.Services;

public interface IRedisService
{
    Task SetValueAsync(string key, dynamic value, TimeSpan? expiration = null);
    void SetValue(string key, dynamic value, TimeSpan? expiration = null);

    Task<T?> GetValueAsync<T>(string key);
    T? GetValue<T>(string key);

    Dictionary<string, string> KeyValues(string pattern);

    bool KeyExists(string key);

    void DeleteKeys(string[] keys);

    bool DeleteKey(string key);
}