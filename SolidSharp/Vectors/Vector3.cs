using SolidSharp.Expressions;
using System;
using System.Collections.Generic;

namespace SolidSharp.Vectors
{
	public readonly struct Vector3 : IEquatable<Vector3>
	{
		public SymbolicExpression X { get; }
		public SymbolicExpression Y { get; }
		public SymbolicExpression Z { get; }

		public Vector3(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z)
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

		public override bool Equals(object obj)
			=> obj is Vector3 other && Equals(other);

		public bool Equals(Vector3 other)
			=> EqualityComparer<SymbolicExpression>.Default.Equals(X, other.X)
			&& EqualityComparer<SymbolicExpression>.Default.Equals(Y, other.Y)
			&& EqualityComparer<SymbolicExpression>.Default.Equals(Z, other.Z);

		public override int GetHashCode()
		{
			var hashCode = -307843816;
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(X);
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(Y);
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(Z);
			return hashCode;
		}

		public static Vector3 operator *(in Vector3 a, in Matrix4x3 b)
			=> new Vector3
			(
				a.X * b.M11 + a.Y * b.M21 + a.Z * b.M31 + b.M41,
				a.X * b.M12 + a.Y * b.M22 + a.Z * b.M32 + b.M42,
				a.X * b.M13 + a.Y * b.M23 + a.Z * b.M33 + b.M43
			);
	}
}
