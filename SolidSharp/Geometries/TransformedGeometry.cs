using SolidSharp.Expressions;
using SolidSharp.Vectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidSharp.Geometries
{
    public sealed class TransformedGeometry3 : Geometry3
    {
		private readonly Matrix4x3 _transformation;
		public ref readonly Matrix4x3 Transformation => ref _transformation;

		public Geometry3 WrappedGeometry { get; }

		public TransformedGeometry3(Matrix4x3 transformation, Geometry3 wrappedGeometry)
		{
			_transformation = transformation;
			WrappedGeometry = wrappedGeometry;
		}

		public override SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z)
		{
			var transformed = new Vector3(x, y, z) * _transformation.Inverse();

			return WrappedGeometry.GetInteriorEquation(transformed.X, transformed.Y, transformed.Z);
		}
	}
}
