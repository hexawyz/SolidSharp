using SolidSharp.Expressions;
using SolidSharp.Vectors;

namespace SolidSharp.Geometries
{
	public sealed class SphereGeometry : Geometry3
    {
		public override SymbolicEquationSystem GetInteriorEquation(Vector3 v)
			=> v.Length <= 1;
	}
}
