using NSubstitute;
using Restall.Minefield.Game;

namespace Restall.Minefield.Tests.Unit.Game
{
	public static class EvaluatePlayerInputTestDoubles
	{
		public static IEvaluatePlayerInput Dummy() => Substitute.For<IEvaluatePlayerInput>();

		public static IEvaluatePlayerInput<T> Dummy<T>() where T : IPlayerInput => Substitute.For<IEvaluatePlayerInput<T>>();

		public static IEvaluatePlayerInput Mock() => Substitute.For<IEvaluatePlayerInput>();

		public static IEvaluatePlayerInput<T> Mock<T>() where T : IPlayerInput => Substitute.For<IEvaluatePlayerInput<T>>();
	}
}
