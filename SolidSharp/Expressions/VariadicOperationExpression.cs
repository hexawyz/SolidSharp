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

		bool IExpression.IsUnaryOperation => false;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => true;

		bool IExpression.IsNegation => false;

		bool IExpression.IsAddition => Operator == VariadicOperator.Addition;
		bool IExpression.IsSubtraction => false;
		bool IExpression.IsMultiplication => Operator == VariadicOperator.Multiplication;
		bool IExpression.IsDivision => false;
		bool IExpression.IsPower => false;

		bool IExpression.IsMathematicalFunction => false;

		bool IExpression.IsNumber => false;
		bool IExpression.IsInteger => false;
		bool IExpression.IsPositiveInteger => false;
		bool IExpression.IsNegativeInteger => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => Operands;

		#endregion
	}
}
