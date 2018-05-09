namespace SolidSharp.Expressions.Extensions
{
	public static class BinaryOperatorExtensions
	{
		private readonly static byte[] OperatorPrecedenceData =
		{
			3, // Addition
			3, // Subtraction
			2, // Multiplication
			2, // Division
			1, // Power
			1, // Root
		};

		public static byte GetPrecedence(this BinaryOperator @operator) => OperatorPrecedenceData[(byte)@operator];
	}
}
