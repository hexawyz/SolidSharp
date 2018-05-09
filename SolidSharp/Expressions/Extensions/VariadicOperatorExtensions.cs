namespace SolidSharp.Expressions.Extensions
{
	public static class VariadicOperatorExtensions
	{
		private readonly static byte[] OperatorPrecedenceData =
		{
			3, // Addition
			2, // Multiplication
		};

		public static byte GetPrecedence(this VariadicOperator @operator) => OperatorPrecedenceData[(byte)@operator];
	}
}
