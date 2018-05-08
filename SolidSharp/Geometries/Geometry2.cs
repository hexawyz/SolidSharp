using SolidSharp.Expressions;
using static SolidSharp.Geometries.Parameters;

namespace SolidSharp.Geometries
{
	public abstract class Geometry2 : Geometry
    {
		public sealed override SymbolicEquationSystem GetInteriorEquation() => GetInteriorEquation(X, Y);

		public abstract SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y);
	}
}
