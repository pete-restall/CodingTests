namespace Restall.Minefield
{
	public interface IMapConditionally<in TFrom, out TTo> : IMap<TFrom, TTo>
	{
		bool CanMap(TFrom unmapped);
	}
}
