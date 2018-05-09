using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SolidSharp.Expressions
{
	/// <summary>An expression that represents a numeric constant value.</summary>
	/// <remarks>Instances of this class can be obtained by calling one of the <see cref="Create"/> methods, or by implicit conversion from standard numeric types.</remarks>
	public sealed class NumberExpression : SymbolicExpression, IEquatable<NumberExpression>, IExpression
    {
		// Cache instances for all integers from -128 to 127 included.
		// This will hopefully help saving a bit of memory.
		private static readonly NumberExpression[] _cachedSmallIntegerValues = CreateSmallIntegerValueCache();
		private static NumberExpression[] CreateSmallIntegerValueCache()
		{
			var values = new NumberExpression[256];

			for (int i = 0; i < 256; i++)
			{
				values[i] = new NumberExpression(unchecked((sbyte)i));
			}

			return values;
		}
		
		internal static NumberExpression Zero => _cachedSmallIntegerValues[0];
		internal static NumberExpression One => _cachedSmallIntegerValues[1];
		internal static NumberExpression MinusOne => _cachedSmallIntegerValues[unchecked((byte)-1)];

		internal static NumberExpression Create(sbyte value) => _cachedSmallIntegerValues[unchecked((byte)value)];

		internal static NumberExpression Create(byte value)
			=> value < 128 ?
				_cachedSmallIntegerValues[value] :
				new NumberExpression(value);

		internal static NumberExpression Create(short value)
			=> value >= -128 && value < 128 ?
				Create(unchecked((sbyte)value)) :
				new NumberExpression(value);

		internal static NumberExpression Create(ushort value)
			=> value < 128 ?
				_cachedSmallIntegerValues[unchecked((byte)value)] :
				new NumberExpression(value);

		internal static NumberExpression Create(int value)
			=> value >= -128 && value < 128 ?
				Create(unchecked((sbyte)value)) :
				new NumberExpression(value);

		internal static NumberExpression Create(uint value)
			=> value < 128 ?
				_cachedSmallIntegerValues[unchecked((byte)value)] :
				new NumberExpression(value);

		internal static NumberExpression Create(long value)
			=> value >= -128 && value < 128 ?
				Create(unchecked((sbyte)value)) :
				new NumberExpression(value);

		internal static NumberExpression Create(ulong value)
			=> value < 128 ?
				_cachedSmallIntegerValues[unchecked((byte)value)] :
				new NumberExpression(checked((long)value));

		public long Value { get; }

		public override ExpressionKind Kind => ExpressionKind.Number;
		
		private NumberExpression(long value) => Value = value;

		public override string ToString()
			=> Value.ToString(CultureInfo.InvariantCulture);

		public bool Equals(NumberExpression other)
			=> ReferenceEquals(this, other)
			|| !(other is null) && Value == other.Value;

		public override bool Equals(object obj) => Equals(obj as NumberExpression);

		public override int GetHashCode() => Value.GetHashCode();

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

		bool IExpression.IsNumber => true;
		bool IExpression.IsPositiveNumber => Value >= 0; // Consider 0 as a positive number for simplicity
		bool IExpression.IsNegativeNumber => Value < 0;
		bool IExpression.IsOddNumber => (Value & 1) != 0;
		bool IExpression.IsEvenNumber => (Value & 1) == 0;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		byte IExpression.GetPrecedence() => throw new NotSupportedException();
		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => throw new NotSupportedException();

		#endregion
	}
}
