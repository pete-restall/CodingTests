using System;
using FluentAssertions;
using NSubstitute;
using Restall.Minefield.Game;
using Xunit;

namespace Restall.Minefield.Tests.Unit.Game
{
	public class PlayerInputEvaluationAdaptorTest
	{
		public class StubPlayerInput : IPlayerInput { }

		public class StubDerivedPlayerInput : StubPlayerInput { }

		public class StubUnrelatedPlayerInput : IPlayerInput { }

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Constructor_CalledWithNullPlayerInputEvaluator_ExpectArgumentNullExceptionWithCorrectParamName(bool matchInherited)
		{
			Action constructor = () => _ = new PlayerInputEvaluationAdaptor<StubPlayerInput>(null!, matchInherited);
			constructor
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("playerInputEvaluator");
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Evaluate_CalledWithNullPlayerInput_ExpectArgumentNullExceptionWithCorrectParamName(bool matchInherited)
		{
			var evaluatorAdapter = new PlayerInputEvaluationAdaptor<StubPlayerInput>(EvaluatePlayerInputTestDoubles.Dummy<StubPlayerInput>(), matchInherited);
			evaluatorAdapter
				.Invoking(x => x.Evaluate(null!))
				.Should().Throw<ArgumentNullException>()
				.And.ParamName.Should().Be("playerInput");
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Evaluate_CalledWhenPlayerInputIsSameTypeAsGenericArgument_ExpectInnerEvaluatorIsCalledWithCastPlayerInput(bool matchInherited)
		{
			var innerEvaluator = EvaluatePlayerInputTestDoubles.Mock<StubPlayerInput>();
			var playerInput = new StubPlayerInput();
			var evaluatorAdapter = new PlayerInputEvaluationAdaptor<StubPlayerInput>(innerEvaluator, matchInherited);
			evaluatorAdapter.Evaluate(playerInput);
			innerEvaluator.Received(1).Evaluate(playerInput);
		}

		[Theory]
		[InlineData(true)]
		[InlineData(false)]
		public void Evaluate_CalledWhenPlayerInputIsUnrelatedTypeToGenericArgument_ExpectInnerEvaluatorIsNotCalled(bool matchInherited)
		{
			var innerEvaluator = EvaluatePlayerInputTestDoubles.Mock<StubPlayerInput>();
			var evaluatorAdapter = new PlayerInputEvaluationAdaptor<StubPlayerInput>(innerEvaluator, matchInherited);
			evaluatorAdapter.Evaluate(new StubUnrelatedPlayerInput());
			innerEvaluator.DidNotReceive().Evaluate(Arg.Any<StubPlayerInput>());
		}

		[Fact]
		public void Evaluate_CalledWhenPlayerInputIsTypeDerivedFromGenericArgumentAndMatchInheritedIsTrue_ExpectInnerEvaluatorIsCalledWithCastPlayerInput()
		{
			var innerEvaluator = EvaluatePlayerInputTestDoubles.Mock<StubPlayerInput>();
			var playerInput = new StubDerivedPlayerInput();
			var evaluatorAdapter = new PlayerInputEvaluationAdaptor<StubPlayerInput>(innerEvaluator, matchInherited: true);
			evaluatorAdapter.Evaluate(playerInput);
			innerEvaluator.Received(1).Evaluate(playerInput);
		}

		[Fact]
		public void Evaluate_CalledWhenPlayerInputIsTypeDerivedFromGenericArgumentAndMatchInheritedIsFalse_ExpectInnerEvaluatorIsNotCalled()
		{
			var innerEvaluator = EvaluatePlayerInputTestDoubles.Mock<StubPlayerInput>();
			var playerInput = new StubDerivedPlayerInput();
			var evaluatorAdapter = new PlayerInputEvaluationAdaptor<StubPlayerInput>(innerEvaluator, matchInherited: false);
			evaluatorAdapter.Evaluate(playerInput);
			innerEvaluator.DidNotReceive().Evaluate(Arg.Any<StubPlayerInput>());
		}
	}
}
