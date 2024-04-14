using System;

namespace Restall.Minefield.Game.MoveUp
{
	public class QuitGameKeypressToPlayerInputMapper : IMapConditionally<ConsoleKeyInfo, IPlayerInput>
	{
		private static readonly IPlayerInput QuitGame = new QuitGamePlayerInput();

		public bool CanMap(ConsoleKeyInfo unmapped) => unmapped.Key == ConsoleKey.Q;

		public IPlayerInput Map(ConsoleKeyInfo unmapped)
		{
			if (!this.CanMap(unmapped))
				throw new ArgumentException($"ConsoleKeyInfo does not represent the 'Quit' key; did you forget to call {nameof(this.CanMap)}() ?  key={unmapped.KeyChar}", nameof(unmapped));

			return QuitGame;
		}
	}
}
