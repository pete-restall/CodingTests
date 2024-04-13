using NSubstitute;
using Restall.Minefield.Game;

namespace Restall.Minefield.Tests.Unit.Game
{
	public static class RenderFramesTestDoubles
	{
		public static IRenderFrames Dummy() => Substitute.For<IRenderFrames>();

		public static IRenderFrames Mock() => Substitute.For<IRenderFrames>();
	}
}
