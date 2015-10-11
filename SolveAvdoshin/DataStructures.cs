using System;
using System.Collections.Generic;

namespace SolveAvdoshin
{
	public class ImprovisedPriorityQueue<T>
	{
		public int Capacity, Count;
		Queue<T>[] Queues;

		public ImprovisedPriorityQueue(int capacity)
		{
			Capacity = capacity;
			Count = 0;

			Queues = new Queue<T>[capacity + 1];

			for(int i = 1; i <= capacity; i++) {
				Queues[i] = new Queue<T>();
			}
		}

		public bool TryEnqueue(T item, int priority)
		{
			if(priority < 1 || priority > Capacity)
				return false;

			Count += 1;
			Queues[priority].Enqueue(item);
			return true;
		}

		public T Dequeue()
		{
			if(Count == 0)
				throw new Exception("ImprovisedPriorityQueue is empty");

			for(int i = 1; i <= Capacity; i++) {
				if(Queues[i].Count != 0) {
					Count -= 1;
					return Queues[i].Dequeue();
				}
			}

			throw new Exception("ImprovisedPriorityQueue is empty");
		}
	}
}

