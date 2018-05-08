using SolidSharp.Expressions;

namespace SolidSharp.Vectors
{
	public readonly struct Vector2
    {
		public SymbolicExpression X { get; }
		public SymbolicExpression Y { get; }

		public Vector2(SymbolicExpression x, SymbolicExpression y) : this()
		{
			X = x;
			Y = y;
		}

		public static SymbolicExpression Dot(in Vector2 a, in Vector2 b)
			=> a.X * b.X + a.Y * b.Y;

		public static Vector2 operator *(in Vector2 a, in Matrix3x2 b)
			=> new Vector2
			(
				a.X * b.M11 + a.Y * b.M21 + b.M31,
				a.X * b.M12 + a.Y * b.M22 + b.M32
			);
	}
}
