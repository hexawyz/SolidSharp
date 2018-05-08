using SolidSharp.Expressions;
using static SolidSharp.Geometries.Parameters;

namespace SolidSharp.Geometries
{
	public abstract class Geometry3 : Geometry
	{
		public sealed override SymbolicEquationSystem GetInteriorEquation() => GetInteriorEquation(X, Y, Z);

		public abstract SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z);
	}
}
