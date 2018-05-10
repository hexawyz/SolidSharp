using SolidSharp.Expressions;
using System.Collections.Generic;
using Xunit;
using static SolidSharp.Expressions.SymbolicMath;

namespace SolidSharp.Tests.Expressions
{
	public sealed class ExpressionSubstitutionTests
	{
		[Fact]
		public void VariableShouldSubstituteAnotherVariable()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			Assert.Equal(y, x.SubstituteVariables(new Dictionary<string, SymbolicExpression> { { "𝓍", y } }));
			Assert.Equal(x, y.SubstituteVariables(new Dictionary<string, SymbolicExpression> { { "𝓎", x } }));
		}

		[Fact]
		public void UnrelatedVariablesShouldNotBeSubstituted()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");
			var z = Var("𝓏");

			Assert.Same(x, x.SubstituteVariables(new Dictionary<string, SymbolicExpression> { { "𝓏", y } }));

			var expr = x + y;
			Assert.Same(expr, expr.SubstituteVariables(new Dictionary<string, SymbolicExpression> { { "𝓏", y } }));
		}

		[Fact]
		public void VariableShouldBeSubstitutedWithOtherExpressions()
		{
			var x = Var("𝓍");
			var y = Var("𝓎");

			var a = 3 * Pow(x, 2) - 3 * y / 12;
			var b = 3 * Pow(x, 2) - 6 * x / 12;
			Assert.Equal(b, a.SubstituteVariables(new Dictionary<string, SymbolicExpression> { { "𝓎", 2 * x } }));
			Assert.Equal(760, b.SubstituteVariables(new Dictionary<string, SymbolicExpression> { { "𝓍", 16 } }));
			Assert.Equal(760, a.SubstituteVariables(new Dictionary<string, SymbolicExpression> { { "𝓍", 16 }, { "𝓎", 32 } }));
		}
	}
}
