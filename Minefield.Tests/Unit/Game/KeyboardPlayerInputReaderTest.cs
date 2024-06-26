using System;
using FluentAssertions;
using Restall.Minefield.Game;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Game
{
	public class KeyboardPlayerInputReaderTest
	{
		[Fact]
		public void Constructor_CalledWithNullReadKey_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new KeyboardPlayerInputReader(null!, DummyKeyToPlayerInputMapper());
			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("readKey");
		}

		private static IMap<ConsoleKeyInfo, IPlayerInput> DummyKeyToPlayerInputMapper() => MapTestDoubles.Dummy<ConsoleKeyInfo, IPlayerInput>();

		[Fact]
		public void Constructor_CalledWithNullKeyToPlayerInputMapper_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new KeyboardPlayerInputReader(DummyReadKey, null!);
			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("keyToPlayerInputMapper");
		}

		private static ConsoleKeyInfo DummyReadKey() => ConsoleKeyInfoGenerator.Any();

		[Fact]
		public void Read_Called_ExpectMappedKeyIsReturned()
		{
			var key = ConsoleKeyInfoGenerator.Any();
			var playerInput = PlayerInputTestDoubles.Dummy();
			var keyToPlayerInputMapper = MapTestDoubles.StubFor(key, playerInput);
			var inputReader = new KeyboardPlayerInputReader(() => key, keyToPlayerInputMapper);
			inputReader.Read().Should().BeSameAs(playerInput);
		}

		[Fact]
		public void Read_CalledWhenKeyToPlayerInputMapperReturnsNull_ExpectInvalidOperationException()
		{
			var keyToPlayerInputMapper = MapTestDoubles.StubFor<ConsoleKeyInfo, IPlayerInput>((IPlayerInput) null!);
			var inputReader = new KeyboardPlayerInputReader(DummyReadKey, keyToPlayerInputMapper);
			inputReader
				.Invoking(x => x.Read())
				.Should().Throw<InvalidOperationException>()
				.And.Message.Should().MatchEquivalentOf("*mapper*null*");
		}
	}
}
