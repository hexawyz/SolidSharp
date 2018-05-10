using SolidSharp.Expressions;
using System;
using System.Collections.Generic;

namespace SolidSharp.Vectors
{
	public readonly struct Vector2 : IEquatable<Vector2>
	{
		public SymbolicExpression X { get; }
		public SymbolicExpression Y { get; }

		public Vector2(SymbolicExpression x, SymbolicExpression y)
		{
			X = x;
			Y = y;
		}

		public SymbolicExpression Length
			=> SymbolicMath.Sqrt(LengthSquared);

		public SymbolicExpression LengthSquared
			=> Dot(this, this);

		public static SymbolicExpression Dot(in Vector2 a, in Vector2 b)
			=> a.X * b.X + a.Y * b.Y;

		public override bool Equals(object obj)
			=> obj is Vector2 other && Equals(other);

		public bool Equals(Vector2 other)
			=> EqualityComparer<SymbolicExpression>.Default.Equals(X, other.X)
			&& EqualityComparer<SymbolicExpression>.Default.Equals(Y, other.Y);

		public override int GetHashCode()
		{
			var hashCode = 1861411795;
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(X);
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(Y);
			return hashCode;
		}

		public static Vector2 operator *(in Vector2 a, in Matrix3x2 b)
			=> new Vector2
			(
				a.X * b.M11 + a.Y * b.M21 + b.M31,
				a.X * b.M12 + a.Y * b.M22 + b.M32
			);
	}
}
