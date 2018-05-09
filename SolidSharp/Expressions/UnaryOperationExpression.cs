using SolidSharp.Expressions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class UnaryOperationExpression : SymbolicExpression , IEquatable<UnaryOperationExpression>, IExpression
	{
		private static readonly Dictionary<UnaryOperator, string> OperatorToString = new Dictionary<UnaryOperator, string>
		{
			{ UnaryOperator.Plus, "+" },
			{ UnaryOperator.Minus, "-" },
			{ UnaryOperator.Sin, "sin" },
			{ UnaryOperator.Cos, "cos" },
			{ UnaryOperator.Tan, "tan" },
			{ UnaryOperator.Ln, "ln" },
		};

		public UnaryOperator Operator { get; set; }

		public SymbolicExpression Operand { get; set; }

		public override ExpressionKind Kind => ExpressionKind.UnaryOperation;

		internal UnaryOperationExpression(UnaryOperator @operator, SymbolicExpression operand)
		{
			Operator = @operator;
			Operand = operand ?? throw new ArgumentNullException(nameof(operand));
		}

		public override string ToString()
		{
			if (Operator == UnaryOperator.Abs)
			{
				return "|" + Operand + "|";
			}

			bool parenthesesRequired = Operator.IsFunction() || Operand.NeedsParentheses();
			string @operator = OperatorToString[Operator];

			return parenthesesRequired ?
				@operator + "(" + Operand.ToString() + ")" :
				@operator + Operand.ToString();
		}

		public bool Equals(UnaryOperationExpression other)
			=> ReferenceEquals(this, other)
			|| (!(other is null) && Operator == other.Operator && Operand.Equals(other.Operand));

		public override bool Equals(object obj)
			=> Equals(obj as UnaryOperationExpression);

		public override int GetHashCode()
		{
			var hashCode = -403254203;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + Operator.GetHashCode();
			hashCode = hashCode * -1521134295 + Operand.GetHashCode();
			return hashCode;
		}
		
		#region IExpression Helpers

		bool IExpression.IsOperation => false;
		bool IExpression.IsUnaryOperation => true;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;
		bool IExpression.NeedsParentheses => !Operator.IsFunction();

		bool IExpression.IsNegation => Operator == UnaryOperator.Minus;
		bool IExpression.IsAbsoluteValue => Operator == UnaryOperator.Abs;

		bool IExpression.IsAddition => false;
		bool IExpression.IsSubtraction => false;
		bool IExpression.IsMultiplication => false;
		bool IExpression.IsDivision => false;

		bool IExpression.IsPower => false;
		bool IExpression.IsRoot => false;

		bool IExpression.IsMathematicalFunction => Operator.IsFunction();

		bool IExpression.IsNumber => false;
		bool IExpression.IsPositiveNumber => false;
		bool IExpression.IsNegativeNumber => false;
		bool IExpression.IsOddNumber => false;
		bool IExpression.IsEvenNumber => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		byte IExpression.GetPrecedence() => 0;
		SymbolicExpression IExpression.GetOperand() => Operand;
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => ImmutableArray.Create(Operand);

		#endregion
	}
}
