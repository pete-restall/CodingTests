using System.Linq;
using Lophtware.Testing.Utilities;
using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class GameLoopIterationTestDoubles
	{
		public static IGameLoopIteration Dummy() => Substitute.For<IGameLoopIteration>();

		public static IGameLoopIteration MockFor(int numberOfIterations) => StubFor(numberOfIterations);

		public static IGameLoopIteration StubFor(int numberOfIterations)
		{
			var iteration = Substitute.For<IGameLoopIteration>();
			iteration.Run().Returns(numberOfIterations > 1, (numberOfIterations - 2).Select(() => true).Concat([false]).ToArray());
			return iteration;
		}
	}
}
