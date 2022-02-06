using System;
using System.Threading;

namespace Assignment_1_5
{
    class Program
    {
        const int BUFFER_SIZE = 10;
        const int EXIT = -1;

        const int MIN_NUMBER = 1;
        const int MAX_NUMBER = 20;

        const int PRODUCER_DELAY = 500;
        const int CONSUMER_DELAY = 1000;

        static void Main()
        {
            ThreadSafeQueue<int> buffer = new ThreadSafeQueue<int>(BUFFER_SIZE, EXIT);

            Thread producer1 = new Thread(() => Produce(buffer, MIN_NUMBER, MAX_NUMBER, PRODUCER_DELAY));
 
            Thread consumer1 = new Thread(() => Consume(buffer, CONSUMER_DELAY));
            Thread consumer2 = new Thread(() => Consume(buffer, CONSUMER_DELAY));
            
            producer1.Start();
            consumer1.Start();
            consumer2.Start();

            producer1.Join();
            buffer.Add(buffer.EXIT);

            consumer1.Join();
            consumer2.Join();

            Console.WriteLine("All Done");
        }

        public static void Produce(ThreadSafeQueue<int> buffer, int minimumValue, int maximumValue, int delayInMilliSeconds)
        {
            for (int item = minimumValue; item <= maximumValue; item++)
            {
                buffer.Add(item);
                Thread.Sleep(delayInMilliSeconds);
            }
            
            Console.WriteLine("Producer Done");
        }

        public static void Consume(ThreadSafeQueue<int> buffer, int delayInMilliSeconds)
        {
            int number = buffer.Fetch();
            while (number != buffer.EXIT)
            {
                Console.WriteLine(number);
                Thread.Sleep(delayInMilliSeconds);

                number = buffer.Fetch();
            }

            buffer.Add(buffer.EXIT);

            Console.WriteLine("Consumer Done");
        }
    }
}

