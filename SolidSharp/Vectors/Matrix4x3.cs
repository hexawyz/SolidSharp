using SolidSharp.Expressions;
using System;
using static SolidSharp.Expressions.SymbolicMath;

namespace SolidSharp.Vectors
{
	/// <summary>Represents a matrix 4 rows by 3 columns matrix.</summary>
	/// <remarks>
	/// This 4x3 matrix is used to represent a 4x4 matrix whose third column would always be (0, 0, 0, 1).
	/// The data is stored in column major order, thus using three column <see cref="Vector4"/>.
	/// </remarks>
	public readonly struct Matrix4x3
	{
		public Vector4 Column1 { get; }
		public Vector4 Column2 { get; }
		public Vector4 Column3 { get; }

		public SymbolicExpression M11 => Column1.X;
		public SymbolicExpression M21 => Column1.Y;
		public SymbolicExpression M31 => Column1.Z;
		public SymbolicExpression M41 => Column1.W;

		public SymbolicExpression M12 => Column2.X;
		public SymbolicExpression M22 => Column2.Y;
		public SymbolicExpression M32 => Column2.Z;
		public SymbolicExpression M42 => Column2.W;

		public SymbolicExpression M13 => Column3.X;
		public SymbolicExpression M23 => Column3.Y;
		public SymbolicExpression M33 => Column3.Z;
		public SymbolicExpression M43 => Column3.W;

		public Matrix4x3(Vector4 column1, Vector4 column2, Vector4 column3)
		{
			Column1 = column1;
			Column2 = column2;
			Column3 = column3;
		}

		public Matrix4x3
		(
			SymbolicExpression m11, SymbolicExpression m12, SymbolicExpression m13,
			SymbolicExpression m21, SymbolicExpression m22, SymbolicExpression m23,
			SymbolicExpression m31, SymbolicExpression m32, SymbolicExpression m33,
			SymbolicExpression m41, SymbolicExpression m42, SymbolicExpression m43
		)
			: this(new Vector4(m11, m21, m31, m41), new Vector4(m12, m22, m32, m42), new Vector4(m13, m23, m33, m43))
		{
		}

		public static Matrix4x3 Scale(SymbolicExpression s)
			=> new Matrix4x3
			(
				s, 0, 0,
				0, s, 0,
				0, 0, s,
				0, 0, 0
			);

		public static Matrix4x3 Scale(Vector3 s)
			=> new Matrix4x3
			(
				s.X, 0, 0,
				0, s.Y, 0,
				0, 0, s.Z,
				0, 0, 0
			);

		public static Matrix4x3 Scale(SymbolicExpression scaleX, SymbolicExpression scaleY, SymbolicExpression scaleZ)
			=> new Matrix4x3
			(
				scaleX, 0, 0,
				0, scaleY, 0,
				0, 0, scaleZ,
				0, 0, 0
			);

		public static Matrix4x3 Translate(Vector3 t)
			=> new Matrix4x3
			(
				1, 0, 0,
				0, 1, 0,
				0, 0, 1,
				t.X, t.Y, t.Z
			);

		public static Matrix4x3 Translate(SymbolicExpression translateX, SymbolicExpression translateY, SymbolicExpression translateZ)
			=> new Matrix4x3
			(
				1, 0, 0,
				0, 1, 0,
				0, 0, 1,
				translateX, translateY, translateZ
			);

		public static Matrix4x3 RotateX(SymbolicExpression angle)
		{
			var cos = Cos(angle);
			var sin = Sin(angle);

			return new Matrix4x3
			(
				1, 0, 0,
				0, cos, sin,
				0, -sin, cos,
				0, 0, 0
			);
		}

		public static Matrix4x3 RotateY(SymbolicExpression angle)
		{
			var cos = Cos(angle);
			var sin = Sin(angle);

			return new Matrix4x3
			(
				cos, 0, -sin,
				0, 1, 0,
				sin, 0, cos,
				0, 0, 0
			);
		}

		public static Matrix4x3 RotateZ(SymbolicExpression angle)
		{
			var cos = Cos(angle);
			var sin = Sin(angle);

			return new Matrix4x3
			(
				cos, sin, 0,
				-sin, cos, 0,
				0, 0, 1,
				0, 0, 0
			);
		}

		public SymbolicExpression Determinant
			=> M11 * M22 * M33
			 + M12 * M23 * M31
			 + M13 * M21 * M32
			 - M13 * M22 * M31
			 - M12 * M21 * M33
			 - M11 * M23 * M32;

		public Matrix4x3 Invert()
			// It's easy, we just have to get the determinant, and then… 😭
			=> throw new NotImplementedException();
	}
}
