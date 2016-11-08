using NorthwindLibrary;
using System;
using System.Collections.Generic;
using CachingSolutionsSamples.Policy;
using StackExchange.Redis;
using System.Runtime.Serialization;
using System.IO;

namespace CachingSolutionsSamples.Cache
{
    public class OrdersRedisCache : ICacheable<NorthwindLibrary.Order>
    {
        private ConnectionMultiplexer redisConnection;
        string prefix = "Cache_Categories";
        DataContractSerializer serializer = new DataContractSerializer(
            typeof(IEnumerable<Category>));

        public OrdersRedisCache(string hostName)
        {
            redisConnection = ConnectionMultiplexer.Connect(hostName);
        }

        public IEnumerable<NorthwindLibrary.Order> Get(string forUser)
        {
            var db = redisConnection.GetDatabase();
            byte[] s = db.StringGet(prefix + forUser);
            if (s == null)
                return null;

            return (IEnumerable<NorthwindLibrary.Order>)serializer
                .ReadObject(new MemoryStream(s));
        }

        public void Set(string forUser, IEnumerable<NorthwindLibrary.Order> categories, ICachePolicy policy)
        {
            var db = redisConnection.GetDatabase();
            var key = prefix + forUser;

            if (categories == null)
            {
                db.StringSet(key, RedisValue.Null);
            }
            else
            {
                var stream = new MemoryStream();
                serializer.WriteObject(stream, categories);
                if (policy == null)
                {
                    db.StringSet(key, stream.ToArray());
                }
                else
                {
                    db.StringSet(key, stream.ToArray(), policy.GetExpirationTime());
                }
            }
        }
    }
}
