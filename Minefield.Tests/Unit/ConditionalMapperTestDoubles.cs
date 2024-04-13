using System.Collections.Generic;
using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class ConditionalMapperTestDoubles
	{
		public static IEnumerable<IMapConditionally<TFrom, TTo>> AnyNumberOfDummies<TFrom, TTo>() => Generate.AnyNumberOf(Dummy<TFrom, TTo>);

		private static IMapConditionally<TFrom, TTo> Dummy<TFrom, TTo>() => Substitute.For<IMapConditionally<TFrom, TTo>>();

		public static IEnumerable<IMapConditionally<TFrom, TTo>> AtLeastOneStubWithAllNotAbleToMapAnything<TFrom, TTo>() =>
			Generate.AtLeastOne(StubNotAbleToMapAnything<TFrom, TTo>);

		private static IMapConditionally<TFrom, TTo> StubNotAbleToMapAnything<TFrom, TTo>()
		{
			var mapper = Stub<TFrom, TTo>();
			mapper.CanMap(Arg.Any<TFrom>()).Returns(false);
			return mapper;
		}

		private static IMapConditionally<TFrom, TTo> Stub<TFrom, TTo>() => Substitute.For<IMapConditionally<TFrom, TTo>>();

		public static IEnumerable<IMapConditionally<TFrom, TTo>> AnyNumberOfStubsWithAllNotAbleToMapAnything<TFrom, TTo>() =>
			Generate.AnyNumberOf(StubNotAbleToMapAnything<TFrom, TTo>);

		public static IMapConditionally<TFrom, TTo> StubAbleToMap<TFrom, TTo>(TFrom unmapped) => StubFor(unmapped, default(TTo)!);

		public static IMapConditionally<TFrom, TTo> StubFor<TFrom, TTo>(TFrom unmapped, TTo mapped)
		{
			var mapper = Stub<TFrom, TTo>();
			mapper.CanMap(unmapped).Returns(true);
			mapper.Map(unmapped).Returns(mapped);
			return mapper;
		}

		public static IMapConditionally<TFrom, TTo> StubFor<TFrom, TTo>(TTo mapped)
		{
			var mapper = Stub<TFrom, TTo>();
			mapper.CanMap(Arg.Any<TFrom>()).Returns(true);
			mapper.Map(Arg.Any<TFrom>()).Returns(mapped);
			return mapper;
		}
	}
}
