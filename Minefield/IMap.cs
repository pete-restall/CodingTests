namespace Restall.Minefield
{
	public interface IMap<in TFrom, out TTo>
	{
		TTo Map(TFrom unmapped);
	}
}
