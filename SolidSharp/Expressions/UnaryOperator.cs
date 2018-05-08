namespace SolidSharp.Expressions
{
	public enum UnaryOperator : byte
    {
		Plus = 0, // This is actually quite the useless operator… I'm not even sure why I'm including something that will never get any practical use.
		Minus = 1,

		// Mathematical functions which are unary functions.

		Sin = 128,
		Cos = 129,
		Tan = 130,

		Ln = 131,
    }

	public static class UnaryOperatorExtensions
	{
		public static bool IsFunction(this UnaryOperator @operator)
			=> (byte)@operator >= 128;
	}
}
