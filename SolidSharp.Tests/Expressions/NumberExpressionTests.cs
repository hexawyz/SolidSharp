using SolidSharp.Expressions;
using Xunit;

namespace SolidSharp.Tests.Expressions
{
	public sealed class NumberExpressionTests
    {
		[Fact]
		public void SmallNumbersShouldBeCached()
		{
			for (int i = -128; i < 128; i++)
			{
				Assert.Same(NumberExpression.Create(i), NumberExpression.Create(i));
			}
		}

		[Theory]
		[InlineData(128)]
		[InlineData(-129)]
		[InlineData(int.MinValue)]
		[InlineData(int.MaxValue)]
		public void LargeNumbersShouldNotBeCached(int number)
		{
			Assert.NotSame(NumberExpression.Create(number), NumberExpression.Create(number));
		}

		[Fact]
		public void SByteValuesShouldBeMappedProperly()
		{
			for (int i = -128; i < 128; i++)
			{
				Assert.Equal(i, NumberExpression.Create(checked((sbyte)i)).Value);
			}
		}

		[Fact]
		public void ByteValuesShouldBeMappedProperly()
		{
			for (int i = 0; i < 256; i++)
			{
				Assert.Equal(i, NumberExpression.Create(checked((byte)i)).Value);
			}
		}

		[Theory]
		[InlineData(short.MinValue)]
		[InlineData(-151)]
		[InlineData(-129)]
		[InlineData(-128)]
		[InlineData(-7)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(127)]
		[InlineData(128)]
		[InlineData(777)]
		[InlineData(12978)]
		[InlineData(short.MaxValue)]
		public void Int16ValuesShouldBeMappedProperly(short value)
		{
			Assert.Equal(value, NumberExpression.Create(value).Value);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(127)]
		[InlineData(128)]
		[InlineData(777)]
		[InlineData(12978)]
		[InlineData(short.MaxValue)]
		[InlineData(ushort.MaxValue)]
		public void UInt16ValuesShouldBeMappedProperly(ushort value)
		{
			Assert.Equal(value, NumberExpression.Create(value).Value);
		}

		[Theory]
		[InlineData(int.MinValue)]
		[InlineData(short.MinValue)]
		[InlineData(-151)]
		[InlineData(-129)]
		[InlineData(-128)]
		[InlineData(-7)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(127)]
		[InlineData(128)]
		[InlineData(777)]
		[InlineData(12978)]
		[InlineData(short.MaxValue)]
		[InlineData(ushort.MaxValue)]
		[InlineData(int.MaxValue)]
		public void Int32ValuesShouldBeMappedProperly(int value)
		{
			Assert.Equal(value, NumberExpression.Create(value).Value);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(127)]
		[InlineData(128)]
		[InlineData(777)]
		[InlineData(12978)]
		[InlineData(short.MaxValue)]
		[InlineData(ushort.MaxValue)]
		[InlineData(int.MaxValue)]
		[InlineData(uint.MaxValue)]
		public void UInt32ValuesShouldBeMappedProperly(uint value)
		{
			Assert.Equal(value, NumberExpression.Create(value).Value);
		}

		[Theory]
		[InlineData(long.MinValue)]
		[InlineData(int.MinValue)]
		[InlineData(short.MinValue)]
		[InlineData(-151)]
		[InlineData(-129)]
		[InlineData(-128)]
		[InlineData(-7)]
		[InlineData(-1)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(127)]
		[InlineData(128)]
		[InlineData(777)]
		[InlineData(12978)]
		[InlineData(short.MaxValue)]
		[InlineData(ushort.MaxValue)]
		[InlineData(int.MaxValue)]
		[InlineData(uint.MaxValue)]
		[InlineData(long.MaxValue)]
		public void Int64ValuesShouldBeMappedProperly(long value)
		{
			Assert.Equal(value, NumberExpression.Create(value).Value);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		[InlineData(127)]
		[InlineData(128)]
		[InlineData(777)]
		[InlineData(12978)]
		[InlineData(short.MaxValue)]
		[InlineData(ushort.MaxValue)]
		[InlineData(int.MaxValue)]
		[InlineData(uint.MaxValue)]
		[InlineData(long.MaxValue)]
		[InlineData(ulong.MaxValue)]
		public void UInt64ValuesShouldBeMappedProperly(ulong value)
		{
			Assert.Equal(value, NumberExpression.Create(value).Value);
		}
	}
}
