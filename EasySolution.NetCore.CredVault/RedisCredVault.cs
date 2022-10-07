using StackExchange.Redis;

namespace EasySolution.NetCore.CredVault
{
    public class RedisCredVault
    {
        ConnectionMultiplexer redis;
        IDatabase _db;
        public static RedisCredVault Localhost => new RedisCredVault("localhost", 6379);
        private RedisCredVault(string host, int port, string? user = null, string? pass = null)
        {
            string configStr = $"{host}:{port}";
            redis = ConnectionMultiplexer.Connect(configStr);
            _db = redis.GetDatabase();
        }

        public string? GetString(string key, string? defaultValue = null)
        {
            if (_db.KeyExists(key)) 
                return _db.StringGet(key);

            return defaultValue;
        }
    }
}