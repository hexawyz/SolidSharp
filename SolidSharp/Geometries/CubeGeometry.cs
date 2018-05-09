using SolidSharp.Expressions;
using SolidSharp.Vectors;

namespace SolidSharp.Geometries
{
	public sealed class CubeGeometry : Geometry3
	{
		// TODO: Find a more beautiful equation for cubic geometry
		public override SymbolicEquationSystem GetInteriorEquation(Vector3 v)
			=> v.X >= -1
				& v.X <= 1
				& v.Y >= -1
				& v.Y <= 1
				& v.Z >= -1
				& v.Z <= 1;
	}
}
