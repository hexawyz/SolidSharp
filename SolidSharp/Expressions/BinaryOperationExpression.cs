using System;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class BinaryOperationExpression : SymbolicExpression , IEquatable<BinaryOperationExpression>, IExpression
	{
		public BinaryOperator Operator { get; set; }

		public SymbolicExpression FirstOperand { get; set; }

		public SymbolicExpression SecondOperand { get; set; }

		public override ExpressionKind Kind => ExpressionKind.BinaryOperation;

		internal BinaryOperationExpression(BinaryOperator @operator, SymbolicExpression firstOperand, SymbolicExpression secondOperand)
		{
			Operator = @operator;
			FirstOperand = firstOperand ?? throw new ArgumentNullException(nameof(firstOperand));
			SecondOperand = secondOperand ?? throw new ArgumentNullException(nameof(secondOperand));
		}

		public bool Equals(BinaryOperationExpression other)
			=> ReferenceEquals(this, other)
			|| (!(other is null)&& Operator == other.Operator && FirstOperand.Equals(other.FirstOperand) && SecondOperand.Equals(other.SecondOperand));

		public override bool Equals(object obj)
			=> Equals(obj as BinaryOperationExpression);

		public override int GetHashCode()
		{
			var hashCode = -403254203;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + Operator.GetHashCode();
			hashCode = hashCode * -1521134295 + FirstOperand.GetHashCode();
			hashCode = hashCode * -1521134295 + SecondOperand.GetHashCode();
			return hashCode;
		}

		#region IExpression Helpers

		bool IExpression.IsUnaryOperation => true;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;

		bool IExpression.IsNegation => false;

		bool IExpression.IsAddition => Operator == BinaryOperator.Addition;
		bool IExpression.IsSubtraction => Operator == BinaryOperator.Subtraction;
		bool IExpression.IsMultiplication => Operator == BinaryOperator.Multiplication;
		bool IExpression.IsDivision => Operator == BinaryOperator.Division;
		bool IExpression.IsPower => Operator == BinaryOperator.Power;

		bool IExpression.IsMathematicalFunction => false;

		bool IExpression.IsNumber => false;
		bool IExpression.IsPositiveNumber => false;
		bool IExpression.IsNegativeNumber => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => ImmutableArray.Create(FirstOperand, SecondOperand);

		#endregion
	}
}
