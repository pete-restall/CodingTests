using NSubstitute;

namespace Restall.Minefield.Tests.Unit
{
	public static class MapTestDoubles
	{
		public static IMap<TFrom, TTo> Dummy<TFrom, TTo>() => Substitute.For<IMap<TFrom, TTo>>();

		public static IMap<TFrom, TTo> StubFor<TFrom, TTo>(TFrom unmapped, TTo mapped)
		{
			var mapper = Stub<TFrom, TTo>();
			mapper.Map(unmapped).Returns(mapped);
			return mapper;
		}

		public static IMap<TFrom, TTo> Stub<TFrom, TTo>() => Substitute.For<IMap<TFrom, TTo>>();

		public static IMap<TFrom, TTo> StubFor<TFrom, TTo>(TTo mapped)
		{
			var mapper = Stub<TFrom, TTo>();
			mapper.Map(Arg.Any<TFrom>()).Returns(mapped);
			return mapper;
		}
	}
}
