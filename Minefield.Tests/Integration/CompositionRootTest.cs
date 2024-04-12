using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Restall.Minefield.Tests.Integration
{
	public class CompositionRootTest
	{
		[Fact]
		public void ExpectGameLoopCanBeConstructed()
		{
			var services = Program.CreateHost([]).Build().Services;
			services.GetService<IGameLoop>().Should().NotBeNull();
		}
	}
}
