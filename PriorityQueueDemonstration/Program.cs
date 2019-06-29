using System;
using System.Collections.Generic;

namespace PriorityQueueDemonstration {
    class Program {
        static void Main(string[] args) => BasicUsage();


        static void BasicUsage() {
            PriorityQueue<int> pq = new PriorityQueue<int>();
            int N = 20;

            for (int i = 0; i < N; i++) {
                int num;

                if (i % 2 == 0) num = i;
                else num = N - i;

                pq.Enqueue(num);
                Console.WriteLine($"{ num }\t=>\tPQ [{ pq.ToString() }]");
            }


            Console.WriteLine();


            while (!pq.Empty) {
                int min = pq.Dequeue();
                Console.WriteLine($"{ min }\t<=\tPQ [{ pq.ToString() }]");
            }

            Console.ReadKey();
        }
    }
}
