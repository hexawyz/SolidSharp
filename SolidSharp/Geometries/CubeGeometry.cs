using SolidSharp.Expressions;

namespace SolidSharp.Geometries
{
	public sealed class CubeGeometry : Geometry3
	{
		public override SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z)
			=> x >= -1
				& x <= 1
				& y >= -1
				& y <= 1;
	}
}
