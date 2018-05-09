namespace SolidSharp.Expressions
{
	public enum UnaryOperator : byte
    {
		Plus = 0, // This is actually quite the useless operator… I'm not even sure why I'm including something that will never get any practical use.
		Minus = 1,

		// Mathematical functions which are unary functions.

		Abs = 128,

		Sin = 129,
		Cos = 130,
		Tan = 131,

		Ln = 132,
    }

	public static class UnaryOperatorExtensions
	{
		public static bool IsFunction(this UnaryOperator @operator)
			=> (byte)@operator >= 128;
	}
}
