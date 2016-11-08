using CachingSolutionsSamples.Cache;
using CachingSolutionsSamples.Policy;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CachingSolutionsSamples
{
	public class CategoriesManager
	{
		private ICacheable<Category> cache;
        private ICachePolicy policy;

        public CategoriesManager(ICacheable<Category> cache, ICachePolicy policy)
		{
			this.cache = cache;
            this.policy = policy;
		}

		public IEnumerable<Category> GetCategories()
		{
			Console.WriteLine("Get Categories");

			var user = Thread.CurrentPrincipal.Identity.Name;
			var categories = cache.Get(user);

			if (categories == null)
			{
				Console.WriteLine("From DB");

				using (var dbContext = new Northwind())
				{
					dbContext.Configuration.LazyLoadingEnabled = false;
					dbContext.Configuration.ProxyCreationEnabled = false;
					categories = dbContext.Categories.ToList();
					cache.Set(user, categories, policy);
				}
			}

			return categories;
		}
	}
}
