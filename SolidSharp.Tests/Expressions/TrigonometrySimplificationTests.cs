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
	}
}
