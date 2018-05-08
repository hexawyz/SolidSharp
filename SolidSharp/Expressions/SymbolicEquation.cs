namespace SolidSharp.Expressions
{
	public sealed class SymbolicEquation : SymbolicEquationSystem
    {
		public ComparisonOperator Operator { get; }
		public SymbolicExpression FirstExpression { get; }
		public SymbolicExpression SecondExpression { get; }

		public override EquationSystemKind Kind => EquationSystemKind.Identity;

		public SymbolicEquation(ComparisonOperator @operator, SymbolicExpression firstExpression, SymbolicExpression secondExpression)
		{
			Operator = @operator;
			FirstExpression = firstExpression;
			SecondExpression = secondExpression;
		}

		public static SymbolicEquation operator !(SymbolicEquation e)
			=> new SymbolicEquation((ComparisonOperator)((int)e.Operator ^ 1), e.FirstExpression, e.SecondExpression);
	}
}
