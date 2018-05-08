namespace SolidSharp.Expressions
{
	public enum ExpressionKind : byte
    {
		Variable = 0,
		Constant = 1,
		Number = 2,
		UnaryOperation = 3,
		BinaryOperation = 4,
		CommutativeOperation = 5,
	}
}
