using SolidSharp.Expressions;

namespace SolidSharp.Geometries
{
	public abstract class Geometry
	{
		private protected Geometry() { }

		public abstract SymbolicEquationSystem GetInteriorEquation();

		public static Geometry2 Union(Geometry2 a, Geometry2 b) => null;

		public static Geometry3 Union(Geometry3 a, Geometry3 b) => null;

		public static Geometry2 Intersection(Geometry2 a, Geometry2 b) => null;

		public static Geometry3 Intersection(Geometry3 a, Geometry3 b) => null;
	}
}
