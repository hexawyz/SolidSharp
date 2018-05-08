using SolidSharp.Expressions;

namespace SolidSharp.Vectors
{
	public readonly struct Vector4
    {
		public SymbolicExpression X { get; }
		public SymbolicExpression Y { get; }
		public SymbolicExpression Z { get; }
		public SymbolicExpression W { get; }

		public Vector4(SymbolicExpression x, SymbolicExpression y, SymbolicExpression z, SymbolicExpression w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
	}
}
