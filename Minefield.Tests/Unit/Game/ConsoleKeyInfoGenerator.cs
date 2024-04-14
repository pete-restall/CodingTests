using System;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;

namespace Restall.Minefield.Tests.Unit.Game
{
	public static class ConsoleKeyInfoGenerator
	{
		public static ConsoleKeyInfo Any() => new(
			(char) IntegerGenerator.WithinInclusiveRange(0, 255),
			EnumGenerator.AnyDefined<ConsoleKey>(),
			shift: BooleanGenerator.Any(),
			alt: BooleanGenerator.Any(),
			control: BooleanGenerator.Any());
	}
}
