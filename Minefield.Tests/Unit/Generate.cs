using System;
using System.Linq;
using Lophtware.Testing.Utilities;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;

namespace Restall.Minefield.Tests.Unit
{
	public static class Generate
	{
		public static T[] AtLeastOne<T>(Func<T> generator) => AtLeast(1, generator);

		public static T[] AtLeast<T>(int minimumCount, Func<T> generator) => IntegerGenerator
			.WithinInclusiveRange(minimumCount, minimumCount + 10)
			.Select(generator)
			.ToArray();

		public static T[] AnyNumberOf<T>(Func<T> generator) => AtLeast(0, generator);
	}
}
