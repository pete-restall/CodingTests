using NSubstitute;
using Restall.Minefield.Game;

namespace Restall.Minefield.Tests.Unit.Game
{
	public static class PlayerInputTestDoubles
	{
		public static IPlayerInput Dummy() => Substitute.For<IPlayerInput>();
	}
}
