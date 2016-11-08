using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using CachingSolutionsSamples.Policy;
using CachingSolutionsSamples.Cache;
using System.Configuration;
using System.Data.SqlClient;

namespace CachingSolutionsSamples
{
	[TestClass]
	public class CacheTests
	{
        private static string connection;

        [ClassInitialize]
        public static void SetUp(TestContext context)
        {
            connection = ConfigurationManager.ConnectionStrings["NorthwindDB"].ConnectionString;
            SqlDependency.Start(connection);
        }

		[TestMethod]
		public void MemoryCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesMemoryCache(), null);

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

        [TestMethod]
        public void MemoryCacheWithCachePolicy()
        {
            var categoryManager = new CategoriesManager(new CategoriesMemoryCache(), new CategoryCachePolicy());

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(categoryManager.GetCategories().Count());
                Thread.Sleep(100);
            }
        }

        [TestMethod]
		public void RedisCache()
		{
			var categoryManager = new CategoriesManager(new CategoriesRedisCache("localhost, allowAdmin=true"), new CategoryCachePolicy(2));

			for (var i = 0; i < 10; i++)
			{
				Console.WriteLine(categoryManager.GetCategories().Count());
				Thread.Sleep(100);
			}
		}

        [TestMethod]
        public void OrdersMemoryCache()
        {
            var ordersManager = new OrdersManager(new OrdersMemoryCache(), new OrderCachePolicy());

            for (var i = 0; i < 10; i++)
            {
                Console.WriteLine(ordersManager.GetOrders().Count());
                Thread.Sleep(100);
            }
        }

        [ClassCleanup]
        public static void TearDown()
        {
            SqlDependency.Stop(connection);
        }
    }
}
