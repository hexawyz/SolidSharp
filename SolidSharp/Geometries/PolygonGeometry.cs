using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using SolidSharp.Expressions;
using SolidSharp.Vectors;

namespace SolidSharp.Geometries
{
	public sealed class PolygonGeometry : Geometry2
	{
		public ImmutableArray<Vector2> Points { get; }

		public PolygonGeometry(ImmutableArray<Vector2> points)
		{
			Points = points;
		}

		public override SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y)
		{
			throw new NotImplementedException();
		}
	}
}
