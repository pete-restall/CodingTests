using System;

namespace Restall.Minefield.Game
{
	public class PlayerInputEvaluationAdaptor<T> : IEvaluatePlayerInput where T : IPlayerInput
	{
		private readonly IEvaluatePlayerInput<T> playerInputEvaluator;
		private readonly bool matchInherited;

		public PlayerInputEvaluationAdaptor(IEvaluatePlayerInput<T> playerInputEvaluator, bool matchInherited)
		{
			this.playerInputEvaluator = playerInputEvaluator ?? throw new ArgumentNullException(nameof(playerInputEvaluator));
			this.matchInherited = matchInherited;
		}

		public void Evaluate(IPlayerInput playerInput)
		{
			if (playerInput is null)
				throw new ArgumentNullException(nameof(playerInput));

			if (playerInput is T castPlayerInput)
			{
				if (this.matchInherited || playerInput.GetType() == typeof(T))
					this.playerInputEvaluator.Evaluate(castPlayerInput);
			}
		}
	}
}
