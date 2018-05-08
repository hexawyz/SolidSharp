using System;
using System.Collections.Immutable;

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

		public static NumberExpression Zero => _cachedSmallIntegerValues[0];
		public static NumberExpression One => _cachedSmallIntegerValues[1];
		public static NumberExpression MinusOne => _cachedSmallIntegerValues[unchecked((byte)-1)];
		
		public static NumberExpression Half => new NumberExpression(0.5m);

		public static NumberExpression Create(sbyte value) => _cachedSmallIntegerValues[unchecked((byte)value)];

		public static NumberExpression Create(byte value)
			=> value < 128 ?
				_cachedSmallIntegerValues[value] :
				new NumberExpression(value);

		public static NumberExpression Create(short value)
			=> value >= -128 && value < 128 ?
				Create(unchecked((sbyte)value)) :
				new NumberExpression(value);

		public static NumberExpression Create(ushort value)
			=> value < 128 ?
				_cachedSmallIntegerValues[unchecked((byte)value)] :
				new NumberExpression(value);

		public static NumberExpression Create(int value)
			=> value >= -128 && value < 128 ?
				Create(unchecked((sbyte)value)) :
				new NumberExpression(value);

		public static NumberExpression Create(uint value)
			=> value < 128 ?
				_cachedSmallIntegerValues[unchecked((byte)value)] :
				new NumberExpression(value);

		public static NumberExpression Create(long value)
			=> value >= -128 && value < 128 ?
				Create(unchecked((sbyte)value)) :
				new NumberExpression(value);

		public static NumberExpression Create(ulong value)
			=> value < 128 ?
				_cachedSmallIntegerValues[unchecked((byte)value)] :
				new NumberExpression(value);

		public static NumberExpression Create(float value)
		{
			if (value >= -128 && value <= 127)
			{
				if ((sbyte)value is sbyte truncatedValue && value == truncatedValue)
				{
					return Create(truncatedValue);
				}
				else if (value == 0.5f)
				{
					return Half;
				}
			}

			return new NumberExpression(value);
		}

		public static NumberExpression Create(double value)
		{
			if (value >= -128 && value <= 127)
			{
				if ((sbyte)value is sbyte truncatedValue && value == truncatedValue)
				{
					return Create(truncatedValue);
				}
				else if (value == 0.5d)
				{
					return Half;
				}
			}

			return new NumberExpression(value);
		}

		public static NumberExpression Create(decimal value)
		{
			if (value.IsInteger())
			{
				if (value >= -128 && value <= 127)
				{
					return Create((sbyte)value);
				}
			}
			else if (value == 0.5m)
			{
				return Half;
			}

			return new NumberExpression(value);
		}

		public decimal Value { get; }
		public bool IsInteger => Value.IsInteger();

		public override ExpressionKind Kind => ExpressionKind.Number;

		private NumberExpression(int value) : this((decimal)value) { }
		private NumberExpression(uint value) : this((decimal)value) { }

		private NumberExpression(long value) : this((decimal)value) { }
		private NumberExpression(ulong value) : this((decimal)value) { }

		private NumberExpression(float value) : this((decimal)value) { }
		private NumberExpression(double value) : this((decimal)value) { }

		private NumberExpression(decimal value) => Value = value;

		public bool Equals(NumberExpression other)
			=> ReferenceEquals(this, other)
			|| !(other is null) && Value == other.Value;

		public override bool Equals(object obj) => Equals(obj as NumberExpression);

		public override int GetHashCode() => Value.GetHashCode();

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

		bool IExpression.IsNumber => true;
		bool IExpression.IsInteger => Value.IsInteger();
		bool IExpression.IsPositiveInteger => Value.IsInteger() && Value > 0;
		bool IExpression.IsNegativeInteger => Value.IsInteger() && Value < 0;

		bool IExpression.IsVariable => false;
		bool IExpression.IsConstant => false;

		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => throw new NotSupportedException();

		#endregion

		public static implicit operator NumberExpression(sbyte value) => Create(value);
		public static implicit operator NumberExpression(byte value) => Create(value);
		public static implicit operator NumberExpression(short value) => Create(value);
		public static implicit operator NumberExpression(ushort value) => Create(value);
		public static implicit operator NumberExpression(int value) => Create(value);
		public static implicit operator NumberExpression(uint value) => Create(value);
		public static implicit operator NumberExpression(long value) => Create(value);
		public static implicit operator NumberExpression(ulong value) => Create(value);
		public static implicit operator NumberExpression(float value) => Create(value);
		public static implicit operator NumberExpression(double value) => Create(value);
		public static implicit operator NumberExpression(decimal value) => Create(value);
	}
}
