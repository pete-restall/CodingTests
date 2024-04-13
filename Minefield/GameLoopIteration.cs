using System;

namespace Restall.Minefield
{
	public class GameLoopIteration : IGameLoopIteration
	{
		private readonly IReadPlayerInput playerInputReader;
		private readonly IEvaluatePlayerInput playerInputEvaluator;
		private readonly IRenderFrames frameRenderer;

		public GameLoopIteration(IReadPlayerInput playerInputReader, IEvaluatePlayerInput playerInputEvaluator, IRenderFrames frameRenderer)
		{
			this.playerInputReader = playerInputReader ?? throw new ArgumentNullException(nameof(playerInputReader));
			this.playerInputEvaluator = playerInputEvaluator ?? throw new ArgumentNullException(nameof(playerInputEvaluator));
			this.frameRenderer = frameRenderer ?? throw new ArgumentNullException(nameof(frameRenderer));
		}

		public bool Run()
		{
			var playerInput = this.playerInputReader.Read() ?? throw new InvalidOperationException("Player Input Reader returned null");
			if (playerInput is not UnknownPlayerInput and not QuitGamePlayerInput)
			{
				this.playerInputEvaluator.Evaluate(playerInput);
				this.frameRenderer.Render();
			}

			return playerInput is not QuitGamePlayerInput;
		}
	}
}
