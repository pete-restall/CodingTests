using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class EvaluatePlayerInputTestDoubles
	{
		public static IEvaluatePlayerInput Dummy() => Substitute.For<IEvaluatePlayerInput>();

		public static IEvaluatePlayerInput Mock() => Substitute.For<IEvaluatePlayerInput>();
	}
}
