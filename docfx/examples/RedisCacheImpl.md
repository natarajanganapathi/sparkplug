# Redis IDistributedCache Implementation Examle

```C#
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace SparkPlug.Caching
{
    public class RedisCache : IDistributedCache
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisCache(ConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public byte[] Get(string key)
        {
            var db = _redis.GetDatabase();
            return db.StringGet(key);
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            var db = _redis.GetDatabase();
            return db.StringGetAsync(key, token);
        }

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            var db = _redis.GetDatabase();
            var expiry = options.AbsoluteExpirationRelativeToNow;
            db.StringSet(key, value, expiry);
        }

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var db = _redis.GetDatabase();
            var expiry = options.AbsoluteExpirationRelativeToNow;
            return db.StringSetAsync(key, value, expiry, token);
        }

        public void Refresh(string key)
        {
            var db = _redis.GetDatabase();
            var value = db.StringGet(key);
            if (value.HasValue)
            {
                db.KeyExpire(key, TimeSpan.FromSeconds(30));
            }
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            var db = _redis.GetDatabase();
            var value = db.StringGet(key);
            if (value.HasValue)
            {
                return db.KeyExpireAsync(key, TimeSpan.FromSeconds(30), token);
            }

            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            var db = _redis.GetDatabase();
            db.KeyDelete(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            var db = _redis.GetDatabase();
            return db.KeyDeleteAsync(key, token);
        }
    }
}

```