using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class RenderFramesTestDoubles
	{
		public static IRenderFrames Dummy() => Substitute.For<IRenderFrames>();

		public static IRenderFrames Mock() => Substitute.For<IRenderFrames>();
	}
}
