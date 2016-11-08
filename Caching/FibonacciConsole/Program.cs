using System;
using FibonacciCaching.Cache;
using FibonacciCaching.Fibonacci;

namespace FibonacciConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var cache = new FibonacciMemoryCache();
            //var cache = new FibonacciDistributedCache("localhost, allowAdmin=true"); // запустить Redis server в папке packages

            var fibonacci = new CachedFibonacci(cache);
            while (true)
            {
                Console.Write("Введите индекс: ");
                var index = int.Parse(Console.ReadLine());
                var item = fibonacci.GetItem(index);
                Console.WriteLine(item);
            }
        }
    }
}
