using SolidSharp.Expressions;

namespace SolidSharp.Geometries
{
	public sealed class ExtrudedGeometry : Geometry3
    {
		public Geometry2 BaseGeometry { get; }
		public Axis Direction { get; }
		
		public ExtrudedGeometry(Geometry2 baseGeometry, Axis direction)
		{
			BaseGeometry = baseGeometry;
			Direction = direction;
		}

		public override SymbolicEquationSystem GetInteriorEquation(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z)
			=> null;
	}
}
