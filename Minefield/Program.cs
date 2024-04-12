using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Restall.Minefield
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateHost(args).Build();
			host.Services.GetRequiredService<IGameLoop>().Run();
		}

		public static IHostBuilder CreateHost(string[] args) => Host
			.CreateDefaultBuilder(args ?? throw new ArgumentNullException(nameof(args)))
			.ConfigureServices(services => services.Scan(scan => scan
				.FromAssemblyOf<IGameLoop>()
				.AddClasses()
				.AsSelfWithInterfaces()
				.WithSingletonLifetime()));
	}
}
