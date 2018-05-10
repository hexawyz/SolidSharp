using SolidSharp.Vectors;
using Xunit;
using static SolidSharp.Expressions.SymbolicMath;

namespace SolidSharp.Tests.Vectors
{
	public sealed class Vector2dTests
    {
		[Fact]
		public static void TranslationMatricesCanInverse()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			var m = Matrix3x2.Translate(x, y);
			var im = m.Invert();

			Assert.NotEqual(Matrix3x2.Identity, m);
			Assert.NotEqual(Matrix3x2.Identity, im);

			var a = m * im;
			var b = im * m;

			Assert.Equal(a, b);
			
			Assert.Equal(Matrix3x2.Identity, a);
			Assert.Equal(Matrix3x2.Identity, b);
		}

		[Fact]
		public static void AbstractScalingMatricesCanInverse()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			var m = Matrix3x2.Scale(x, y);
			var im = m.Invert();

			// NB: Expressions such as x / y cannot simplify when y is a variable.
			// Because of this, we can't verify M*M⁻¹ and M⁻¹*M…

			Assert.NotEqual(Matrix3x2.Identity, m);
			Assert.NotEqual(Matrix3x2.Identity, im);
		}

		[Fact]
		public static void IdentityMatrixInverseIsIdentity()
		{
			var m = Matrix3x2.Identity;
			var im = m.Invert();

			Assert.Equal(Matrix3x2.Identity, m);
			Assert.Equal(Matrix3x2.Identity, im);

			var a = m * im;
			var b = im * m;

			Assert.Equal(Matrix3x2.Identity, a);
			Assert.Equal(Matrix3x2.Identity, b);
		}

		[Theory]
		[InlineData(1, 2)]
		[InlineData(2, 1)]
		[InlineData(2, 2)]
		[InlineData(50, 3)]
		[InlineData(29, 180)]
		public static void ConcreteScalingMatricesCanInverse(int sx, int sy)
		{
			var m = Matrix3x2.Scale(sx, sy);
			var im = m.Invert();

			Assert.NotEqual(Matrix3x2.Identity, m);
			Assert.NotEqual(Matrix3x2.Identity, im);

			var a = m * im;
			var b = im * m;

			Assert.Equal(a, b);

			Assert.Equal(Matrix3x2.Identity, a);
			Assert.Equal(Matrix3x2.Identity, b);
		}

		[Fact]
		public static void AbstractRotationMatricesCanInverse()
		{
			var theta = Var("𝜃");

			var m = Matrix3x2.Rotate(theta);
			var im = m.Invert();

			// TODO: Improve support for trigonometric functions

			Assert.NotEqual(Matrix3x2.Identity, m);
			Assert.NotEqual(Matrix3x2.Identity, im);
		}
	}
}
