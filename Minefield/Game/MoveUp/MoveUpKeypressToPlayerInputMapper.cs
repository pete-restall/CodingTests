using System;

namespace Restall.Minefield.Game.MoveUp
{
	public class MoveUpKeypressToPlayerInputMapper : IMapConditionally<ConsoleKeyInfo, IPlayerInput>
	{
		private static readonly IPlayerInput MoveUp = new MoveUpPlayerInput();

		public bool CanMap(ConsoleKeyInfo unmapped) => unmapped.Key == ConsoleKey.UpArrow && unmapped.Modifiers == ConsoleModifiers.None;

		public IPlayerInput Map(ConsoleKeyInfo unmapped)
		{
			if (!this.CanMap(unmapped))
				throw new ArgumentException($"ConsoleKeyInfo does not represent the 'Move Up' key; did you forget to call {nameof(this.CanMap)}() ?  key={unmapped.Key}, modifiers={unmapped.Modifiers}", nameof(unmapped));

			return MoveUp;
		}
	}
}
