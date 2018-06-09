using SolidSharp.Expressions;
using SolidSharp.Expressions.Extensions;
using System;
using System.Collections.Immutable;
using System.Reflection;
using Xunit;
using static SolidSharp.Expressions.SymbolicMath;

namespace SolidSharp.Tests.Expressions
{
	public sealed class ExpressionSimplificationTest
	{
		[Fact]
		public void DoubleNegationShouldBeNegated()
		{
			var t = Var("𝓉");

			Assert.Same(t, -(-t));
		}

		[Fact]
		public void ExponentsShouldSum()
		{
			var t = Var("𝓉");
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(Pow(t, x + y), Pow(t, x) * Pow(t, y));
		}

		[Fact]
		public void ExponentsShouldSubstract()
		{
			var t = Var("𝓉");
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(Pow(t, x - y), Pow(t, x) / Pow(t, y));
		}

		[Fact]
		public void ExponentsShouldMultiply()
		{
			var t = Var("𝓉");
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(Pow(t, x * y), Pow(Pow(t, x), y));
			Assert.Equal(Pow(t, x * y), Pow(Pow(t, y), x));
		}

		[Fact]
		public void RootsShouldMultiply()
		{
			var t = Var("𝓉");
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(Root(t, x * y), Root(Root(t, x), y));
			Assert.Equal(Root(t, x * y), Root(Root(t, y), x));
		}

		[Fact]
		public void PowersAndMultiplicationsShouldMerge()
		{
			var t = Var("𝓉");

			Assert.Equal(Pow(2, 3 + t), 8 * Pow(2, t));
			Assert.Equal(Pow(2, 3 + t), Pow(2, t) * 8);

			Assert.Equal(3 * Pow(2, 4 + t), 48 * Pow(2, t));
			Assert.Equal(3 * Pow(2, 4 + t), Pow(2, t) * 48);

			Assert.Equal(Pow(3, 2 + t), 9 * Pow(3, t));
			Assert.Equal(Pow(3, 2 + t), Pow(3, t) * 9);

			Assert.Equal(2 * Pow(3, 4 + t), 162 * Pow(3, t));
			Assert.Equal(2 * Pow(3, 4 + t), Pow(3, t) * 162);
		}

		[Fact]
		public void AdditionsShouldMerge()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");
			var z = Var("𝓏");
			var w = Var("𝓌");

			var expected = SymbolicExpression.Add(ImmutableArray.Create(x, y, z, w));

			Assert.Equal(expected, x + (y + (z + w)));
			Assert.Equal(expected, x + ((y + z) + w));
			Assert.Equal(expected, (x + y) + (z + w));
			Assert.Equal(expected, (x + (y + z)) + w);
			Assert.Equal(expected, ((x + y) + z) + w);
		}

		[Fact]
		public void MultiplicationsShouldMerge()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");
			var z = Var("𝓏");
			var w = Var("𝓌");

			var expected = SymbolicExpression.Multiply(ImmutableArray.Create(x, y, z, w));

			Assert.Equal(expected, x * (y * (z * w)));
			Assert.Equal(expected, x * ((y * z) * w));
			Assert.Equal(expected, (x * y) * (z * w));
			Assert.Equal(expected, (x * (y * z)) * w);
			Assert.Equal(expected, ((x * y) * z) * w);
		}

		[Fact]
		public void DivisionsShouldMerge()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");
			var z = Var("𝓏");
			var w = Var("𝓌");

			Assert.Equal((x * z) / (y * w), (x / y) * (z / w));
		}

		[Fact]
		public void DivisionByZeroShouldThrow()
		{
			var t = Var("𝓉");

			Assert.Throws<DivideByZeroException>(() => t / Zero);
		}

		[Fact]
		public void NumbersShouldBeOrderedFirstInCommutativeBinaryOperations()
		{
			var t = Var("𝓉");

			var addition = t + 99;

			Assert.Equal(99 + t, addition);
			Assert.Equal(99, ((BinaryOperationExpression)(addition)).FirstOperand);

			var multiplication = t * 138;

			Assert.Equal(138 * t, multiplication);
			Assert.Equal(138, ((BinaryOperationExpression)(multiplication)).FirstOperand);
		}

		[Fact]
		public void NumbersShouldBeOrderedFirstInCommutativeVariadicOperations()
		{
			var t = Var("𝓉");

			var addition = SymbolicExpression.Add(ImmutableArray.Create(t, Pi, 735, Pow(t, 2)));

			Assert.Equal(735 + Pi + t + Pow(t, 2), addition);
			Assert.Equal(735, ((VariadicOperationExpression)addition).Operands[0]);

			var multiplication = SymbolicExpression.Multiply(ImmutableArray.Create(t, Pow(t, 2), 78, Pow(E, t), Pi));

			Assert.Equal(78 * Pi * t * Pow(t, 2) * Pow(E, t), multiplication);
			Assert.Equal(78, ((VariadicOperationExpression)multiplication).Operands[0]);
		}

		[Fact]
		public void MultiplicationByZeroShouldBeZero()
		{
			var t = Var("𝓉");

			Assert.Equal(0, N(0) * N(641));
			Assert.Equal(0, N(641) * N(0));

			Assert.Equal(0, N(0) * Pi);
			Assert.Equal(0, Pi * N(0));

			Assert.Equal(0, t * 0);
			Assert.Equal(0, 0 * t);
		}

		[Fact]
		public void IdentitySubtractionsShouldCancel()
		{
			var t = Var("𝓉");

			Assert.Same(Zero, t - t);
		}

		[Fact]
		public void NegationShouldConvertToSubtraction()
		{
			var t = Var("𝓉");
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(x - y, x + (-y));
			Assert.Equal(y - x, (-x) + y);
			Assert.Same(Zero, t + (-t));
		}

		[Theory]
		[InlineData(9, 4)]
		[InlineData(21, 99)]
		[InlineData(77, 1)]
		[InlineData(9, 12)]
		[InlineData(42, 63)]
		[InlineData(21, 12)]
		[InlineData(55, 867)]
		[InlineData(49, 32)]
		[InlineData(3, 37)]
		[InlineData(1, 4)]
		[InlineData(1, 3)]
		[InlineData(1, 2)]
		[InlineData(2, 3)]
		public void OppositeFractionsShouldNeutralize(int x, int y)
		{
			Assert.Equal(N(1), (N(x) / N(y)) * (N(y) / N(x)));
			Assert.Equal(N(1), (N(y) / N(x)) * (N(x) / N(y)));
		}

		[Theory]
		[InlineData(0, 1, 5, 2, 10)]
		[InlineData(0, 1, 3, 2, 6)]
		[InlineData(0, 1, 7, 3, 21)]
		[InlineData(1, 1, 5, 12, 10)]
		[InlineData(2, 1, 3, 14, 6)]
		[InlineData(1, 1, 21, 22, 21)]
		public void FractionsShouldSimplify(int nExpected, int pExpected, int qExpected, int pInitial, int qInitial)
		{
			Assert.Equal(N(nExpected) + N(pExpected) / N(qExpected), N(pInitial) / N(qInitial));
		}

		[Theory]
		[InlineData("Pi")]
		[InlineData("E")]
		public void ConstantDividedByItselfShouldGiveOne(string constantName)
		{
			var c = (SymbolicExpression)typeof(ConstantExpression).GetTypeInfo().GetField(constantName).GetValue(null);

			Assert.Equal(1, c / c);
			Assert.Equal(1, c * (1 / c));
			Assert.Equal(1, (1 / c) * c);
		}

		[Fact]
		public void NegativeDividedByNegativeShouldBePositive()
		{
			var t = Var("𝓉");

			Assert.Equal(t / t, (-t) / (-t));
		}

		[Fact]
		public void NegativeMultipliedByNegativeShouldBePositive()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(x * y, (-x) * (-y));
		}

		[Fact]
		public void NegativeSquaredShouldBePositive()
		{
			var t = Var("𝓉");

			Assert.Equal(t * t, (-t) * (-t));
		}

		[Theory]
		[InlineData(2)]
		[InlineData(4)]
		[InlineData(6)]
		[InlineData(8)]
		[InlineData(10)]
		[InlineData(80)]
		[InlineData(444)]
		public void NegativeToEvenPowerShouldBePositive(int power)
		{
			var t = Var("𝓉");

			Assert.Equal(Pow(t, power), Pow(-t, power));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(5)]
		[InlineData(7)]
		[InlineData(9)]
		[InlineData(11)]
		[InlineData(79)]
		[InlineData(333)]
		public void NegativeToOddPowerShouldBeNegative(int power)
		{
			var t = Var("𝓉");

			Assert.Equal(-Pow(t, power), Pow(-t, power));
		}

		[Fact]
		public void ZeroPowerZeroShouldNotSimplify()
		{
			var e = Pow(Zero, Zero);

			Assert.Equal(ExpressionKind.BinaryOperation, e.Kind);
			Assert.Equal(Zero, e.GetFirstOperand());
			Assert.Equal(Zero, e.GetSecondOperand());
		}

		[Theory]
		[InlineData(-10)]
		[InlineData(-3)]
		[InlineData(-2)]
		[InlineData(-1)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(6)]
		[InlineData(25)]
		[InlineData(666)]
		public void NumberToZeroPowerShouldBeOne(int number)
		{
			Assert.Same(One, Pow(number, Zero));
		}

		[Fact]
		public void SquareRootSquaredShouldNegate()
		{
			var t = Var("𝓉");

			Assert.Same(t, Pow(Sqrt(t), 2));
			Assert.Same(t, Sqrt(t) * Sqrt(t));
		}

		[Fact]
		public void SquareRootOfSquareShouldBeAbsoluteValue()
		{
			var t = Var("𝓉");

			Assert.Equal(Abs(t), Sqrt(Pow(t, 2)));
			Assert.Equal(Abs(t), Sqrt(t * t));
		}

		[Theory]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(4)]
		[InlineData(5)]
		[InlineData(666)]
		[InlineData(2552)]
		[InlineData(77777777)]
		public void RootShouldBeNegatedByPower(long value)
		{
			var t = Var("𝓉");

			Assert.Same(t, Pow(Root(t, value), value));
		}

		[Fact]
		public void FirstRootShouldBeIdentity()
		{
			var t = Var("𝓉");

			Assert.Same(t, Root(t, 1));
		}

		[Fact]
		public void SquareOfImaginaryShouldBeMinusOne()
		{
			Assert.Same(MinusOne, I * I);
			Assert.Same(MinusOne, Pow(I, I));
		}

		[Theory]
		[InlineData(-4)]
		[InlineData(-3)]
		[InlineData(-2)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(777)]
		[InlineData(-672)]
		[InlineData(-10)]
		[InlineData(20)]
		public void SquareOfImaginaryNumberShouldBeNegativeSquare(long number)
		{
			var n = N(number) * I;
			long sq = -(number * number);

			Assert.Equal(N(sq), n * n);
			Assert.Equal(N(sq), Pow(n, 2));
		}

		[Fact]
		public void AbsoluteValueOfSquaredShouldGoAway()
		{
			var t = Var("𝓉");

			Assert.NotEqual(t, Abs(t));
			Assert.Equal(t * t, Abs(t * t));
		}

		[Fact]
		public void AbsoluteValueSquaredShouldGoAway()
		{
			var t = Var("𝓉");

			Assert.NotEqual(t, Abs(t));
			Assert.Equal(t * t, Abs(t) * Abs(t));
			Assert.Equal(t * t, Pow(Abs(t), 2));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(-2)]
		[InlineData(-3)]
		[InlineData(-20)]
		[InlineData(-55)]
		[InlineData(-203)]
		[InlineData(-757)]
		[InlineData(-1000652)]
		public void AbsoluteValueOfNegativeNumberIsNegation(long number)
		{
			Assert.Equal(N(-number), Abs(N(number)));
		}

		public static TheoryData<decimal, long, long> DecimalNumberConversionData = new TheoryData<decimal, long, long>
		{
			{ 0.1m, 1, 10 },
			{ 0.25m, 1, 4 },
			{ 0.5m, 1, 2 },
			{ 0.2m, 1, 5 },
			{ 0.8m, 4, 5 },
			{ 0.08m, 4, 50 },
			{ 0.33m, 33, 100 },
			{ 3.14159m, 314159, 100000 },
			{ 1.625m, 13, 8 },
		};

		[Theory]
		[MemberData(nameof(DecimalNumberConversionData))]
		public void DecimalNumbersShouldConvertToFractions(decimal number, long numerator, long denominator)
		{
			Assert.Equal(N(numerator) / N(denominator), N(number));
		}

		[Fact]
		public void DivisionShouldSimplifyAdditions()
		{
			var t = Var("𝓉");

			Assert.Equal(1 + t / 2, (2 + t) / 2);
			Assert.Equal(1 + t + 2 * Pow(t, 2), (2 + 2 * t + 4 * Pow(t, 2)) / 2);
			Assert.Equal(1 + t + 2 * Pow(t, 2), (Pi + Pi * t + 2 * Pi * Pow(t, 2)) / Pi);
		}

		[Fact]
		public void DivisionShouldSimplifyMultiplications()
		{
			var t = Var("𝓉");

			Assert.Equal(t, (2 * t) / 2);
			Assert.Equal(2 * t, (4 * t) / 2);

			Assert.Equal(t, (Pi * t) / Pi);
			Assert.Equal(2 * t, (2 * Pi * t) / Pi);
		}

		[Fact]
		public void UnorderedAdditionsAndSubtractionsShouldCancel()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(N(0), x + y - x - y);
		}

		[Fact]
		public void UnorderedAdditionsAndSubtractionsShouldMerge()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(2 * x + 2 * y, x + y + x + y);
		}

		[Fact]
		public void NumbersAndFractionsShouldAdd()
		{
			Assert.Equal(N(1) / N(2), N(1) - N(1) / N(2));
			Assert.Equal(-N(1) / N(2), -N(1) + N(1) / N(2));
			Assert.Equal(-N(1) / N(2), N(1) / N(2) - N(1));
		}

		[Fact]
		public void ExponentsShouldPropagateIntoDivisions()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(Pow(x, 2) / Pow(y, 2), Pow(x / y, 2));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)] // Prime number
		[InlineData(2)] // Prime number
		[InlineData(3)] // Prime number
		[InlineData(4)]
		[InlineData(5)] // Prime number
		[InlineData(6)]
		[InlineData(7)] // Prime number
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(32)]
		[InlineData(33)]
		[InlineData(81)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(1000)]
		[InlineData(1000000)]
		[InlineData(174)]
		[InlineData(1321)] // Prime number
		[InlineData(691)] // Prime number
		[InlineData(7873)] // Prime number
		public void SquareRootOfPositiveSquareShouldCompute(int number)
		{
			Assert.Equal(N(number), Sqrt(N((long)number * number)));
		}

		[Theory]
		[InlineData(2, 1, 2)] // Sqrt(2) does not simplify
		[InlineData(3, 1, 3)] // Sqrt(3) does not simplify
		[InlineData(6, 1, 6)] // Sqrt(6) does not simplify
		[InlineData(12, 2, 3)] // Sqrt(12) does simplify to 2 * Sqrt(3)
		[InlineData(50, 5, 2)] // Sqrt(50) does simplify to 5 * Sqrt(2)
		[InlineData(71L * 43 * 43 * 211 * 211 * 311 * 311, 43 * 211 * 311, 71)]
		[InlineData(311L * 211 * 211 * 211 * 211, 211 * 211, 311)]
		[InlineData(7L * 4 * 4 * 2097143 * 2097143, 4 * 2097143, 7)]
		public void SquareRootOfPositiveNumberShouldSimplify(long number, int simplified, int remaining)
		{
			Assert.Equal(N(simplified) * Sqrt(N(remaining)), Sqrt(N(number)));
		}

		[Fact]
		public void SquareRootOfMinusOneIsImaginary()
		{
			Assert.Equal(I, Sqrt(-1));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)] // Prime number
		[InlineData(2)] // Prime number
		[InlineData(3)] // Prime number
		[InlineData(4)]
		[InlineData(5)] // Prime number
		[InlineData(6)]
		[InlineData(7)] // Prime number
		[InlineData(8)]
		[InlineData(9)]
		[InlineData(32)]
		[InlineData(33)]
		[InlineData(81)]
		[InlineData(10)]
		[InlineData(100)]
		[InlineData(1000)]
		[InlineData(1000000)]
		[InlineData(174)]
		[InlineData(1321)] // Prime number
		[InlineData(691)] // Prime number
		[InlineData(7873)] // Prime number
		public void SquareRootOfNegativeSquareShouldComputeToImaginary(int number)
		{
			Assert.Equal(N(number) * I, Sqrt(-N((long)number * number)));
		}

		[Theory]
		[InlineData(0, 3, 1, 0)] // ∛0 = 0
		[InlineData(0, 4, 1, 0)] // ∜0 = 0
		[InlineData(1, 3, 1, 1)] // ∛1 = 1
		[InlineData(1, 4, 1, 1)] // ∜1 = 1
		[InlineData(2, 3, 1, 2)] // ∛2 does not simplify
		[InlineData(3, 3, 1, 3)] // ∛3 does not simplify
		[InlineData(4, 3, 1, 4)] // ∛4 does not simplify
		[InlineData(8, 3, 2, 1)] // ∛8 = 2
		[InlineData(27, 3, 3, 1)] // ∛27 = 3
		[InlineData(27 * 8, 3, 6, 1)] // ∛(27 * 8) = 6
		[InlineData(27 * 2, 3, 3, 2)] // ∛(27 * 2) = 3∛2
		[InlineData(16, 4, 2, 1)] // ∜16 = 2
		[InlineData(32, 4, 2, 2)] // ∜32 = 2∜2
		[InlineData(81, 4, 3, 1)] // ∜81 = 3
		[InlineData(2 * 81, 4, 3, 2)] // ∜162 = 3∜2
		[InlineData(7L * 7 * 7 * 7 * 13 * 13 * 13 * 13 * 21 * 3 * 3 * 2 * 2 * 2, 4, 7 * 13, 21 * 3 * 3 * 2 * 2 * 2)]
		public void NthRootOfPositiveNumberShouldSimplify(long number, long exponent, int simplified, int remaining)
		{
			Assert.Equal(N(simplified) * Root(N(remaining), exponent), Root(N(number), exponent));
		}

		[Fact]
		public void RootsWithSameExponentShouldMerge()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");
			var z = Var("𝓏");

			Assert.Equal(Root(x * y, z), Root(x, z) * Root(y, z));
			Assert.Equal(Root(x * y, z), Root(y, z) * Root(x, z));
		}

		[Fact]
		public void LnOfPowerShouldSimplify()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(y * Ln(x), Ln(Pow(x, y)));
		}

		[Fact]
		public void LnShouldCancelExp()
		{
			var x = Var("𝓍");

			Assert.Same(One, Ln(E));
			Assert.Same(x, Ln(Pow(E, x)));
			Assert.Same(x, Ln(Exp(x)));
		}

		[Fact]
		public void ExpShouldCancelLn()
		{
			var x = Var("𝓍");

			Assert.Same(One, Exp(Ln(1)));
			Assert.Same(x, Pow(E, Ln(x)));
			Assert.Same(x, Exp(Ln(x)));
		}

		[Fact]
		public void ExpOfSingleLnShouldConvertToPower()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(Pow(x, y), Exp(y * Ln(x)));
			Assert.Equal(Pow(x, y), Pow(E, y * Ln(x)));
		}

		[Theory]
		[InlineData(2, 2, 4)]
		[InlineData(3, 12, 531441)]
		[InlineData(5, 2, 25)]
		[InlineData(33, 4, 1185921)]
		public void ExpOfSingleLnShouldCompute(int @base, int exponent, long expectedResult)
		{
			Assert.Equal(N(expectedResult), Exp(N(exponent) * Ln(N(@base))));
			Assert.Equal(N(expectedResult), Exp(Ln(N(@base)) * N(exponent)));
			Assert.Equal(N(expectedResult), Pow(E, N(exponent) * Ln(N(@base))));
			Assert.Equal(N(expectedResult), Pow(E, Ln(N(@base)) * N(exponent)));
		}

		[Fact]
		public void ExpOfMultipleLnShouldNotConvertToPower()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");
			var z = Var("𝓏");

			bool IsExp(SymbolicExpression e) => e.IsPower() && e.GetFirstOperand().Equals(E);

			Assert.True(IsExp(Exp(Ln(x) * Ln(y))));
			Assert.True(IsExp(Exp(Ln(y) * Ln(x))));

			Assert.True(IsExp(Exp(Ln(x) * Ln(y) * Ln(z))));
			Assert.True(IsExp(Exp(Ln(x) * Ln(z) * Ln(y))));
			Assert.True(IsExp(Exp(Ln(y) * Ln(x) * Ln(z))));
			Assert.True(IsExp(Exp(Ln(y) * Ln(z) * Ln(x))));
			Assert.True(IsExp(Exp(Ln(z) * Ln(x) * Ln(y))));
			Assert.True(IsExp(Exp(Ln(z) * Ln(y) * Ln(x))));
		}

		[Fact]
		public void ExpOfSingleLnFactorShouldConvertToPower()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");
			var z = Var("𝓏");
			
			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, y, x), Exp(x * Ln(y)));
			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, y, x), Exp(Ln(y) * x));

			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, x, y), Exp(y * Ln(x)));
			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, x, y), Exp(Ln(x) * y));

			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, y, x * z), Exp(x * Ln(y) * z));
			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, y, x * z), Exp(z * Ln(y) * x));
			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, y, x * z), Exp(Ln(y) * x * z));
			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, y, x * z), Exp(Ln(y) * z * x));
			Assert.Equal(new BinaryOperationExpression(BinaryOperator.Power, y, x * z), Exp(x * z * Ln(y)));
		}
	}
}
