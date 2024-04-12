using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class PlayerInputTestDoubles
	{
		public static IPlayerInput Dummy() => Substitute.For<IPlayerInput>();
	}
}
