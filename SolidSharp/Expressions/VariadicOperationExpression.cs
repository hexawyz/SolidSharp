using SolidSharp.Expressions.Extensions;
using System;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class VariadicOperationExpression : SymbolicExpression, IEquatable<VariadicOperationExpression>, IExpression
    {
		public VariadicOperator Operator { get; }
		public ImmutableArray<SymbolicExpression> Operands { get; }

		internal VariadicOperationExpression(VariadicOperator @operator, ImmutableArray<SymbolicExpression> operands)
		{
			if (operands.IsDefaultOrEmpty || operands.Length < 2) throw new ArgumentException();

			Operator = @operator;
			Operands = operands;
		}

		public override ExpressionKind Kind => ExpressionKind.CommutativeOperation;

		public bool Equals(VariadicOperationExpression other)
			=> ReferenceEquals(this, other)
			|| !(other is null) && Operator == other.Operator && StructuralEqualityComparer.Equals(Operands, other.Operands);

		#region IExpression Helpers

		bool IExpression.IsOperation => false;
		bool IExpression.IsUnaryOperation => false;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => true;
		bool IExpression.NeedsParentheses => true;

		bool IExpression.IsNegation => false;
		bool IExpression.IsAbsoluteValue => false;

		bool IExpression.IsAddition => Operator == VariadicOperator.Addition;
		bool IExpression.IsSubtraction => false;
		bool IExpression.IsMultiplication => Operator == VariadicOperator.Multiplication;
		bool IExpression.IsDivision => false;

		bool IExpression.IsPower => false;
		bool IExpression.IsRoot => false;

		bool IExpression.IsMathematicalFunction => false;

		bool IExpression.IsNumber => false;
		bool IExpression.IsPositiveNumber => false;
		bool IExpression.IsNegativeNumber => false;
		bool IExpression.IsOddNumber => false;
		bool IExpression.IsEvenNumber => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		byte IExpression.GetPrecedence() => Operator.GetPrecedence();
		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => Operands;

		#endregion
	}
}
