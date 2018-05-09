using SolidSharp.Expressions;
using SolidSharp.Vectors;
using static SolidSharp.Geometries.Parameters;

namespace SolidSharp.Geometries
{
	public abstract class Geometry2 : Geometry
    {
		public sealed override SymbolicEquationSystem GetInteriorEquation() => GetInteriorEquation(new Vector2(X, Y));

		public abstract SymbolicEquationSystem GetInteriorEquation(Vector2 v);
	}
}
