using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorthwindLibrary;
using StackExchange.Redis;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Caching;
using CachingSolutionsSamples.Policy;

namespace CachingSolutionsSamples.Cache
{
	class CategoriesRedisCache : ICacheable<Category>
	{
		private ConnectionMultiplexer redisConnection;
		string prefix = "Cache_Categories";
		DataContractSerializer serializer = new DataContractSerializer(
			typeof(IEnumerable<Category>));

		public CategoriesRedisCache(string hostName)
		{
			redisConnection = ConnectionMultiplexer.Connect(hostName);
		}

		public IEnumerable<Category> Get(string forUser)
		{
			var db = redisConnection.GetDatabase();
			byte[] s = db.StringGet(prefix + forUser);
			if (s == null)
				return null;

			return (IEnumerable<Category>)serializer
				.ReadObject(new MemoryStream(s));
		}

		public void Set(string forUser, IEnumerable<Category> categories, ICachePolicy policy)
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
