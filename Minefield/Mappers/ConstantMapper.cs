using System;

namespace Restall.Minefield.Mappers
{
	public class ConstantMapper<TFrom, TTo> : IMapConditionally<TFrom, TTo>
	{
		private readonly TTo mapped;

		public ConstantMapper(TTo mapped)
		{
			this.mapped = mapped ?? throw new ArgumentNullException(nameof(mapped));
		}

		public bool CanMap(TFrom unmapped) => true;

		public TTo Map(TFrom unmapped) => this.mapped;
	}
}
