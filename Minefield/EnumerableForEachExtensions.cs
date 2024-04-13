using System;
using System.Collections.Generic;

namespace Restall.Minefield
{
	public static class EnumerableForEachExtensions
	{
		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			if (items is null)
				throw new ArgumentNullException(nameof(items));

			if (action is null)
				throw new ArgumentNullException(nameof(action));

			foreach (var item in items)
				action(item);
		}
	}
}
