using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class VariableExpression : SymbolicExpression, IExpression, IEquatable<VariableExpression>
	{
		public string Name { get; }

		internal VariableExpression(string name)
		{
			if (name == null) throw new ArgumentNullException(nameof(name));
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException();

			Name = name;
		}

		public override ExpressionKind Kind => ExpressionKind.Variable;

		protected internal override byte GetSortOrder() => 2;

		protected internal override SymbolicExpression Accept(ExpressionVisitor visitor) => visitor.VisitVariable(this);

		public override string ToString() => Name;

		public override bool Equals(object obj) => Equals(obj as VariableExpression);

		public bool Equals(VariableExpression other)
			=> ReferenceEquals(this, other)
			|| !(other is null) && Name == other.Name;

		public override int GetHashCode()
		{
			var hashCode = 890389916;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
			return hashCode;
		}

		#region IExpression Helpers

		bool IExpression.IsOperation => false;
		bool IExpression.IsUnaryOperation => false;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;
		bool IExpression.NeedsParentheses => false;

		bool IExpression.IsNegation => false;
		bool IExpression.IsAbsoluteValue => false;

		bool IExpression.IsAddition => false;
		bool IExpression.IsSubtraction => false;
		bool IExpression.IsMultiplication => false;
		bool IExpression.IsDivision => false;

		bool IExpression.IsPower => false;
		bool IExpression.IsRoot => false;

		bool IExpression.IsMathematicalFunction => false;

		bool IExpression.IsNumber => false;
		bool IExpression.IsPositiveNumber => false;
		bool IExpression.IsNegativeNumber => false;
		bool IExpression.IsOddNumber => false;
		bool IExpression.IsEvenNumber => false;

		bool IExpression.IsVariable => true;
		bool IExpression.IsConstant => false;

		byte IExpression.GetPrecedence() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetFirstOperand() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetSecondOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => throw new NotSupportedException();

		#endregion
	}
}
