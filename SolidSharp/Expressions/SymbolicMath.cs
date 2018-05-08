namespace SolidSharp.Expressions
{
	public static class SymbolicMath
	{
		public static ConstantExpression Pi => ConstantExpression.Pi;
		public static ConstantExpression E => ConstantExpression.E;

		public static SymbolicExpression Pow(SymbolicExpression x, SymbolicExpression y)
			=> ExpressionSimplifier.TrySimplifyPower(x, y)
			?? new BinaryOperationExpression(BinaryOperator.Power, x, y);

		public static SymbolicExpression Sin(SymbolicExpression x)
			=> new UnaryOperationExpression(UnaryOperator.Sin, x);

		public static SymbolicExpression Cos(SymbolicExpression x)
			=> new UnaryOperationExpression(UnaryOperator.Cos, x);

		public static SymbolicExpression Ln(SymbolicExpression x)
			=> new UnaryOperationExpression(UnaryOperator.Ln, x);

		public static SymbolicExpression Exp(SymbolicExpression x)
			=> Pow(E, x);

		public static SymbolicExpression Sqrt(SymbolicExpression x)
			=> Pow(x, NumberExpression.Half);
    }
}
