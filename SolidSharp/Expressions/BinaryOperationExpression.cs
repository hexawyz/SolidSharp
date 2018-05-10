using SolidSharp.Expressions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class BinaryOperationExpression : SymbolicExpression, IEquatable<BinaryOperationExpression>, IExpression
	{
		private static readonly Dictionary<BinaryOperator, string> OperatorToString = new Dictionary<BinaryOperator, string>
		{
			{ BinaryOperator.Addition, "+" },
			{ BinaryOperator.Subtraction, "-" },
			{ BinaryOperator.Multiplication, "×" },
			{ BinaryOperator.Division, "∕" },
			{ BinaryOperator.Power, "^" },
		};

		public BinaryOperator Operator { get; set; }

		public SymbolicExpression FirstOperand { get; set; }

		public SymbolicExpression SecondOperand { get; set; }

		internal BinaryOperationExpression(BinaryOperator @operator, SymbolicExpression firstOperand, SymbolicExpression secondOperand)
		{
			Operator = @operator;
			FirstOperand = firstOperand ?? throw new ArgumentNullException(nameof(firstOperand));
			SecondOperand = secondOperand ?? throw new ArgumentNullException(nameof(secondOperand));
		}

		public override ExpressionKind Kind => ExpressionKind.BinaryOperation;

		protected internal override byte GetSortOrder()
		{
			switch (Operator)
			{
				case BinaryOperator.Addition: return 4;
				case BinaryOperator.Subtraction: return 5;
				case BinaryOperator.Multiplication: return 6;
				case BinaryOperator.Division: return 7;
				case BinaryOperator.Root: return 8;
				case BinaryOperator.Power: return 9;
				default: throw new InvalidOperationException();
			}
		}

		protected internal override SymbolicExpression Accept(ExpressionVisitor visitor) => visitor.VisitBinaryOperation(this);

		public SymbolicExpression Update(SymbolicExpression firstOperand, SymbolicExpression secondOperand)
		{
			if (!ReferenceEquals(FirstOperand, firstOperand) || !ReferenceEquals(SecondOperand, secondOperand))
			{
				switch (Operator)
				{
					case BinaryOperator.Addition: return Add(firstOperand, secondOperand);
					case BinaryOperator.Subtraction: return Subtract(firstOperand, secondOperand);
					case BinaryOperator.Multiplication: return Multiply(firstOperand, secondOperand);
					case BinaryOperator.Division: return Divide(firstOperand, secondOperand);
					case BinaryOperator.Power: return SymbolicMath.Pow(firstOperand, secondOperand);
					case BinaryOperator.Root: return SymbolicMath.Root(firstOperand, secondOperand);
					default: throw new InvalidOperationException();
				}
			}

			return this;
		}

		public override string ToString()
		{
			byte currentOperatorPrecedence = Operator.GetPrecedence();

			bool firstParenthesesRequired = FirstOperand.NeedsParentheses() && (!FirstOperand.IsOperation() || (FirstOperand.GetPrecedence() > currentOperatorPrecedence));
			bool secondParenthesesRequired = SecondOperand.NeedsParentheses() && (!SecondOperand.IsOperation() || (SecondOperand.GetPrecedence() >= currentOperatorPrecedence));

			if (Operator == BinaryOperator.Root)
			{
				if (SecondOperand.IsNumber())
				{
					switch (SecondOperand.GetValue())
					{
						case 2:
							return firstParenthesesRequired ? "√(" + FirstOperand.ToString() + ")" : "√" + FirstOperand.ToString();
						case 3:
							return firstParenthesesRequired ? "∛(" + FirstOperand.ToString() + ")" : "∛" + FirstOperand.ToString();
						case 4:
							return firstParenthesesRequired ? "∜(" + FirstOperand.ToString() + ")" : "∜" + FirstOperand.ToString();
						default:
							return "root(" + FirstOperand.ToString() + ", " + SecondOperand.ToString() + ")";
					}
				}
			}

			string @operator = OperatorToString[Operator];

			return firstParenthesesRequired ?
					secondParenthesesRequired ?
						"(" + FirstOperand.ToString() + ") " + @operator + " (" + SecondOperand.ToString() + ")" :
						"(" + FirstOperand.ToString() + ") " + @operator + SecondOperand.ToString() :
					secondParenthesesRequired ?
						FirstOperand.ToString() + @operator + " (" + SecondOperand.ToString() + ")" :
						FirstOperand.ToString() + @operator + SecondOperand.ToString();
		}

		public bool Equals(BinaryOperationExpression other)
			=> ReferenceEquals(this, other)
			|| (!(other is null) && Operator == other.Operator && FirstOperand.Equals(other.FirstOperand) && SecondOperand.Equals(other.SecondOperand));

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

		bool IExpression.IsOperation => true;
		bool IExpression.IsUnaryOperation => true;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;
		bool IExpression.NeedsParentheses => true;

		bool IExpression.IsNegation => false;
		bool IExpression.IsAbsoluteValue => false;

		bool IExpression.IsAddition => Operator == BinaryOperator.Addition;
		bool IExpression.IsSubtraction => Operator == BinaryOperator.Subtraction;
		bool IExpression.IsMultiplication => Operator == BinaryOperator.Multiplication;
		bool IExpression.IsDivision => Operator == BinaryOperator.Division;

		bool IExpression.IsPower => Operator == BinaryOperator.Power;
		bool IExpression.IsRoot => Operator == BinaryOperator.Root;

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
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => ImmutableArray.Create(FirstOperand, SecondOperand);
		
		#endregion
	}
}
