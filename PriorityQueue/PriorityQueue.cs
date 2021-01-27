using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Collections {
    /// <summary>
    /// MinHeap implementation of a priorityqueue.
    /// </summary>
    /// <typeparam name="T">Type of the container.</typeparam>
    public class PriorityQueue<T> {
        /// <summary>
        /// Returns the amount of items in the queue.
        /// </summary>
        public int Count => Heap.Count;
        /// <summary>
        /// Returns true if the queue is empty.
        /// </summary>
        public bool Empty => Heap.Count == 0;

        private List<T> Heap { get; set; }
        private Func<T, T, int> CompareFunction { get; set; } // -1 (<), 0 (==), +1 (>)



        /// <summary>
        /// Creates a new instance of type <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="compareFunc">Function to compare two values. The Function returns -1 for lower than, 0 for equality and 1 for greater than.</param>
        public PriorityQueue(Func<T, T, int> compareFunc) {
            Heap = new List<T>();
            CompareFunction = compareFunc;
        }
        /// <summary>
        /// Creates a new instance of type <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="compareFunc">Function to compare two values. The Function returns -1 for lower than, 0 for equality and 1 for greater than.</param>
        /// <param name="capacity">Initial capacity.</param>
        public PriorityQueue(Func<T, T, int> compareFunc, int capacity) {
            Heap = new List<T>(capacity);
            CompareFunction = compareFunc;
        }
        /// <summary>
        /// Creates a new instance of type <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="compareFunc">Function to compare two values. The Function returns -1 for lower than, 0 for equality and 1 for greater than.</param>
        /// <param name="collection">Initial entries in the queue.</param>
        public PriorityQueue(Func<T, T, int> compareFunc, IEnumerable<T> collection) {
            Heap = new List<T>(collection);
            CompareFunction = compareFunc;
            BuildHeap();
        }



        /// <summary>
        /// Adds an item to the queue.
        /// </summary>
        /// <param name="item">Object to add to the queue.</param>
        public void Enqueue(T item) {
            Heap.Add(item);
            PercUp(Heap.Count - 1);
        }
        /// <summary>
        /// Removes the first item in the queue and returns it.
        /// </summary>
        /// <returns>Popped entry of type <see cref="T"/></returns>
        public T Dequeue() {
            if (Empty) throw new OverflowException("Priority Queue is empty");

            T front = Heap[0];
            Heap[0] = Heap[Heap.Count - 1];
            Heap.RemoveAt(Heap.Count - 1);
            PercDown(0);

            return front;
        }
        /// <summary>
        /// Returns the first item in the queue.
        /// </summary>
        /// <returns>Entry of type <see cref="T"/></returns>
        public T Peek() {
            if (Empty) throw new OverflowException("Priority Queue is empty");

            return Heap[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => Heap.Clear();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => Heap.Contains(item);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array) => Heap.CopyTo(array);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex) => Heap.CopyTo(array, arrayIndex);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(int index, T[] array, int arrayIndex, int count) => Heap.CopyTo(index, array, arrayIndex, count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => Heap.ToArray();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrimExcess() => Heap.TrimExcess();


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PercDown(int index) {
            while (true) {
                int child = (index << 1) + 1;

                if (child >= Heap.Count) break;

                // Determine min from children
                if (child + 1 < Heap.Count && CompareFunction(Heap[child], Heap[child + 1]) > 0) {
                    child++;
                }

                if (CompareFunction(Heap[index], Heap[child]) <= 0) {
                    // parent <= min-child
                    break;
                }

                // parent > child => swap parent and min-child
                T temp = Heap[index];
                Heap[index] = Heap[child];
                Heap[child] = temp;


                index = child;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PercUp(int index) {
            while (index > 0) {
                int parent = (index - 1) >> 1;

                if (CompareFunction(Heap[index], Heap[parent]) >= 0) {
                    // parent >= index
                    break;
                }

                // parent < index
                T temp = Heap[parent];
                Heap[parent] = Heap[index];
                Heap[index] = temp;

                index = parent;
            }
        }
        private void BuildHeap() {
            if (Empty) return;

            for (int i = (Heap.Count - 2) >> 1; i >= 0; i--) {
                PercDown(i);
            }
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Heap.Count; i++) {
                builder.Append(Heap[i].ToString());

                if (i != Heap.Count - 1) builder.Append(", ");
            }

            return builder.ToString();
        }
        public override bool Equals(object obj) {
            return obj is PriorityQueue<T> queue &&
                   EqualityComparer<List<T>>.Default.Equals(Heap, queue.Heap) &&
                   EqualityComparer<Func<T, T, int>>.Default.Equals(CompareFunction, queue.CompareFunction);
        }
        public override int GetHashCode() {
            int hashCode = -147935277;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<T>>.Default.GetHashCode(Heap);
            hashCode = hashCode * -1521134295 + EqualityComparer<Func<T, T, int>>.Default.GetHashCode(CompareFunction);
            return hashCode;
        }


        // Testing Bottom-Up
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsConsistent() {
            if (Empty) return true;

            for (int i = (Heap.Count - 2) >> 1; i >= 0; i--) {
                int child = (i << 1) + 1;

                if (child < Heap.Count && CompareFunction(Heap[i], Heap[child]) > 0) return false;
                if (child + 1 < Heap.Count && CompareFunction(Heap[i], Heap[child + 1]) > 0) return false;
            }

            return true;
        }
        internal static void TestQueue(int N) {
            PriorityQueue<int> pq = new PriorityQueue<int>((i1, i2) => i1.CompareTo(i2));
            Random random = new Random();

            for (int i = 0; i < N; i++) {
                int operation = random.Next(2);
                int num = -1;

                if (operation == 0) { // Enqueue
                    num = random.Next(100_000);
                    pq.Enqueue(num);

                } else if (pq.Count > 0) {
                    num = pq.Dequeue();
                }

                if (!pq.IsConsistent()) {
                    Console.WriteLine($"Test failed at iteration { i } while {(operation == 0 ? "Enqueueing" : "Dequeueing")} { num }");
                }
            }
        }
    }
}
namespace System.Collections.Generic {
    /// <summary>
    /// MinHeap implementation of a priorityqueue.
    /// </summary>
    /// <typeparam name="T">Type of the container. Needs to implement <see cref="IComparable"/></typeparam>
    public class PriorityQueue<T> where T : IComparable<T> { // -1 (<), 0 (==), +1 (>)
        /// <summary>
        /// Returns the amount of items in the Queue.
        /// </summary>
        public int Count => Heap.Count;
        /// <summary>
        /// Returns true if the Queue is empty.
        /// </summary>
        public bool Empty => Heap.Count == 0;

        private List<T> Heap { get; set; }



        /// <summary>
        /// Creates a new instance of type <see cref="PriorityQueue{T}"/>.
        /// </summary>
        public PriorityQueue() {
            Heap = new List<T>();
        }
        /// <summary>
        /// Creates a new instance of type <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="capacity">Initial capacity.</param>
        public PriorityQueue(int capacity) {
            Heap = new List<T>(capacity);
        }
        /// <summary>
        /// Creates a new instance of type <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <param name="collection">Initial entries in the queue.</param>
        public PriorityQueue(IEnumerable<T> collection) {
            Heap = new List<T>(collection);
            BuildHeap();
        }



        /// <summary>
        /// Adds an item to the queue.
        /// </summary>
        /// <param name="item">Object to add to the queue.</param>
        public void Enqueue(T item) {
            Heap.Add(item);
            PercUp(Heap.Count - 1);
        }
        /// <summary>
        /// Removes the first item in the queue and returns it.
        /// </summary>
        /// <returns>Popped entry of type <see cref="T"/></returns>
        public T Dequeue() {
            if (Empty) throw new OverflowException("Priority Queue is empty");

            T front = Heap[0];
            Heap[0] = Heap[Heap.Count - 1];
            Heap.RemoveAt(Heap.Count - 1);
            PercDown(0);

            return front;
        }
        /// <summary>
        /// Returns the first item in the queue.
        /// </summary>
        /// <returns>Entry of type <see cref="T"/></returns>
        public T Peek() {
            if (Empty) throw new OverflowException("Priority Queue is empty");

            return Heap[0];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => Heap.Clear();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => Heap.Contains(item);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array) => Heap.CopyTo(array);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(T[] array, int arrayIndex) => Heap.CopyTo(array, arrayIndex);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(int index, T[] array, int arrayIndex, int count) => Heap.CopyTo(index, array, arrayIndex, count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T[] ToArray() => Heap.ToArray();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrimExcess() => Heap.TrimExcess();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PercDown(int index) {
            while (true) {
                int child = (index << 1) + 1;

                if (child >= Heap.Count) break;

                // Determine min from children
                if (child + 1 < Heap.Count && Heap[child].CompareTo(Heap[child + 1]) > 0) {
                    child++;
                }

                if (Heap[index].CompareTo(Heap[child]) <= 0) {
                    // parent <= min-child
                    break;
                }

                // parent > child => swap parent and min-child
                T temp = Heap[index];
                Heap[index] = Heap[child];
                Heap[child] = temp;


                index = child;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PercUp(int index) {
            while (index > 0) {
                int parent = (index - 1) >> 1;

                if (Heap[index].CompareTo(Heap[parent]) >= 0) {
                    // parent >= index
                    break;
                }

                // parent < index
                T temp = Heap[parent];
                Heap[parent] = Heap[index];
                Heap[index] = temp;

                index = parent;
            }
        }
        private void BuildHeap() {
            if (Empty) return;

            for (int i = (Heap.Count - 2) >> 1; i >= 0; i--) {
                PercDown(i);
            }
        }

        public override string ToString() {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < Heap.Count; i++) {
                builder.Append(Heap[i].ToString());

                if (i != Heap.Count - 1) builder.Append(", ");
            }

            return builder.ToString();
        }
        public override bool Equals(object obj) {
            return obj is PriorityQueue<T> queue &&
                   EqualityComparer<List<T>>.Default.Equals(Heap, queue.Heap);
        }
        public override int GetHashCode() {
            int hashCode = -147935277;
            hashCode = hashCode * -1521134295 + EqualityComparer<List<T>>.Default.GetHashCode(Heap);
            return hashCode;
        }


        // Testing Bottom-Up
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsConsistent() {
            if (Empty) return true;

            for (int i = (Heap.Count - 2) >> 1; i >= 0; i--) {
                int child = (i << 1) + 1;

                if (child < Heap.Count && Heap[i].CompareTo(Heap[child]) > 0) return false;
                if (child + 1 < Heap.Count && Heap[i].CompareTo(Heap[child + 1]) > 0) return false;
            }

            return true;
        }
        internal static void TestQueue(int N) {
            PriorityQueue<int> pq = new PriorityQueue<int>();
            Random random = new Random();

            for (int i = 0; i < N; i++) {
                int operation = random.Next(2);
                int num = -1;

                if (operation == 0) { // Enqueue
                    num = random.Next(100_000);
                    pq.Enqueue(num);

                } else if (pq.Count > 0) {
                    num = pq.Dequeue();
                }

                if (!pq.IsConsistent()) {
                    Console.WriteLine($"Test failed at iteration { i } while {(operation == 0 ? "Enqueueing" : "Dequeueing")} { num }");
                }
            }
        }
    }
}