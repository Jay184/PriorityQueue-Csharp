using System;
using System.Collections.Generic;

namespace PriorityQueueDemonstration {
    public class Program {
        public static void Main(string[] args) {
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

        }
    }

    public class Test : IComparable<Test> {
        public int Value { get; set; }

        public int CompareTo(Test other) {
            return Value.CompareTo(other);
        }
    }
}
