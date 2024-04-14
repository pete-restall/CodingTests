using System;
using FluentAssertions;
using Restall.Minefield.Game.MoveUp;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Game.MoveUp
{
	public class MoveUpKeypressToPlayerInputMapperTest
	{
		private static readonly ConsoleKeyInfo MoveUpKey = new('?', ConsoleKey.UpArrow, shift: false, alt: false, control: false);

		public class NonMoveUpKeys : TheoryData<ConsoleKeyInfo>
		{
			public NonMoveUpKeys()
			{
				this.AddRange(
					[
						new('?', ConsoleKey.UpArrow, shift: false, alt: false, control: true),
						new('?', ConsoleKey.UpArrow, shift: false, alt: true, control: false),
						new('?', ConsoleKey.UpArrow, shift: false, alt: true, control: true),

						new('?', ConsoleKey.UpArrow, shift: true, alt: false, control: false),
						new('?', ConsoleKey.UpArrow, shift: true, alt: false, control: true),
						new('?', ConsoleKey.UpArrow, shift: true, alt: true, control: false),
						new('?', ConsoleKey.UpArrow, shift: true, alt: true, control: true)
					]);
			}
		}

		[Theory]
		[ClassData(typeof(NonMoveUpKeys))]
		public void CanMap_CalledWithNonMoveUpKey_ExpectFalseIsReturned(ConsoleKeyInfo nonMoveUpKey)
		{
			var mapper = new MoveUpKeypressToPlayerInputMapper();
			mapper.CanMap(nonMoveUpKey).Should().BeFalse();
		}

		[Fact]
		public void CanMap_CalledWithMoveUpKey_ExpectTrueIsReturned()
		{
			var mapper = new MoveUpKeypressToPlayerInputMapper();
			mapper.CanMap(MoveUpKey).Should().BeTrue();
		}

		[Theory]
		[ClassData(typeof(NonMoveUpKeys))]
		public void Map_CalledWithNonMoveUpKey_ExpectArgumentExceptionWithCorrectParamName(ConsoleKeyInfo nonMoveUpKey)
		{
			var mapper = new MoveUpKeypressToPlayerInputMapper();
			mapper
				.Invoking(x => x.Map(nonMoveUpKey))
				.Should().Throw<ArgumentException>()
				.WithMessage("*CanMap*")
				.And.ParamName.Should().Be("unmapped");
		}

		[Fact]
		public void Map_CalledWithMoveUpKey_ExpectInstanceOfMoveUpPlayerInputIsReturned()
		{
			var mapper = new MoveUpKeypressToPlayerInputMapper();
			mapper.Map(MoveUpKey).Should().BeOfType<MoveUpPlayerInput>();
		}
	}
}
