using System;
using System.Collections.Generic;
using CachingSolutionsSamples.Policy;
using NorthwindLibrary;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Cache
{
    public class OrdersMemoryCache : ICacheable<Order>
    {
        ObjectCache cache = MemoryCache.Default;
        string prefix = "Cache_Orders";

        public IEnumerable<Order> Get(string forUser)
        {
            return (IEnumerable<Order>)cache.Get(prefix + forUser);
        }

        public void Set(string forUser, IEnumerable<Order> categories, ICachePolicy policy)
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
