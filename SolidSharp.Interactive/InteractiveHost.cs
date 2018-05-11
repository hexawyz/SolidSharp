using SolidSharp.Geometries;
using E = SolidSharp.Expressions.SymbolicExpression;
using M = SolidSharp.Expressions.SymbolicMath;

#pragma warning disable IDE1006 // Naming styles

namespace SolidSharp.Interactive
{
	public class InteractiveHost
	{
		public static E x => Parameters.X;
		public static E y => Parameters.Y;
		public static E z => Parameters.Z;
		public static E t => M.Var("𝓉");

		public static E Var(string name) => M.Var(name);

		public static E sin(E x) => M.Sin(x);
		public static E cos(E x) => M.Cos(x);
		public static E tan(E x) => M.Tan(x);

		public static E sqrt(E x) => M.Sqrt(x);
		public static E root(E x, E y) => M.Root(x, y);
		public static E pow(E x, E y) => M.Pow(x, y);
	}
}
