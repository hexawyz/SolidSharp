using SolidSharp.Expressions;
using SolidSharp.Vectors;

namespace SolidSharp.Geometries
{
	public sealed class DiscGeometry : Geometry2
	{
		public override SymbolicEquationSystem GetInteriorEquation(Vector2 v)
			=> v.LengthSquared <= 1;
	}
}
