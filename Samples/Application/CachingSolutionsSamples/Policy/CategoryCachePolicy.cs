using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Caching;

namespace CachingSolutionsSamples.Policy
{
    public class CategoryCachePolicy : ICachePolicy
    {
        public double ExpirationTimeInDays { get; set; }

        public CategoryCachePolicy(double expirationTimeInDays = 0)
        {
            ExpirationTimeInDays = expirationTimeInDays;
        }

        public CacheItemPolicy GetMonitorCachePolicy()
        {
            var policy = new CacheItemPolicy();
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["NorthwindDB"].ConnectionString))
            {
                using (var command = new SqlCommand("SELECT [CategoryID], [CategoryName], [Description] FROM [NorthwindEF].[Northwind].[Categories]", conn))
                {
                    command.CommandTimeout = System.Int32.MaxValue;
                    var dependency = new SqlDependency(command);
                    conn.Open();
                    command.ExecuteNonQuery();
                    policy.ChangeMonitors.Add(new SqlChangeMonitor(dependency));
                }
            }

            return policy;
        }

        public TimeSpan GetExpirationTime()
        {
            return TimeSpan.FromDays(ExpirationTimeInDays);
        }
    }
}
