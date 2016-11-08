namespace FibonacciCaching.Fibonacci
{
    public static class Fibonacci
    {
        public static int GetItem(int index)
        {
            if (index == 0)
            {
                return 0;
            }

            if (index == 1 || index == 2)
            {
                return 1;
            }

            var previous = 1;
            var current = 1;
            for (int i = 2; i < index; i++)
            {
                var next = previous + current;
                previous = current;
                current = next;
            }

            return current;
        }
    }
}
