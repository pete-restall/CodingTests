using System;
using System.Collections.Generic;
using System.Linq;

namespace Restall.Minefield.Mappers
{
	[ChainOfResponsibility]
	public class ConditionalMapperChain<TFrom, TTo> : IMapConditionally<TFrom, TTo>
	{
		private readonly IEnumerable<IMapConditionally<TFrom, TTo>> innerMappers;

		public ConditionalMapperChain(IEnumerable<IMapConditionally<TFrom, TTo>> innerMappers)
		{
			this.innerMappers = innerMappers?.ToArray() ?? throw new ArgumentNullException(nameof(innerMappers));
		}

		public TTo Map(TFrom unmapped)
		{
			if (unmapped is null)
				throw new ArgumentNullException(nameof(unmapped));

			if (!this.TryGetMapperFor(unmapped, out var mapper))
				throw new InvalidOperationException($"Cannot map {typeof(TFrom)} to {typeof(TTo)}; did you forget to call {nameof(this.CanMap)}() ?");

			return mapper!.Map(unmapped);
		}

		private bool TryGetMapperFor(TFrom unmapped, out IMap<TFrom, TTo>? mapper)
		{
			mapper = this.innerMappers.FirstOrDefault(x => x.CanMap(unmapped));
			return mapper is not null;
		}

		public bool CanMap(TFrom unmapped) => this.TryGetMapperFor(unmapped, out var _);
	}
}
