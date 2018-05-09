using SolidSharp.Expressions;

namespace SolidSharp.Vectors
{
	public readonly struct Vector3
    {
		public SymbolicExpression X { get; }
		public SymbolicExpression Y { get; }
		public SymbolicExpression Z { get; }

		public Vector3(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z) : this()
		{
			X = x;
			Y = y;
			Z = z;
		}

		public SymbolicExpression Length
			=> SymbolicMath.Sqrt(LengthSquared);

		public SymbolicExpression LengthSquared
			=> Dot(this, this);

		public static SymbolicExpression Dot(in Vector3 a, in Vector3 b)
			=> a.X * b.X + a.Y * b.Y + a.Z * b.Z;

		public static Vector3 operator *(in Vector3 a, in Matrix4x3 b)
			=> new Vector3
			(
				a.X * b.M11 + a.Y * b.M21 + a.Z * b.M31 + b.M41,
				a.X * b.M12 + a.Y * b.M22 + a.Z * b.M32 + b.M42,
				a.X * b.M13 + a.Y * b.M23 + a.Z * b.M33 + b.M43
			);
	}
}
