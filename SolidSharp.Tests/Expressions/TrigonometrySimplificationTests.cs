using Xunit;
using static SolidSharp.Expressions.SymbolicMath;

namespace SolidSharp.Tests.Expressions
{
	public sealed class TrigonometrySimplificationTests
	{
		[Fact]
		public void SineOfZeroShouldBeZero()
		{
			Assert.Equal(Zero, Sin(Zero));
		}

		[Fact]
		public void CosineOfZeroShouldBeOne()
		{
			Assert.Equal(One, Cos(Zero));
		}

		[Theory]
		[InlineData(-35)]
		[InlineData(-2)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(7)]
		[InlineData(22)]
		[InlineData(6654354190)]
		public void SineOfPiMultiplesShouldBeZero(long n)
		{
			Assert.Equal(Zero, Sin(n * Pi));
			Assert.Equal(Zero, Sin(Pi * n));
		}

		[Theory]
		[InlineData(-35)]
		[InlineData(-3)]
		[InlineData(-1)]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(5)]
		[InlineData(7)]
		[InlineData(23)]
		[InlineData(6654354191)]
		public void SineOfHalfPiMultiplesShouldBeOneOrMinusOne(long n)
		{
			var expected = (n & 2) != 0 ? MinusOne : One;

			Assert.Equal(expected, Sin(N(n) * Pi / N(2)));
			Assert.Equal(expected, Sin(N(n) * (Pi / N(2))));
			Assert.Equal(expected, Sin((Pi * N(n)) / N(2)));
			Assert.Equal(expected, Sin(Pi * (N(n) / N(2))));
		}

		[Theory]
		[InlineData(-35)]
		[InlineData(-3)]
		[InlineData(-1)]
		[InlineData(1)]
		[InlineData(3)]
		[InlineData(5)]
		[InlineData(7)]
		[InlineData(23)]
		[InlineData(6654354191)]
		public void CosineOfHalfPiMultiplesShouldBeZero(long n)
		{
			Assert.Equal(Zero, Cos(N(n) * Pi / N(2)));
			Assert.Equal(Zero, Cos(N(n) * (Pi / N(2))));
			Assert.Equal(Zero, Cos((Pi * N(n)) / N(2)));
			Assert.Equal(Zero, Cos(Pi * (N(n) / N(2))));
		}
	}
}
