using SolidSharp.Expressions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static SolidSharp.Expressions.SymbolicMath;

namespace SolidSharp.Vectors
{
	/// <summary>Represents a matrix 3 rows by 2 columns matrix.</summary>
	/// <remarks>
	/// This 3x2 matrix is used to represent a 3x3 matrix whose third column would always be (0, 0, 1).
	/// The data is stored in column major order, thus using two column <see cref="Vector3"/>.
	/// </remarks>
	public readonly struct Matrix3x2 : IEquatable<Matrix3x2>
	{
		public static readonly Matrix3x2 Identity = new Matrix3x2
		(
			1, 0,
			0, 1,
			0, 0
		);

		public Vector3 Column1 { get; }
		public Vector3 Column2 { get; }

		public SymbolicExpression M11 => Column1.X;
		public SymbolicExpression M21 => Column1.Y;
		public SymbolicExpression M31 => Column1.Z;

		public SymbolicExpression M12 => Column2.X;
		public SymbolicExpression M22 => Column2.Y;
		public SymbolicExpression M32 => Column2.Z;

		public Matrix3x2(Vector3 column1, Vector3 column2)
		{
			Column1 = column1;
			Column2 = column2;
		}

		public Matrix3x2
		(
			SymbolicExpression m11, SymbolicExpression m12,
			SymbolicExpression m21, SymbolicExpression m22,
			SymbolicExpression m31, SymbolicExpression m32
		)
			: this(new Vector3(m11, m21, m31), new Vector3(m12, m22, m32))
		{
		}

		public SymbolicExpression Determinant
			=> M11 * M22 - M21 * M12;

		public Matrix3x2 Invert()
		{
			var det = Determinant;

			return new Matrix3x2
			(
				M22 / det, -M12 / det,
				-M21 / det, M11 / det,
				(M21 * M32 - M22 * M31) / det,
				-(M11 * M32 - M12 * M31) / det
			);
		}

		public static Matrix3x2 Scale(SymbolicExpression s)
			=> new Matrix3x2
			(
				s, 0,
				0, s,
				0, 0
			);

		public static Matrix3x2 Scale(Vector2 s)
			=> new Matrix3x2
			(
				s.X, 0,
				0, s.Y,
				0, 0
			);

		public static Matrix3x2 Scale(SymbolicExpression scaleX, SymbolicExpression scaleY)
			=> new Matrix3x2
			(
				scaleX, 0,
				0, scaleY,
				0, 0
			);

		public static Matrix3x2 Translate(Vector2 t)
			=> new Matrix3x2
			(
				1, 0,
				0, 1,
				t.X, t.Y
			);

		public static Matrix3x2 Translate(SymbolicExpression translateX, SymbolicExpression translateY)
			=> new Matrix3x2
			(
				1, 0,
				0, 1,
				translateX, translateY
			);

		public static Matrix3x2 Rotate(SymbolicExpression angle)
			=> new Matrix3x2
			(
				Cos(angle), Sin(angle),
				-Sin(angle), Cos(angle),
				0, 0
			);

		public override bool Equals(object obj)
			=> obj is Matrix3x2 other && Equals(other);

		public bool Equals(Matrix3x2 other)
			=> EqualityComparer<Vector3>.Default.Equals(Column1, other.Column1)
			&& EqualityComparer<Vector3>.Default.Equals(Column2, other.Column2);

		public override int GetHashCode()
		{
			var hashCode = 681758273;
			hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(Column1);
			hashCode = hashCode * -1521134295 + EqualityComparer<Vector3>.Default.GetHashCode(Column2);
			return hashCode;
		}

		public static Matrix3x2 Multiply(in Matrix3x2 a, in Matrix3x2 b)
			=> new Matrix3x2
			(
				a.M11 * b.M11 + a.M12 * b.M21, a.M11 * b.M12 + a.M12 * b.M22,
				a.M21 * b.M11 + a.M22 * b.M21, a.M21 * b.M12 + a.M22 * b.M22,
				a.M31 * b.M11 + a.M32 * b.M21 + b.M31, a.M31 * b.M12 + a.M32 * b.M22 + b.M32
			);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Matrix3x2 operator *(in Matrix3x2 a, in Matrix3x2 b) => Multiply(a, b);
	}
}
