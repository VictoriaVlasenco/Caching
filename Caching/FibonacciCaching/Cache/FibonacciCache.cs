using System;
using System.IO;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using StackExchange.Redis;

namespace FibonacciCaching.Cache
{
    public class FibonacciMemoryCache : Cacheable<string, int?>
    {
        public FibonacciMemoryCache()
        {
            Prefix = "fibonacci_";
        }

        public override int? this[string key]
        {
            get
            {
                ObjectCache cache = MemoryCache.Default;
                return cache.Get(GetKey(key)) as int?;
            }

            set
            {
                if (!value.HasValue)
                {
                    throw new ArgumentNullException(); 
                } 

                ObjectCache cache = MemoryCache.Default;
                cache.Set(GetKey(key), value, ObjectCache.InfiniteAbsoluteExpiration);
            }
        }
    }

    public class FibonacciDistributedCache : Cacheable<string, int?>
    {
        private ConnectionMultiplexer redis;
        private DataContractSerializer serializer;

        public FibonacciDistributedCache(string coniguration)
        {
            Prefix = "fibonacci_";

            redis = ConnectionMultiplexer.Connect(coniguration);
            serializer = new DataContractSerializer(typeof(int));
        }

        public override int? this[string key]
        {
            get
            {
                var db = redis.GetDatabase();
                var item = db.StringGet(GetKey(key));
                if (!item.HasValue)
                {
                    return null;
                }

                return (int)serializer.ReadObject(new MemoryStream(item));
            }

            set
            {
                if (!value.HasValue)
                {
                    throw new ArgumentNullException();
                }

                var db = redis.GetDatabase();
                var stream = new MemoryStream();
                serializer.WriteObject(stream, value);
                db.StringSet(GetKey(key), stream.ToArray());
            }
        }
    }
}
