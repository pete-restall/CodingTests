using System;
using FluentAssertions;
using Xunit;

namespace Restall.Minefield.Tests.Unit
{
	public class ProgramTest
	{
		[Fact]
		public void CreateHost_CalledWithNullArgs_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			FluentActions
				.Invoking(() => Program.CreateHost(null!))
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("args");
		}
	}
}
