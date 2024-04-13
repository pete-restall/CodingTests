using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Restall.Minefield.Game;
using Restall.Minefield.Mappers;

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
			.ConfigureServices(services =>
			{
				services
					.Scan(scan => scan
						.FromAssemblyOf<IGameLoop>()
						.AddClasses(classes => classes.WithoutAttribute<ChainOfResponsibilityAttribute>())
						.AsSelfWithInterfaces()
						.WithSingletonLifetime())

					.AddSingleton(ctx => new KeyboardPlayerInputReader(
						Console.ReadKey,
						ctx.ConditionalMapperChainFor<ConsoleKeyInfo, IPlayerInput>(withDefault: new UnknownPlayerInput())));
			});

		private static ConditionalMapperChain<TFrom, TTo> ConditionalMapperChainFor<TFrom, TTo>(this IServiceProvider diContext, TTo withDefault) => new(
			diContext
				.GetServices<IMapConditionally<TFrom, TTo>>()
				.Concat([new ConstantMapper<TFrom, TTo>(withDefault)]));
		}
}
