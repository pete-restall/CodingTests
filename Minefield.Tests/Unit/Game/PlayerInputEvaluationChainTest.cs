using System;
using FluentAssertions;
using NSubstitute;
using Restall.Minefield.Game;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Game
{
	public class PlayerInputEvaluationChainTest
	{
		[Fact]
		public void Constructor_CalledWithNullPlayerInputEvaluators_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			Action constructor = () => _ = new PlayerInputEvaluationChain(null!);
			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("playerInputEvaluators");
		}

		[Fact]
		public void Evaluate_CalledWithNullPlayerInput_ExpectArgumentNullExceptionWithCorrectParamName()
		{
			var evaluationChain = new PlayerInputEvaluationChain([]);
			evaluationChain.Invoking(x => x.Evaluate(null!))
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("playerInput");
		}

		[Fact]
		public void Evaluate_Called_ExpectAllPlayerInputEvaluatorsPassedToConstructorAreCalledWithSamePlayerInput()
		{
			var innerEvaluators = Generate.AtLeastOne(EvaluatePlayerInputTestDoubles.Mock);
			var playerInput = PlayerInputTestDoubles.Dummy();
			var evaluationChain = new PlayerInputEvaluationChain(innerEvaluators);
			evaluationChain.Evaluate(playerInput);
			innerEvaluators.ForEach(x => x.Received(1).Evaluate(playerInput));
		}

		[Fact]
		public void Evaluate_CalledMultipleTimes_ExpectPlayerInputEvaluatorsPassedToConstructorAreOnlyEnumeratedOnce()
		{
			var innerEvaluators = EnumerableTestDoubles.MockFor<IEvaluatePlayerInput>();
			var evaluationChain = new PlayerInputEvaluationChain(innerEvaluators);
			evaluationChain.Evaluate(PlayerInputTestDoubles.Dummy());
			evaluationChain.Evaluate(PlayerInputTestDoubles.Dummy());
			innerEvaluators.Received(1).GetEnumerator();
		}
	}
}
