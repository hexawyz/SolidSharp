using SolidSharp.Expressions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class VariadicOperationExpression : SymbolicExpression, IEquatable<VariadicOperationExpression>, IExpression
	{
		private static readonly Dictionary<VariadicOperator, string> OperatorToString = new Dictionary<VariadicOperator, string>
		{
			{ VariadicOperator.Addition, " + " },
			{ VariadicOperator.Multiplication, " × " },
		};

		public VariadicOperator Operator { get; }
		public ImmutableArray<SymbolicExpression> Operands { get; }

		internal VariadicOperationExpression(VariadicOperator @operator, ImmutableArray<SymbolicExpression> operands)
		{
			if (operands.IsDefaultOrEmpty || operands.Length < 2) throw new ArgumentException();

			Operator = @operator;
			Operands = operands;
		}

		public override ExpressionKind Kind => ExpressionKind.CommutativeOperation;

		protected internal override byte GetSortOrder()
		{
			switch (Operator)
			{
				case VariadicOperator.Addition: return 4;
				case VariadicOperator.Multiplication: return 6;
				default: throw new InvalidOperationException();
			}
		}

		protected internal override SymbolicExpression Accept(ExpressionVisitor visitor) => visitor.VisitVariadicOperation(this);

		public SymbolicExpression Update(ImmutableArray<SymbolicExpression> operands)
		{
			if (Operands != operands)
			{
				switch (Operator)
				{
					case VariadicOperator.Addition: return Add(operands);
					case VariadicOperator.Multiplication: return Multiply(operands);
					default: throw new InvalidOperationException();
				}
			}

			return this;
		}

		public override string ToString()
			=> string.Join(OperatorToString[Operator], Operands);

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

		bool IExpression.IsSimpleFraction => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		byte IExpression.GetPrecedence() => Operator.GetPrecedence();
		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetFirstOperand() => Operands[0];
		SymbolicExpression IExpression.GetSecondOperand() => Operands[1];
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => Operands;

		#endregion
	}
}
