using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace ConfigurationManager.API.Cache
{
    public class CacheService: ICacheService
    {
        private IDatabase _database;
        private ISubscriber _publisher;
        private RedisChannel _invalidateChannel = new RedisChannel("invalidate_channel", RedisChannel.PatternMode.Literal);

        public CacheService(string redisConnectionString)
        {
            var options = ConfigurationOptions.Parse(redisConnectionString);

            var redisClient = ConnectionMultiplexer.Connect(options);

            _database = redisClient.GetDatabase();

            _publisher = redisClient.GetSubscriber();
        }

        public async Task InvalidateCacheAsync(string key)
        {
            var result = await _database.KeyDeleteAsync(key);

            if (result)
            {
                await _publisher.PublishAsync(_invalidateChannel, key);
            }
        }
    }
}
