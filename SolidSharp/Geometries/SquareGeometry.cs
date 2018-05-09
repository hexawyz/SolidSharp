using SolidSharp.Expressions;
using SolidSharp.Vectors;

namespace SolidSharp.Geometries
{
	public sealed class SquareGeometry : Geometry2
	{
		public override SymbolicEquationSystem GetInteriorEquation(Vector2 v)
			=> SymbolicMath.Abs(v.X - v.Y) + SymbolicMath.Abs(v.X + v.Y) <= 2;
	}
}
