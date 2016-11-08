namespace FibonacciCaching.Cache
{
    public abstract class Cacheable<T, TResult>
    {
        protected string Prefix { get; set; }

        public abstract TResult this[T index] { get; set; }

        public virtual string GetKey(string key)
        {
            return string.IsNullOrEmpty(Prefix) ? key : Prefix  + key;
        }
    }
}
