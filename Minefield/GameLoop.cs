using System;

namespace Restall.Minefield
{
	public class GameLoop : IGameLoop
	{
		private readonly IGameLoopIteration gameLoopIteration;
		private readonly IRenderFrames frameRenderer;

		public GameLoop(IGameLoopIteration gameLoopIteration, IRenderFrames frameRenderer)
		{
			this.gameLoopIteration = gameLoopIteration ?? throw new ArgumentNullException(nameof(gameLoopIteration));
			this.frameRenderer = frameRenderer ?? throw new ArgumentNullException(nameof(frameRenderer));
		}

		public void Run()
		{
			this.frameRenderer.Render();
			while (this.gameLoopIteration.Run())
				;;
		}
	}
}
