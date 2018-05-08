using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class ConstantExpression : SymbolicExpression, IEquatable<ConstantExpression>, IExpression
    {
		/// <summary>Represents the mathematical constant <c>π</c>.</summary>
		public static ConstantExpression Pi = new ConstantExpression("π", Math.PI);

		/// <summary>Represents the mathematical constant <c>𝑒</c>.</summary>
		public static ConstantExpression E = new ConstantExpression("𝑒", Math.E);

		/// <summary>Gets the name of this constant.</summary>
		public string Name { get; }

		/// <summary>Gets the value of this constant.</summary>
		/// <remarks>That value can be used when using numeric evaluation of the value of an expression.</remarks>
		public double Value { get; }

		public override ExpressionKind Kind => ExpressionKind.Constant;

		private ConstantExpression(string name, double value) => (Name, Value) = (name, value);

		public override bool Equals(object obj) => Equals(obj as ConstantExpression);

		public bool Equals(ConstantExpression other) => ReferenceEquals(this, other) || !(other is null) && Name == other.Name;

		public override int GetHashCode() => EqualityComparer<string>.Default.GetHashCode(Name);

		#region IExpression Helpers

		bool IExpression.IsUnaryOperation => false;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;

		bool IExpression.IsNegation => false;

		bool IExpression.IsAddition => false;
		bool IExpression.IsSubtraction => false;
		bool IExpression.IsMultiplication => false;
		bool IExpression.IsDivision => false;
		bool IExpression.IsPower => false;

		bool IExpression.IsMathematicalFunction => false;

		bool IExpression.IsNumber => false;
		bool IExpression.IsInteger => false;
		bool IExpression.IsPositiveInteger => false;
		bool IExpression.IsNegativeInteger => false;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => true;

		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => throw new NotSupportedException();

		#endregion
	}
}
