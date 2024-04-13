using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Restall.Minefield.Game
{
	[ChainOfResponsibility]
	public class PlayerInputEvaluationChain : IEvaluatePlayerInput
	{
		private readonly IEnumerable<IEvaluatePlayerInput> playerInputEvaluators;

		public PlayerInputEvaluationChain(IEnumerable<IEvaluatePlayerInput> playerInputEvaluators)
		{
			this.playerInputEvaluators = playerInputEvaluators?.ToImmutableArray() ?? throw new ArgumentNullException(nameof(playerInputEvaluators));
		}

		public void Evaluate(IPlayerInput playerInput)
		{
			if (playerInput is null)
				throw new ArgumentNullException(nameof(playerInput));

			this.playerInputEvaluators.ForEach(x => x.Evaluate(playerInput));
		}
	}
}
