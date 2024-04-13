using NSubstitute;
using Restall.Minefield.Game;

namespace Restall.Minefield.Tests.Unit.Game
{
	public static class EvaluatePlayerInputTestDoubles
	{
		public static IEvaluatePlayerInput Dummy() => Substitute.For<IEvaluatePlayerInput>();

		public static IEvaluatePlayerInput Mock() => Substitute.For<IEvaluatePlayerInput>();
	}
}
