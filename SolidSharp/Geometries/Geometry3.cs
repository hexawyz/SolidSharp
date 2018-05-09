using SolidSharp.Expressions;
using SolidSharp.Vectors;
using static SolidSharp.Geometries.Parameters;

namespace SolidSharp.Geometries
{
	public abstract class Geometry3 : Geometry
	{
		public sealed override SymbolicEquationSystem GetInteriorEquation() => GetInteriorEquation(new Vector3(X, Y, Z));

		public abstract SymbolicEquationSystem GetInteriorEquation(Vector3 v);
	}
}
