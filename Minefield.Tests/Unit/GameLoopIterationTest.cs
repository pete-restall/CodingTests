using System;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Restall.Minefield.Tests.Unit
{
	public class GameLoopIterationTest
	{
		[Fact]
		public void Constructor_CalledWithNullPlayerInputReader_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new GameLoopIteration(
				null!,
				EvaluatePlayerInputTestDoubles.Dummy(),
				RenderFramesTestDoubles.Dummy());

			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("playerInputReader");
		}

		[Fact]
		public void Constructor_CalledWithNullPlayerInputEvaluator_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new GameLoopIteration(
				ReadPlayerInputTestDoubles.Dummy(),
				null!,
				RenderFramesTestDoubles.Dummy());

			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("playerInputEvaluator");
		}

		[Fact]
		public void Constructor_CalledWithNullFrameRenderer_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new GameLoopIteration(
				ReadPlayerInputTestDoubles.Dummy(),
				EvaluatePlayerInputTestDoubles.Dummy(),
				null!);

			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("frameRenderer");
		}

		[Fact]
		public void Run_Called_ExpectPlayerInputEvaluatorIsPassedSameInputFromPlayerInputReader()
		{
			var playerInputEvaluator = EvaluatePlayerInputTestDoubles.Mock();
			var playerInput = PlayerInputTestDoubles.Dummy();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(playerInput);
			var gameLoopIteration = new GameLoopIteration(playerInputReader, playerInputEvaluator, RenderFramesTestDoubles.Dummy());
			gameLoopIteration.Run();
			playerInputEvaluator.Received(1).Evaluate(playerInput);
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsQuitGame_ExpectFalseIsReturned()
		{
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(new QuitGamePlayerInput());
			var gameLoopIteration = new GameLoopIteration(playerInputReader, EvaluatePlayerInputTestDoubles.Dummy(), RenderFramesTestDoubles.Dummy());
			gameLoopIteration.Run().Should().BeFalse();
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsQuitGame_ExpectPlayerInputEvaluatorIsNotCalled()
		{
			var playerInputEvaluator = EvaluatePlayerInputTestDoubles.Mock();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(new QuitGamePlayerInput());
			var gameLoopIteration = new GameLoopIteration(playerInputReader, playerInputEvaluator, RenderFramesTestDoubles.Dummy());
			gameLoopIteration.Run();
			playerInputEvaluator.DidNotReceive().Evaluate(Arg.Any<IPlayerInput>());
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsNotQuitGameOrUnknownPlayerInput_ExpectTrueIsReturned()
		{
			var anythingOtherThanQuitGamePlayerInput = PlayerInputTestDoubles.Dummy();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(anythingOtherThanQuitGamePlayerInput);
			var gameLoopIteration = new GameLoopIteration(playerInputReader, EvaluatePlayerInputTestDoubles.Dummy(), RenderFramesTestDoubles.Dummy());
			gameLoopIteration.Run().Should().BeTrue();
		}

		[Fact]
		public void Run_CalledWhenPlayerInputReaderReturnsNull_ExpectInvalidOperationException()
		{
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(null!);
			var gameLoopIteration = new GameLoopIteration(playerInputReader, EvaluatePlayerInputTestDoubles.Dummy(), RenderFramesTestDoubles.Dummy());
			gameLoopIteration
				.Invoking(x => x.Run())
				.Should().Throw<InvalidOperationException>()
				.And.Message.Should().MatchEquivalentOf("*input*null*");
		}

		[Fact]
		public void Run_CalledWhenPlayerInputReaderReturnsNull_ExpectPlayerInputEvaluatorIsNotCalled()
		{
			var playerInputEvaluator = EvaluatePlayerInputTestDoubles.Mock();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(null!);
			var gameLoopIteration = new GameLoopIteration(playerInputReader, playerInputEvaluator, RenderFramesTestDoubles.Dummy());
			gameLoopIteration.Invoking(x => x.Run()).Should().Throw<Exception>();
			playerInputEvaluator.DidNotReceive().Evaluate(Arg.Any<IPlayerInput>());
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsNotQuitGame_ExpectFrameIsRenderedAfterPlayerInputHasBeenEvaluated()
		{
			var frameRenderer = RenderFramesTestDoubles.Mock();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(new QuitGamePlayerInput());
			var gameLoopIteration = new GameLoopIteration(playerInputReader, EvaluatePlayerInputTestDoubles.Dummy(), frameRenderer);
			gameLoopIteration.Run();
			frameRenderer.DidNotReceive().Render();
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsQuitGame_ExpectFrameIsRenderedAfterPlayerInputHasBeenEvaluated()
		{
			var playerInputEvaluator = EvaluatePlayerInputTestDoubles.Mock();
			var frameRenderer = RenderFramesTestDoubles.Mock();
			var anythingOtherThanQuitGamePlayerInput = PlayerInputTestDoubles.Dummy();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(anythingOtherThanQuitGamePlayerInput);
			var gameLoopIteration = new GameLoopIteration(playerInputReader, playerInputEvaluator, frameRenderer);
			gameLoopIteration.Run();
			Received.InOrder(() =>
			{
				playerInputEvaluator.Evaluate(Arg.Any<IPlayerInput>());
				frameRenderer.Render();
			});
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsUnknown_ExpectTrueIsReturned()
		{
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(new UnknownPlayerInput());
			var gameLoopIteration = new GameLoopIteration(playerInputReader, EvaluatePlayerInputTestDoubles.Dummy(), RenderFramesTestDoubles.Dummy());
			gameLoopIteration.Run().Should().BeTrue();
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsUnknown_ExpectPlayerInputEvaluatorIsNotCalled()
		{
			var playerInputEvaluator = EvaluatePlayerInputTestDoubles.Mock();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(new UnknownPlayerInput());
			var gameLoopIteration = new GameLoopIteration(playerInputReader, playerInputEvaluator, RenderFramesTestDoubles.Dummy());
			gameLoopIteration.Run();
			playerInputEvaluator.DidNotReceive().Evaluate(Arg.Any<IPlayerInput>());
		}

		[Fact]
		public void Run_CalledWhenPlayerInputIsUnknown_ExpectFrameIsNotRendered()
		{
			var frameRenderer = RenderFramesTestDoubles.Mock();
			var playerInputReader = ReadPlayerInputTestDoubles.StubFor(new UnknownPlayerInput());
			var gameLoopIteration = new GameLoopIteration(playerInputReader, EvaluatePlayerInputTestDoubles.Dummy(), frameRenderer);
			gameLoopIteration.Run();
			frameRenderer.DidNotReceive().Render();
		}
	}
}
