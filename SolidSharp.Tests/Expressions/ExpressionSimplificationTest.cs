using SolidSharp.Expressions;
using System.Collections.Immutable;
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
		public void PowersShouldSum()
		{
			var t = Var("𝓉");
			var x = Var("𝓍");
			var y = Var("𝓎");
			
			Assert.Equal(Pow(t, x + y), Pow(t, x) * Pow(t, y));
		}

		[Fact]
		public void PowersShouldSubstract()
		{
			var t = Var("𝓉");
			var x = Var("𝓍");
			var y = Var("𝓎");
			
			Assert.Equal(Pow(t, x - y), Pow(t, x) / Pow(t, y));
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
		public void FractionsShouldNeutralize(int x, int y)
		{
			Assert.Equal(N(1), (N(x) / N(y)) * (N(y) / N(x)));
			Assert.Equal(N(1), (N(y) / N(x)) * (N(x) / N(y)));
		}

		[Theory]
		[InlineData(1, 5, 2, 10)]
		[InlineData(1, 3, 2, 6)]
		[InlineData(1, 7, 3, 21)]
		public void FractionsShouldSimplify(int pa, int qa, int pb, int qb)
		{
			Assert.Equal(N(pa) / N(qa), N(pb) / N(qb));
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
	}
}
