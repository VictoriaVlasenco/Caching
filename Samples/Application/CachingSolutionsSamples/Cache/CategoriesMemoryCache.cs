using System.Collections.Generic;
using NorthwindLibrary;
using System.Runtime.Caching;
using CachingSolutionsSamples.Policy;

namespace CachingSolutionsSamples.Cache
{
	internal class CategoriesMemoryCache : ICacheable<Category>
	{
		ObjectCache cache = MemoryCache.Default;
		string prefix  = "Cache_Categories";

		public IEnumerable<Category> Get(string forUser)
		{
			return (IEnumerable<Category>) cache.Get(prefix + forUser);
		}

		public void Set(string forUser, IEnumerable<Category> categories, ICachePolicy policy)
		{
            if (policy == null)
            {
                cache.Set(prefix + forUser, categories, ObjectCache.InfiniteAbsoluteExpiration);
            }
            else
            {
                cache.Set(prefix + forUser, categories, policy.GetMonitorCachePolicy());
            }
        }
	}
}
