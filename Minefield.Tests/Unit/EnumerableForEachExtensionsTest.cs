using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using NSubstitute;
using Xunit;

namespace Restall.Minefield.Tests.Unit
{
	public class EnumerableForEachExtensionsTest
	{
		[Fact]
		public void ForEach_CalledWithNullItems_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			FluentActions
				.Invoking(() => ((IEnumerable<object>) null!).ForEach(_ => {}))
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("items");
		}

		[Fact]
		public void ForEach_CalledWithNullAction_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Generate.AnyNumberOf(() => new object())
				.Invoking(x => x.ForEach(null!))
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("action");
		}

		[Fact]
		public void ForEach_Called_ExpectActionIsCalledInOrderWithAllItems()
		{
			var action = Substitute.For<Action<int>>();
			var items = Generate.AnyNumberOf(IntegerGenerator.Any).ToArray();
			items.ForEach(action);
			Received.InOrder(() =>
			{
				foreach (var item in items)
					action(item);
			});
		}
	}
}
