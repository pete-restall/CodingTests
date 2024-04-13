using System;
using FluentAssertions;
using Lophtware.Testing.Utilities.NonDeterminism.PrimitiveGeneration;
using NSubstitute;
using Restall.Minefield.Game;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Game
{
	public class GameLoopTest
	{
		[Fact]
		public void Constructor_CalledWithNullGameLoopIteration_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new GameLoop(null!, RenderFramesTestDoubles.Dummy());
			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("gameLoopIteration");
		}

		[Fact]
		public void Constructor_CalledWithNullFrameRenderer_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new GameLoop(GameLoopIterationTestDoubles.Dummy(), null!);
			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("frameRenderer");
		}

		[Fact]
		public void Run_Called_ExpectGameLoopIterationIsCalledUntilItReturnsFalse()
		{
			var numberOfIterations = IntegerGenerator.WithinInclusiveRange(1, 10);
			var gameLoopIteration = GameLoopIterationTestDoubles.MockFor(numberOfIterations);
			var gameLoop = new GameLoop(gameLoopIteration, RenderFramesTestDoubles.Dummy());
			gameLoop.Run();
			gameLoopIteration.Received(numberOfIterations).Run();
		}

		[Fact]
		public void Run_Called_ExpectFrameIsRenderedPriorToFirstLoopIteration()
		{
			var gameLoopIteration = GameLoopIterationTestDoubles.MockFor(numberOfIterations: 1);
			var frameRenderer = RenderFramesTestDoubles.Mock();
			var gameLoop = new GameLoop(gameLoopIteration, frameRenderer);
			gameLoop.Run();
			Received.InOrder(() =>
			{
				frameRenderer.Render();
				gameLoopIteration.Run();
			});
		}

		[Fact]
		public void Run_Called_ExpectFrameRenderIsOnlyEverCalledOnce()
		{
			var frameRenderer = RenderFramesTestDoubles.Mock();
			var gameLoopIteration = GameLoopIterationTestDoubles.StubFor(numberOfIterations: IntegerGenerator.WithinInclusiveRange(2, 10));
			var gameLoop = new GameLoop(gameLoopIteration, frameRenderer);
			gameLoop.Run();
			frameRenderer.Received(1).Render();
		}

		[Fact]
		public void Run_NotCalled_ExpectFrameRenderIsNotCalled()
		{
			var frameRenderer = RenderFramesTestDoubles.Mock();
			_ = new GameLoop(GameLoopIterationTestDoubles.Dummy(), frameRenderer);
			frameRenderer.DidNotReceive().Render();
		}
	}
}
