using SolidSharp.Expressions;

namespace SolidSharp.Geometries
{
	public sealed class DiscGeometry : Geometry2
	{
		public override SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y)
			=> x * x + y * y <= 1;
	}
}
