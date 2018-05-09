using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	/// <summary>A helper interface used to provide information about an expression without casting.</summary>
	internal interface IExpression
	{
		/// <summary>Gets a value indicating if the expression represents any kind of operation.</summary>
		bool IsOperation { get; }

		/// <summary>Gets a value indicating if the expression represents a unary operation.</summary>
		bool IsUnaryOperation { get; }
		/// <summary>Gets a value indicating if the expression represents a binary operation.</summary>
		bool IsBinaryOperation { get; }
		/// <summary>Gets a value indicating if the expression represents a n-ary operation.</summary>
		bool IsVariadicOperation { get; }

		/// <summary>Gets a value indicating if the expression represents a negation.</summary>
		bool IsNegation { get; }

		/// <summary>Gets a value indicating if the expression represents an addition.</summary>
		bool IsAddition { get; }
		/// <summary>Gets a value indicating if the expression represents a subtraction.</summary>
		bool IsSubtraction { get; }
		/// <summary>Gets a value indicating if the expression represents a multiplication.</summary>
		bool IsMultiplication { get; }
		/// <summary>Gets a value indicating if the expression represents a division.</summary>
		bool IsDivision { get; }

		/// <summary>Gets a value indicating if the expression represents a power.</summary>
		bool IsPower { get; }
		/// <summary>Gets a value indicating if the expression represents a root.</summary>
		bool IsRoot { get; }

		/// <summary>Gets a value indicating if the expression represents a mathematical function.</summary>
		bool IsMathematicalFunction { get; }

		/// <summary>Gets a value indicating if the expression represents a number.</summary>
		bool IsNumber { get; }
		/// <summary>Gets a value indicating if the expression represents a positive number.</summary>
		bool IsPositiveNumber { get; }
		/// <summary>Gets a value indicating if the expression represents a negative number.</summary>
		bool IsNegativeNumber { get; }
		/// <summary>Gets a value indicating if the expression represents an odd number.</summary>
		bool IsOddNumber { get; }
		/// <summary>Gets a value indicating if the expression represents an even number.</summary>
		bool IsEvenNumber { get; }

		/// <summary>Gets a value indicating if the expression represents a variable.</summary>
		bool IsVariable { get; }
		/// <summary>Gets a value indicating if the expression represents a mathematic constant.</summary>
		bool IsConstant { get; }

		/// <summary>Gets the operand of the expression.</summary>
		/// <remarks>
		/// This method will only succeed for expressions that represent unary operations.
		/// You should always verify the kind of the expression before trying to retrieve its operands.
		/// </remarks>
		/// <param name="e">The expression.</param>
		/// <returns>The operand of the expression.</returns>
		/// <exception cref="NotSupportedException">This kind of expression doesn't have exactly one operand.</exception>
		SymbolicExpression GetOperand();

		/// <summary>Gets the operands of the expression.</summary>
		/// <remarks>
		/// This method will only succeed for expressions that represent n-ary operations.
		/// You should always verify the kind of the expression before trying to retrieve its operands.
		/// </remarks>
		/// <param name="e">The expression.</param>
		/// <returns>An array containing the operands of the expression.</returns>
		/// <exception cref="NotSupportedException">This kind of expression doesn't have any operand.</exception>
		ImmutableArray<SymbolicExpression> GetOperands();
	}
}
