using System;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class UnaryOperationExpression : SymbolicExpression , IEquatable<UnaryOperationExpression>, IExpression
	{
		public UnaryOperator Operator { get; set; }

		public SymbolicExpression Operand { get; set; }

		public override ExpressionKind Kind => ExpressionKind.UnaryOperation;

		internal UnaryOperationExpression(UnaryOperator @operator, SymbolicExpression operand)
		{
			Operator = @operator;
			Operand = operand ?? throw new ArgumentNullException(nameof(operand));
		}

		public bool Equals(UnaryOperationExpression other)
			=> ReferenceEquals(this, other)
			|| (!(other is null) && Operator == other.Operator && Operand.Equals(other.Operand));

		public override bool Equals(object obj)
			=> Equals(obj as BinaryOperationExpression);

		public override int GetHashCode()
		{
			var hashCode = -403254203;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + Operator.GetHashCode();
			hashCode = hashCode * -1521134295 + Operand.GetHashCode();
			return hashCode;
		}
		
		#region IExpression Helpers

		bool IExpression.IsUnaryOperation => true;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;

		bool IExpression.IsNegation => Operator == UnaryOperator.Minus;

		bool IExpression.IsAddition => false;
		bool IExpression.IsSubtraction => false;
		bool IExpression.IsMultiplication => false;
		bool IExpression.IsDivision => false;
		bool IExpression.IsPower => false;

		bool IExpression.IsMathematicalFunction => Operator.IsFunction();

		bool IExpression.IsNumber => false;
		bool IExpression.IsPositiveNumber => false;
		bool IExpression.IsNegativeNumber => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		SymbolicExpression IExpression.GetOperand() => Operand;
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => ImmutableArray.Create(Operand);

		#endregion
	}
}
