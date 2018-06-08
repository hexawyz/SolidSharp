using System.Runtime.CompilerServices;

namespace SolidSharp.Expressions
{
	public static class SymbolicMath
	{
		public static SymbolicExpression Pi => ConstantExpression.Pi;
		public static SymbolicExpression E => ConstantExpression.E;
		public static SymbolicExpression I => ConstantExpression.I;

		public static SymbolicExpression HalfPi => Pi / N(2);
		public static SymbolicExpression QuarterPi => Pi / N(4);

		public static SymbolicExpression Zero => NumberExpression.Zero;
		public static SymbolicExpression One => NumberExpression.One;
		public static SymbolicExpression MinusOne => NumberExpression.MinusOne;

		private static readonly SymbolicExpression Two = SymbolicExpression.Constant(2);

		// Provide short syntax for easily creating a numeric constant when automatic conversions don't apply.
		public static SymbolicExpression N(sbyte value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(byte value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(short value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(ushort value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(int value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(uint value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(long value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(ulong value) => SymbolicExpression.Constant(value);

		public static SymbolicExpression N(float value) => SymbolicExpression.Constant(value);
		public static SymbolicExpression N(double value) => SymbolicExpression.Constant(value);

		public static SymbolicExpression N(decimal value) => SymbolicExpression.Constant(value);

		// Short syntax for creating a variable
		public static SymbolicExpression Var(string name) => SymbolicExpression.Variable(name);

		public static SymbolicExpression Abs(SymbolicExpression x)
			=> ExpressionSimplifier.TrySimplifyAbs(x)
			?? new UnaryOperationExpression(UnaryOperator.Abs, x);

		public static SymbolicExpression Pow(SymbolicExpression x, SymbolicExpression y)
			=> ExpressionSimplifier.TrySimplifyPower(x, y)
			?? new BinaryOperationExpression(BinaryOperator.Power, x, y);

		public static SymbolicExpression Root(SymbolicExpression x, SymbolicExpression y)
			=> ExpressionSimplifier.TrySimplifyRoot(x, y)
			?? new BinaryOperationExpression(BinaryOperator.Root, x, y);

		public static SymbolicExpression Sin(SymbolicExpression x)
			=> ExpressionSimplifier.TrySimplifySin(x)
			?? new UnaryOperationExpression(UnaryOperator.Sin, x);

		public static SymbolicExpression Cos(SymbolicExpression x)
			=> ExpressionSimplifier.TrySimplifyCos(x)
			?? new UnaryOperationExpression(UnaryOperator.Cos, x);

		public static SymbolicExpression Tan(SymbolicExpression x)
			=> new UnaryOperationExpression(UnaryOperator.Tan, x);

		public static SymbolicExpression Ln(SymbolicExpression x)
			=> ExpressionSimplifier.TrySimplifyLn(x)
			?? new UnaryOperationExpression(UnaryOperator.Ln, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Exp(SymbolicExpression x)
			=> Pow(E, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Sqrt(SymbolicExpression x)
			=> Root(x, Two);
	}
}
