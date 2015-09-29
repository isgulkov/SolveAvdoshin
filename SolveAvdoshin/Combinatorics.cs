using System;
using System.Collections.Generic;

namespace SolveAvdoshin
{
	public static class Combinatorics
	{
		public static IEnumerable<T[]> AllNTuples<T>(T[] items, int size)
		{
			if(size == 0) {
				yield return new T[] { };
			}
			else {
				foreach(var tuple in AllNTuples<T>(items, size - 1)) {
					foreach(var item in items) {
						T[] newTuple = new T[tuple.Length + 1];

						newTuple[0] = item;
						tuple.CopyTo(newTuple, 1);

						yield return newTuple;

					}
				}
			}


		}
	}
}

