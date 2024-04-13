using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Lophtware.Testing.Utilities.NonDeterminism;
using NSubstitute;
using Restall.Minefield.Mappers;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Mappers
{
	public class ConditionalMapperChainTest
	{
		[Fact]
		public void Constructor_CalledWithNullInnerMappers_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => CreateMapperWith(null!);
			constructor.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("innerMappers");
		}

		private static ConditionalMapperChain<object, object> CreateMapperWith(IEnumerable<IMapConditionally<object, object>> innerMappers) =>
			new(innerMappers);

		[Fact]
		public void CanMap_CalledWithNull_ExpectFalseIsReturned()
		{
			var mapper = CreateMapperWith(ConditionalMapperTestDoubles.AnyNumberOfDummies<object, object>());
			mapper.CanMap(null!).Should().BeFalse();
		}

		[Fact]
		public void CanMap_CalledWhenNoInnerMappers_ExpectFalseIsReturned()
		{
			var mapper = CreateMapperWith(Enumerable.Empty<IMapConditionally<object, object>>());
			mapper.CanMap(new object()).Should().BeFalse();
		}

		[Fact]
		public void CanMap_CalledWhenNoInnerMappersCanMap_ExpectFalseIsReturned()
		{
			var mapper = CreateMapperWith(ConditionalMapperTestDoubles.AtLeastOneStubWithAllNotAbleToMapAnything<object, object>());
			mapper.CanMap(new object()).Should().BeFalse();
		}

		[Fact]
		public void CanMap_CalledWhenOneInnerMapperCanMap_ExpectTrueIsReturned()
		{
			var objToMap = new object();
			var mappersNotAbleToMap = ConditionalMapperTestDoubles.AnyNumberOfStubsWithAllNotAbleToMapAnything<object, object>();
			var innerMappers = mappersNotAbleToMap.Concat([ConditionalMapperTestDoubles.StubAbleToMap<object, object>(objToMap)]);
			var mapper = CreateMapperWith(innerMappers.Shuffle());
			mapper.CanMap(objToMap).Should().BeTrue();
		}

		[Fact]
		public void CanMap_CalledMultipleTimes_ExpectInnerMappersPassedToConstructorAreOnlyEnumeratedOnce()
		{
			var innerMappers = EnumerableTestDoubles.MockFor<IMapConditionally<object, object>>();
			var mapper = CreateMapperWith(innerMappers);
			mapper.CanMap(new object());
			mapper.CanMap(new object());
			innerMappers.Received(1).GetEnumerator();
		}

		[Fact]
		public void Map_CalledWithNull_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			var mapper = CreateMapperWith(ConditionalMapperTestDoubles.AnyNumberOfDummies<object, object>());
			mapper.Invoking(x => x.Map(null!)).Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("unmapped");
		}

		[Fact]
		public void Map_CalledWhenNoInnerMappers_ExpectInvalidOperationException()
		{
			var mapper = CreateMapperWith(Enumerable.Empty<IMapConditionally<object, object>>());
			mapper.Invoking(x => x.Map(new object())).Should().Throw<InvalidOperationException>().And.Message.Should().Contain("CanMap");
		}

		[Fact]
		public void Map_CalledWhenNoInnerMappersCanMap_ExpectInvalidOperationException()
		{
			var mapper = CreateMapperWith(ConditionalMapperTestDoubles.AtLeastOneStubWithAllNotAbleToMapAnything<object, object>());
			mapper.Invoking(x => x.Map(new object())).Should().Throw<InvalidOperationException>().And.Message.Should().Contain("CanMap");
		}

		[Fact]
		public void Map_CalledWhenOneInnerMapperCanMap_ExpectMappedObjectIsReturned()
		{
			var objToMap = new object();
			var mappedObj = new object();
			var mappersNotAbleToMap = ConditionalMapperTestDoubles.AnyNumberOfStubsWithAllNotAbleToMapAnything<object, object>();
			var innerMappers = mappersNotAbleToMap.Concat(new[] {ConditionalMapperTestDoubles.StubFor(objToMap, mappedObj)});
			var mapper = CreateMapperWith(innerMappers.Shuffle());
			mapper.Map(objToMap).Should().BeSameAs(mappedObj);
		}

		[Fact]
		public void Map_CalledMultipleTimes_ExpectInnerMappersPassedToConstructorAreOnlyEnumeratedOnce()
		{
			var ableToMap = ConditionalMapperTestDoubles.StubFor<object, object>(new object());
			var innerMappers = EnumerableTestDoubles.MockFor(ableToMap);
			var mapper = CreateMapperWith(innerMappers);
			mapper.Map(new object());
			mapper.Map(new object());
			innerMappers.Received(1).GetEnumerator();
		}
	}
}
