using System.Runtime.CompilerServices;

namespace SolidSharp.Expressions
{
	public static class SymbolicMath
	{
		public static ConstantExpression Pi => ConstantExpression.Pi;
		public static ConstantExpression E => ConstantExpression.E;

		public static SymbolicExpression Zero => NumberExpression.Zero;
		public static SymbolicExpression One => NumberExpression.One;
		public static SymbolicExpression MinusOne => NumberExpression.MinusOne;

		private static readonly SymbolicExpression Two = SymbolicExpression.Constant(2);

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
			=> new UnaryOperationExpression(UnaryOperator.Sin, x);

		public static SymbolicExpression Cos(SymbolicExpression x)
			=> new UnaryOperationExpression(UnaryOperator.Cos, x);

		public static SymbolicExpression Ln(SymbolicExpression x)
			=> new UnaryOperationExpression(UnaryOperator.Ln, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Exp(SymbolicExpression x)
			=> Pow(E, x);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static SymbolicExpression Sqrt(SymbolicExpression x)
			=> Root(x, Two);
    }
}
