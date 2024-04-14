using System;
using FluentAssertions;
using Restall.Minefield.Game;
using Restall.Minefield.Game.MoveUp;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Game
{
	public class QuitGameKeypressToPlayerInputMapperTest
	{
		public class QuitKeys : TheoryData<ConsoleKeyInfo>
		{
			public QuitKeys()
			{
				this.AddRange(
					[
						.. AllModifierCombinationsFor('Q', shift: true),
						.. AllModifierCombinationsFor('q', shift: false),
						.. AllModifierCombinationsFor('q', shift: true),
						.. AllModifierCombinationsFor('Q', shift: false),
					]);
			}

			private static ConsoleKeyInfo[] AllModifierCombinationsFor(char ch, bool shift) => [
				new(ch, ConsoleKey.Q, shift, alt: false, control: false),
				new(ch, ConsoleKey.Q, shift, alt: false, control: true),
				new(ch, ConsoleKey.Q, shift, alt: true, control: false),
				new(ch, ConsoleKey.Q, shift, alt: true, control: true)];
		}

		[Fact]
		public void CanMap_CalledWithNonQuitKey_ExpectFalseIsReturned()
		{
			var nonQuitKey = AnyNonQuitKey();
			var mapper = new QuitGameKeypressToPlayerInputMapper();
			mapper.CanMap(nonQuitKey).Should().BeFalse();
		}

		private static ConsoleKeyInfo AnyNonQuitKey()
		{
			var key = ConsoleKeyInfoGenerator.Any();
			return key.Key != ConsoleKey.Q ? key : AnyNonQuitKey();
		}

		[Theory]
		[ClassData(typeof(QuitKeys))]
		public void CanMap_CalledWithQuitKey_ExpectTrueIsReturned(ConsoleKeyInfo quitKey)
		{
			var mapper = new QuitGameKeypressToPlayerInputMapper();
			mapper.CanMap(quitKey).Should().BeTrue();
		}

		[Fact]
		public void Map_CalledWithNonQuitKey_ExpectArgumentExceptionWithCorrectParamName()
		{
			var nonQuitKey = AnyNonQuitKey();
			var mapper = new QuitGameKeypressToPlayerInputMapper();
			mapper
				.Invoking(x => x.Map(nonQuitKey))
				.Should().Throw<ArgumentException>()
				.WithMessage("*CanMap*")
				.And.ParamName.Should().Be("unmapped");
		}

		[Theory]
		[ClassData(typeof(QuitKeys))]
		public void Map_CalledWithQuitKey_ExpectInstanceOfQuitGamePlayerInputIsReturned(ConsoleKeyInfo quitKey)
		{
			var mapper = new QuitGameKeypressToPlayerInputMapper();
			mapper.Map(quitKey).Should().BeOfType<QuitGamePlayerInput>();
		}
	}
}
