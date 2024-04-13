using System.Collections;
using System.Collections.Generic;
using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class EnumerableTestDoubles
	{
		public static IEnumerable<T> MockFor<T>(params T[] items)
		{
			var enumerable = Substitute.For<IEnumerable<T>>();
			enumerable.GetEnumerator().Returns(_ => ((IEnumerable<T>) items).GetEnumerator());
			((IEnumerable) enumerable).GetEnumerator().Returns(_ => enumerable.GetEnumerator());
			return enumerable;
		}
	}
}
