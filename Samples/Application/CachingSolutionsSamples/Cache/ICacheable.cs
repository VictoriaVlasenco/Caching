using CachingSolutionsSamples.Policy;
using System.Collections.Generic;

namespace CachingSolutionsSamples.Cache
{
	public interface ICacheable<T>
	{
		IEnumerable<T> Get(string forUser);
		void Set(string forUser, IEnumerable<T> categories, ICachePolicy policy);
	}
}
