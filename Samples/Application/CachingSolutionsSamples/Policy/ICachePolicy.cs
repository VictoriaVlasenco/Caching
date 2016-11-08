using System;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Policy
{
    public interface ICachePolicy
    {
        TimeSpan GetExpirationTime();

        CacheItemPolicy GetMonitorCachePolicy();
    }
}
