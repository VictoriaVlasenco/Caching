using CachingSolutionsSamples.Cache;
using CachingSolutionsSamples.Policy;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CachingSolutionsSamples
{
    public class OrdersManager
    {
        private ICacheable<Order> cache;
        private ICachePolicy policy;

        public OrdersManager(ICacheable<Order> cache, ICachePolicy policy)
        {
            this.cache = cache;
            this.policy = policy;
        }

        public IEnumerable<Order> GetOrders()
        {
            Console.WriteLine("Get Orders");

            var user = Thread.CurrentPrincipal.Identity.Name;
            var orders = cache.Get(user);

            if (orders == null)
            {
                Console.WriteLine("From DB");

                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    orders = dbContext.Orders.ToList();
                    cache.Set(user, orders, policy);
                }
            }

            return orders;
        }
    }
}
