using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace SolidSharp.Expressions
{
	/// <summary>Represents an expression used for symbolic calculus.</summary>
	/// <remarks>
	/// <para>
	/// Comparison operators on this type have been overriden for constructing equations
	/// instead of providing a boolean result like regular .NET types do.
	/// This will be fine in the domain of symbolic computations,
	/// but if you want to compare actual <see cref="SymbolicExpression"/> objects,
	/// you should call the <see cref="Equals(object)"/> method.
	/// </para>
	/// <para>
	/// It should be noted that the current implementation of symbolic expressions do
	/// apply automatic simplifications while generating expression objects.
	/// (e.g. x * x will automatically become x ^2, x - x will automatically become 0, …)
	/// </para>
	/// <para>
	/// This is done in order to avoid generating overly complex expression trees in
	/// memory, and thus avoid allocating objects that would be optimized away later.
	/// This has the disatvantage of losing the original expression, but the advantage
	/// of making some optimizations easier to do.
	/// (Mainly because we'll know in advance that there will be no useless expressions
	/// such as <c>0 + (-0) + x - x</c>)
	/// </para>
	/// </remarks>
	public abstract class SymbolicExpression
#if !DEBUG
		: IExpression
#endif
    {
		private protected SymbolicExpression() { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VariableExpression Variable() => new VariableExpression();
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static VariableExpression Variable(string name) => new VariableExpression(name);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(sbyte value) => NumberExpression.Create(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(byte value) => NumberExpression.Create(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(short value) => NumberExpression.Create(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(ushort value) => NumberExpression.Create(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(int value) => NumberExpression.Create(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(uint value) => NumberExpression.Create(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(long value) => NumberExpression.Create(value);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(ulong value) => NumberExpression.Create(value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(float value) => Constant((decimal)value); // Hoping that things will round nicely to decimal…
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Constant(double value) => Constant((decimal)value); // Hoping that things will round nicely to decimal…

		public static SymbolicExpression Constant(decimal value)
		{
			if (value.IsInteger())
			{
				return Constant(checked((long)value));
			}
			else
			{
				(long numerator, byte power) = value.Decompose();

				return Divide(numerator, SymbolicMath.Pow(10, power));
			}
		}

		public static SymbolicExpression Negate(SymbolicExpression e)
			=> ExpressionSimplifier.TrySimplifyNegation(e)
			?? new UnaryOperationExpression(UnaryOperator.Minus, e);

		public static SymbolicExpression Add(SymbolicExpression a, SymbolicExpression b)
			=> ExpressionSimplifier.TrySimplifyAddition(a, b)
			?? new BinaryOperationExpression(BinaryOperator.Addition, a, b);

		public static SymbolicExpression Add(ImmutableArray<SymbolicExpression> operands)
			=> ExpressionSimplifier.TrySimplifyAddition(operands)
			?? new VariadicOperationExpression(VariadicOperator.Addition, operands);

		public static SymbolicExpression Subtract(SymbolicExpression a, SymbolicExpression b)
			=> ExpressionSimplifier.TrySimplifySubtraction(a, b)
			?? new BinaryOperationExpression(BinaryOperator.Subtraction, a, b);

		public static SymbolicExpression Multiply(SymbolicExpression a, SymbolicExpression b)
			=> ExpressionSimplifier.TrySimplifyMultiplication(a, b)
			?? new BinaryOperationExpression(BinaryOperator.Multiplication, a, b);

		public static SymbolicExpression Multiply(ImmutableArray<SymbolicExpression> operands)
			=> ExpressionSimplifier.TrySimplifyMultiplication(operands)
			?? new VariadicOperationExpression(VariadicOperator.Multiplication, operands);

		public static SymbolicExpression Divide(SymbolicExpression a, SymbolicExpression b)
			=> ExpressionSimplifier.TrySimplifyDivision(a, b)
			?? new BinaryOperationExpression(BinaryOperator.Division, a, b);

		/// <summary>Gets the kind of expression represented by this instance.</summary>
		public abstract ExpressionKind Kind { get; }

		public override bool Equals(object obj) => ReferenceEquals(this, obj);
		public override int GetHashCode() => ((int)Kind).GetHashCode();

#region IExpression Helpers

#if !DEBUG // This seems to make VS go crazy while debugging.

		// NB: IExpression must be reimplemented in every class derived from SymbolicExpressions.

		bool IExpression.IsUnaryOperation => false;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;

		bool IExpression.IsNegation => false;

		bool IExpression.IsAddition => throw new NotImplementedException();
		bool IExpression.IsSubtraction => throw new NotImplementedException();
		bool IExpression.IsMultiplication => throw new NotImplementedException();
		bool IExpression.IsDivision => throw new NotImplementedException();
		bool IExpression.IsPower => throw new NotImplementedException();

		bool IExpression.IsMathematicalFunction => throw new NotImplementedException();

		bool IExpression.IsNumber => throw new NotImplementedException();
		bool IExpression.IsPositiveNumber => throw new NotImplementedException();
		bool IExpression.IsNegativeNumber => throw new NotImplementedException();

		bool IExpression.IsVariable => throw new NotImplementedException();
		bool IExpression.IsConstant => false;

		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => throw new NotImplementedException();
#endif

#endregion
		
		public static SymbolicExpression operator +(SymbolicExpression e)
			=> e;

		public static SymbolicExpression operator -(SymbolicExpression e)
			=> Negate(e);

		public static SymbolicExpression operator +(SymbolicExpression a, SymbolicExpression b)
			=> Add(a, b);

		public static SymbolicExpression operator -(SymbolicExpression a, SymbolicExpression b)
			=> Subtract(a, b);

		public static SymbolicExpression operator *(SymbolicExpression a, SymbolicExpression b)
			=> Multiply(a, b);

		public static SymbolicExpression operator /(SymbolicExpression a, SymbolicExpression b)
			=> Divide(a, b);

		public static implicit operator SymbolicExpression(sbyte value) => Constant(value);
		public static implicit operator SymbolicExpression(byte value) => Constant(value);
		public static implicit operator SymbolicExpression(short value) => Constant(value);
		public static implicit operator SymbolicExpression(ushort value) => Constant(value);
		public static implicit operator SymbolicExpression(int value) => Constant(value);
		public static implicit operator SymbolicExpression(uint value) => Constant(value);
		public static implicit operator SymbolicExpression(long value) => Constant(value);
		public static implicit operator SymbolicExpression(ulong value) => Constant(value);
		public static implicit operator SymbolicExpression(decimal value) => Constant(value);

		public static implicit operator SymbolicExpression(float value) => Constant(value);
		public static implicit operator SymbolicExpression(double value) => Constant(value);

		public static SymbolicEquation operator ==(SymbolicExpression a, SymbolicExpression b) => new SymbolicEquation(ComparisonOperator.EqualTo, a, b);
		public static SymbolicEquation operator !=(SymbolicExpression a, SymbolicExpression b) => new SymbolicEquation(ComparisonOperator.NotEqualTo, a, b);
		public static SymbolicEquation operator <(SymbolicExpression a, SymbolicExpression b) => new SymbolicEquation(ComparisonOperator.LessThan, a, b);
		public static SymbolicEquation operator <=(SymbolicExpression a, SymbolicExpression b) => new SymbolicEquation(ComparisonOperator.LessThanOrEqualTo, a, b);
		public static SymbolicEquation operator >(SymbolicExpression a, SymbolicExpression b) => new SymbolicEquation(ComparisonOperator.GreaterThan, a, b);
		public static SymbolicEquation operator >=(SymbolicExpression a, SymbolicExpression b) => new SymbolicEquation(ComparisonOperator.GreaterThanOrEqualTo, a, b);
	}
}
