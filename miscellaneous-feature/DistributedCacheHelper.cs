using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Text.Json;

namespace miscellaneous_feature
{
    public static class DistributedCacheHelper
    {
        public static async Task SetRecordAsync<T>(string recordId, T data, TimeSpan? absoluteExpTime = null)
        {

            var jsonData = JsonSerializer.Serialize(data);
            await redis.GetDatabase().StringSetAsync(recordId, jsonData, absoluteExpTime ?? TimeSpan.FromSeconds(60));
        }

        public static async Task<T> GetRecordAsync<T>(string recordId)
        {
            RedisValue jsonData = await redis.GetDatabase().StringGetAsync(recordId);
            if (jsonData.IsNullOrEmpty)
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(jsonData);
        }

        public static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(new ConfigurationOptions { EndPoints = { "localhost:6379" } });
    }
}
