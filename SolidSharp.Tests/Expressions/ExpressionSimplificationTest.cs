﻿using SolidSharp.Expressions;
using System.Collections.Immutable;
using Xunit;

namespace SolidSharp.Tests.Expressions
{
	public sealed class ExpressionSimplificationTest
    {
		[Fact]
		public void DoubleNegationShouldBeNegated()
		{
			var t = SymbolicExpression.Variable("𝓉");

			Assert.Same(t, SymbolicExpression.Negate(SymbolicExpression.Negate(t)));
			Assert.Same(t, -(-t));
		}

		[Fact]
		public void PowersShouldSum()
		{
			var t = SymbolicExpression.Variable("𝓉");
			var x = SymbolicExpression.Variable("𝓍");
			var y = SymbolicExpression.Variable("𝓎");

			var expected = SymbolicMath.Pow(t, SymbolicExpression.Add(x, y));
			Assert.Equal(expected, SymbolicMath.Pow(t, x + y)); // Verify that everything parses correctly

			Assert.Equal(expected, SymbolicExpression.Multiply(SymbolicMath.Pow(t, x), SymbolicMath.Pow(t, y)));
			Assert.Equal(expected, SymbolicMath.Pow(t, x) * SymbolicMath.Pow(t, y));
		}

		[Fact]
		public void PowersShouldSubstract()
		{
			var t = SymbolicExpression.Variable("𝓉");
			var x = SymbolicExpression.Variable("𝓍");
			var y = SymbolicExpression.Variable("𝓎");

			var expected = SymbolicMath.Pow(t, SymbolicExpression.Subtract(x, y));
			Assert.Equal(expected, SymbolicMath.Pow(t, x - y)); // Verify that everything parses correctly

			Assert.Equal(expected, SymbolicExpression.Divide(SymbolicMath.Pow(t, x), SymbolicMath.Pow(t, y)));
			Assert.Equal(expected, SymbolicMath.Pow(t, x) / SymbolicMath.Pow(t, y));
		}

		[Fact]
		public void AdditionsShouldMerge()
		{
			var x = SymbolicExpression.Variable("𝓍");
			var y = SymbolicExpression.Variable("𝓎");
			var z = SymbolicExpression.Variable("𝓏");
			var w = SymbolicExpression.Variable("𝓌");

			var expected = SymbolicExpression.Add(ImmutableArray.Create<SymbolicExpression>(x, y, z, w));

			Assert.Equal(expected, SymbolicExpression.Add(x, SymbolicExpression.Add(y, SymbolicExpression.Add(z, w))));
			Assert.Equal(expected, SymbolicExpression.Add(x, SymbolicExpression.Add(SymbolicExpression.Add(y, z), w)));
			Assert.Equal(expected, SymbolicExpression.Add(SymbolicExpression.Add(x, y), SymbolicExpression.Add(z, w)));
			Assert.Equal(expected, SymbolicExpression.Add(SymbolicExpression.Add(x, SymbolicExpression.Add(y, z)), w));
			Assert.Equal(expected, SymbolicExpression.Add(SymbolicExpression.Add(SymbolicExpression.Add(x, y), z), w));

			Assert.Equal(expected, x + (y + (z + w)));
			Assert.Equal(expected, x + ((y + z) + w));
			Assert.Equal(expected, (x + y) + (z + w));
			Assert.Equal(expected, (x + (y + z)) + w);
			Assert.Equal(expected, ((x + y) + z) + w);
		}

		[Fact]
		public void IdentitySubtractionsShouldCancel()
		{
			var t = SymbolicExpression.Variable("𝓉");
			var zero = SymbolicExpression.Constant(0);

			Assert.Same(zero, SymbolicExpression.Subtract(t, t));
			Assert.Same(zero, t - t);
		}

		[Fact]
		public void NegationShouldConvertToSubtraction()
		{
			var t = SymbolicExpression.Variable("𝓉");
			var x = SymbolicExpression.Variable("𝓍");
			var y = SymbolicExpression.Variable("𝓎");
			
			var expected1 = SymbolicExpression.Subtract(x, y);
			Assert.Equal(expected1, SymbolicExpression.Add(x, SymbolicExpression.Negate(y)));
			Assert.Equal(expected1, x + (-y));

			var expected2 = SymbolicExpression.Subtract(y, x);
			Assert.Equal(expected2, SymbolicExpression.Add(SymbolicExpression.Negate(x), y));
			Assert.Equal(expected2, (-x) + y);

			var zero = SymbolicExpression.Constant(0);
			Assert.Same(zero, SymbolicExpression.Add(t, SymbolicExpression.Negate(t)));
			Assert.Same(zero, t + (-t));
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
			Assert.Equal(SymbolicExpression.Constant(1), (SymbolicExpression.Constant(x) / SymbolicExpression.Constant(y)) * (SymbolicExpression.Constant(y) / SymbolicExpression.Constant(x)));
			Assert.Equal(SymbolicExpression.Constant(1), (SymbolicExpression.Constant(y) / SymbolicExpression.Constant(x)) * (SymbolicExpression.Constant(x) / SymbolicExpression.Constant(y)));
		}

		[Theory]
		[InlineData(1, 5, 2, 10)]
		[InlineData(1, 3, 2, 6)]
		[InlineData(1, 7, 3, 21)]
		public void FractionsShouldSimplify(int pa, int qa, int pb, int qb)
		{
			Assert.Equal(SymbolicExpression.Constant(pa) / SymbolicExpression.Constant(qa), SymbolicExpression.Constant(pb) / SymbolicExpression.Constant(qb));
		}

		[Fact]
		public void SquareRootSquaredShouldNegate()
		{
			var t = SymbolicExpression.Variable("𝓉");

			Assert.Same(t, SymbolicMath.Pow(SymbolicMath.Sqrt(t), 2));
			Assert.Same(t, SymbolicMath.Sqrt(t) * SymbolicMath.Sqrt(t));
		}

		[Fact]
		public void SquareRootOfSquareShouldBeAbsoluteValue()
		{
			var t = SymbolicExpression.Variable("𝓉");

			Assert.Equal(SymbolicMath.Abs(t), SymbolicMath.Sqrt(SymbolicMath.Pow(t, 2)));
			Assert.Equal(SymbolicMath.Abs(t), SymbolicMath.Sqrt(t * t));
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
			var t = SymbolicExpression.Variable("𝓉");

			Assert.Same(t, SymbolicMath.Pow(SymbolicMath.Root(t, value), value));
		}

		[Fact]
		public void AbsoluteValueOfSquaredShouldGoAway()
		{
			var t = SymbolicExpression.Variable("𝓉");

			Assert.NotEqual(t, SymbolicMath.Abs(t));
			Assert.Equal(t * t, SymbolicMath.Abs(t * t));
		}

		[Fact]
		public void AbsoluteValueSquaredShouldGoAway()
		{
			var t = SymbolicExpression.Variable("𝓉");

			Assert.NotEqual(t, SymbolicMath.Abs(t));
			Assert.Equal(t * t, SymbolicMath.Abs(t) * SymbolicMath.Abs(t));
			Assert.Equal(t * t, SymbolicMath.Pow(SymbolicMath.Abs(t), 2));
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
			Assert.Equal(SymbolicExpression.Constant(numerator) / SymbolicExpression.Constant(denominator), SymbolicExpression.Constant(number));
		}
	}
}