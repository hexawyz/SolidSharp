using System;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public static class SymbolicExpressionExtensions
	{
		public static bool IsNegation(this SymbolicExpression e)
			=> ((IExpression)e).IsNegation;

		public static bool IsAddition(this SymbolicExpression e)
			=> ((IExpression)e).IsAddition;

		public static bool IsSubtraction(this SymbolicExpression e)
			=> ((IExpression)e).IsSubtraction;

		public static bool IsMultiplication(this SymbolicExpression e)
			=> ((IExpression)e).IsMultiplication;

		public static bool IsDivision(this SymbolicExpression e)
			=> ((IExpression)e).IsDivision;

		public static bool IsPower(this SymbolicExpression e)
			=> ((IExpression)e).IsPower;

		public static bool IsNumber(this SymbolicExpression e)
			=> ((IExpression)e).IsNumber;

		public static bool IsInteger(this SymbolicExpression e)
			=> ((IExpression)e).IsInteger;

		public static bool IsZero(this SymbolicExpression e)
			=> ReferenceEquals(e, NumberExpression.Zero); // This will work because we're making sure that Zero is a singleton.

		public static bool IsOne(this SymbolicExpression e)
			=> ReferenceEquals(e, NumberExpression.One); // This will work because we're making sure that One is a singleton.

		public static bool IsMinusOne(this SymbolicExpression e)
			=> ReferenceEquals(e, NumberExpression.MinusOne); // This will work because we're making sure that MinusOne is a singleton.

		/// <summary>Gets the operand of the expression.</summary>
		/// <remarks>
		/// This method will only succeed for expressions that represent unary operations.
		/// You should always verify the kind of the expression before trying to retrieve its operands.
		/// </remarks>
		/// <param name="e">The expression.</param>
		/// <returns>The operand of the expression.</returns>
		/// <exception cref="NotSupportedException">This kind of expression doesn't have exactly one operand.</exception>
		public static SymbolicExpression GetOperand(this SymbolicExpression e)
			=> ((IExpression)e).GetOperand();

		/// <summary>Gets the operands of the expression.</summary>
		/// <remarks>
		/// This method will only succeed for expressions that represent n-ary operations.
		/// You should always verify the kind of the expression before trying to retrieve its operands.
		/// </remarks>
		/// <param name="e">The expression.</param>
		/// <returns>An array containing the operands of the expression.</returns>
		/// <exception cref="NotSupportedException">This kind of expression doesn't have any operand.</exception>
		public static ImmutableArray<SymbolicExpression> GetOperands(this SymbolicExpression e)
			=> ((IExpression)e).GetOperands();
	}
}
