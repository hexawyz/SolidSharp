using SolidSharp.Expressions;
using System;
using System.Collections.Generic;

namespace SolidSharp.Vectors
{
	public readonly struct Vector4 : IEquatable<Vector4>
	{
		public SymbolicExpression X { get; }
		public SymbolicExpression Y { get; }
		public SymbolicExpression Z { get; }
		public SymbolicExpression W { get; }

		public Vector4(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z, SymbolicExpression w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}

		public override bool Equals(object obj)
			=> obj is Vector4 other && Equals(other);

		public bool Equals(Vector4 other)
			=> EqualityComparer<SymbolicExpression>.Default.Equals(X, other.X)
			&& EqualityComparer<SymbolicExpression>.Default.Equals(Y, other.Y)
			&& EqualityComparer<SymbolicExpression>.Default.Equals(Z, other.Z)
			&& EqualityComparer<SymbolicExpression>.Default.Equals(W, other.W);

		public override int GetHashCode()
		{
			var hashCode = 707706286;
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(X);
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(Y);
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(Z);
			hashCode = hashCode * -1521134295 + EqualityComparer<SymbolicExpression>.Default.GetHashCode(W);
			return hashCode;
		}
	}
}
