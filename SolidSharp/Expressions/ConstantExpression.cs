using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class ConstantExpression : SymbolicExpression, IEquatable<ConstantExpression>, IExpression
    {
		/// <summary>Represents the mathematical constant <c>π</c>.</summary>
		public static ConstantExpression Pi = new ConstantExpression("π", 3.1415926535897932384626433833m, 1);

		/// <summary>Represents the mathematical constant <c>𝑒</c>.</summary>
		public static ConstantExpression E = new ConstantExpression("𝑒", 2.7182818284590452353602874714m, 2);

		/// <summary>Represents the mathematical constant <c>𝑒</c>.</summary>
		public static ConstantExpression I = new ConstantExpression("𝑖", null, 0);

		/// <summary>Gets the name of this constant.</summary>
		public string Name { get; }

		/// <summary>Gets the value of this constant.</summary>
		/// <remarks>That value can be used when using numeric evaluation of the value of an expression.</remarks>
		public decimal? Value { get; }

		/// <summary>Gets a number indicating the priority of the constant when sortign expressions.</summary>
		/// <remarks>The lower, the highest.</remarks>
		internal byte ConstantSortOrder { get; }

		private ConstantExpression(string name, decimal? value, byte constantSortOrder)
			=> (Name, Value, ConstantSortOrder) = (name, value, constantSortOrder);

		public override ExpressionKind Kind => ExpressionKind.Constant;

		protected internal override byte GetSortOrder() => SymbolicExpressionComparer.Constant;

		protected internal override SymbolicExpression Accept(ExpressionVisitor visitor) => visitor.VisitConstant(this);

		public override string ToString() => Name;

		public override bool Equals(object obj) => Equals(obj as ConstantExpression);

		public bool Equals(ConstantExpression other) => ReferenceEquals(this, other) || !(other is null) && Name == other.Name;

		public override int GetHashCode() => EqualityComparer<string>.Default.GetHashCode(Name);

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
		bool IExpression.IsLn => false;

		bool IExpression.IsMathematicalFunction => false;

		bool IExpression.IsNumber => false;
		bool IExpression.IsPositiveNumber => false;
		bool IExpression.IsNegativeNumber => false;
		bool IExpression.IsOddNumber => false;
		bool IExpression.IsEvenNumber => false;

		bool IExpression.IsSimpleFraction => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => true;

		byte IExpression.GetPrecedence() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetFirstOperand() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetSecondOperand() => throw new NotSupportedException();
		ReadOnlySpan<SymbolicExpression> IExpression.GetOperands() => throw new NotSupportedException();

		#endregion
	}
}
