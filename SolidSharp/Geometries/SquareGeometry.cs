using SolidSharp.Expressions;

namespace SolidSharp.Geometries
{
	public sealed class SquareGeometry : Geometry2
	{
		public override SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y)
			=> x >= -1
				& x <= 1
				& y >= -1
				& y <= 1;
	}
}
