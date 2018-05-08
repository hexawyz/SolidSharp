namespace SolidSharp.Expressions
{
	public enum ComparisonOperator : byte
    {
		// NB: All operators are expected to be paired with their opposite,
		// so as inversing an operator gets as simple as <c>XOR 1</c>.

		EqualTo = 0,
		NotEqualTo = 1,

		LessThan = 2,
		GreaterThanOrEqualTo = 3,

		GreaterThan = 4,
		LessThanOrEqualTo = 5,
	}
}
