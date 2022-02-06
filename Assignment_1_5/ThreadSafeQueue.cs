using System;
using System.Threading;

namespace Assignment_1_5
{
    public class ThreadSafeQueue<T>
    {
        private readonly T[] buffer;
        private readonly int maxSize;
        private int first;
        private int last;

        private readonly SemaphoreSlim bufferFull;
        private readonly SemaphoreSlim bufferEmpty;

        public T EXIT { get; private set; }
        public int Count { get; private set; }

        public ThreadSafeQueue(int size, T exitValue)
        {
            buffer = new T[maxSize = size];
            Count = first = last = 0;
            EXIT = exitValue;
            bufferFull = new SemaphoreSlim(maxSize, maxSize);
            bufferEmpty = new SemaphoreSlim(0, maxSize);
        }

        public void Add(T item)
        {
            bufferFull.Wait();
            lock (this)
            {
                buffer[first++] = item;
                first %= maxSize;
                Count++;
            }
            bufferEmpty.Release();
        }

        public T Fetch()
        {
            bufferEmpty.Wait();
            T item = default(T);
            lock (this)
            {
                item = buffer[last++];
                last %= maxSize;
                Count--;
            }
            bufferFull.Release();
            return item;
        }
    }
}
