namespace Restall.Minefield.Game
{
	public interface IEvaluatePlayerInput
	{
		void Evaluate(IPlayerInput playerInput);
	}

	public interface IEvaluatePlayerInput<T> where T : IPlayerInput
	{
		void Evaluate(T playerInput);
	}
}
