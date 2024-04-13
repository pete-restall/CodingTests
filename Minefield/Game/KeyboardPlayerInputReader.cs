using System;

namespace Restall.Minefield.Game
{
	public class KeyboardPlayerInputReader : IReadPlayerInput
	{
		private readonly Func<ConsoleKeyInfo> readKey;
		private readonly IMap<ConsoleKeyInfo, IPlayerInput> keyToPlayerInputMapper;

		public KeyboardPlayerInputReader(Func<ConsoleKeyInfo> readKey, IMap<ConsoleKeyInfo, IPlayerInput> keyToPlayerInputMapper)
		{
			this.readKey = readKey ?? throw new ArgumentNullException(nameof(readKey));
			this.keyToPlayerInputMapper = keyToPlayerInputMapper ?? throw new ArgumentNullException(nameof(keyToPlayerInputMapper));
		}

		public IPlayerInput Read()
		{
			var key = this.readKey();
			return this.keyToPlayerInputMapper.Map(key)
				?? throw new InvalidOperationException($"Player Input Mapper returned null; mapper={this.keyToPlayerInputMapper}, key={key}");
		}
	}
}
