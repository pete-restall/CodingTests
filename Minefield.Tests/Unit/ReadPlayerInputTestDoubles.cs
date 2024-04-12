using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class ReadPlayerInputTestDoubles
	{
		public static IReadPlayerInput Dummy() => Substitute.For<IReadPlayerInput>();

		public static IReadPlayerInput StubFor(IPlayerInput playerInput)
		{
			var playerInputReader = Substitute.For<IReadPlayerInput>();
			playerInputReader.Read().Returns(playerInput);
			return playerInputReader;
		}
	}
}
