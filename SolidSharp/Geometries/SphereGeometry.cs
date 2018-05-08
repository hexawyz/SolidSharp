using SolidSharp.Expressions;

namespace SolidSharp.Geometries
{
	public sealed class SphereGeometry : Geometry3
    {
		public override SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z)
			=> x * x + y * y + z * z <= 1;
	}
}
