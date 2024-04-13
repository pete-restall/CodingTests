using System;
using FluentAssertions;
using Restall.Minefield.Mappers;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Mappers
{
	public class ConstantMapperTest
	{
		[Fact]
		public void Constructor_CalledWithNullMapped_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new ConstantMapper<object, object>(null!);
			constructor.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("mapped");
		}

		[Fact]
		public void CanMap_CalledWithNull_ExpectTrueIsReturned()
		{
			var mapper = new ConstantMapper<object, object>(new object());
			mapper.CanMap(null!).Should().BeTrue();
		}

		[Fact]
		public void CanMap_CalledWithAnyObject_ExpectTrueIsReturned()
		{
			var mapper = new ConstantMapper<object, object>(new object());
			mapper.CanMap(new object()).Should().BeTrue();
		}

		[Fact]
		public void Map_CalledWithNullObject_ExpectObjectPassedToConstructorIsReturned()
		{
			var mappedConstant = new object();
			var mapper = new ConstantMapper<object, object>(mappedConstant);
			mapper.Map(null!).Should().BeSameAs(mappedConstant);
		}

		[Fact]
		public void Map_CalledWithAnyObject_ExpectObjectPassedToConstructorIsReturned()
		{
			var mappedConstant = new object();
			var mapper = new ConstantMapper<object, object>(mappedConstant);
			mapper.Map(new object()).Should().BeSameAs(mappedConstant);
		}
	}
}
