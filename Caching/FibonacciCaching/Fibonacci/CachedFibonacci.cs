using System;
using FibonacciCaching.Cache;

namespace FibonacciCaching.Fibonacci
{
    public class CachedFibonacci
    {
        private readonly Cacheable<string, int?> cache;

        public CachedFibonacci(Cacheable<string, int?> cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            this.cache = cache;
        }

        public int GetItem(int index)
        {
            var item = cache[index.ToString()];
            if (item.HasValue)
            {
                return item.Value;
            }

            item = Fibonacci.GetItem(index);
            cache[index.ToString()] = item;

            return item.Value;
        }
    }
}
