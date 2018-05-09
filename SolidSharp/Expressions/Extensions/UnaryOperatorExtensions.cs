namespace SolidSharp.Expressions.Extensions
{
	public static class UnaryOperatorExtensions
	{
		public static bool IsFunction(this UnaryOperator @operator)
			=> (byte)@operator >= 128;
	}
}
