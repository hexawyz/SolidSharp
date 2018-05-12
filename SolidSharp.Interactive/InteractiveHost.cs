using SolidSharp.Geometries;
using SolidSharp.Vectors;
using System.Threading;
using E = SolidSharp.Expressions.SymbolicExpression;
using M = SolidSharp.Expressions.SymbolicMath;

#pragma warning disable IDE1006 // Naming styles

namespace SolidSharp.Interactive
{
	public class InteractiveHost
	{
		internal bool IsNewSessionRequested { get; set; }
		internal bool IsExitRequested { get; set; }

		// Predefined variables.
		//public static E x => Parameters.X;
		//public static E y => Parameters.Y;
		//public static E z => Parameters.Z;
		//public static E t => var("𝓉");
		// These names are ASCII and work in the non UTF-16 aware Windows console:
		public static E x => var("x");
		public static E y => var("y");
		public static E z => var("z");
		public static E t => var("t");

		// Useful constants
		public static E e => M.E;
		public static E pi => M.Pi;
		public static E π => M.Pi;

		// Mirror of the SymbolicMath.N method
		public static E n(sbyte value) => M.N(value);
		public static E n(byte value) => M.N(value);
		public static E n(short value) => M.N(value);
		public static E n(ushort value) => M.N(value);
		public static E n(int value) => M.N(value);
		public static E n(uint value) => M.N(value);
		public static E n(long value) => M.N(value);
		public static E n(ulong value) => M.N(value);
		public static E n(float value) => M.N(value);
		public static E n(double value) => M.N(value);
		public static E n(decimal value) => M.N(value);
		public static E var(string name) => M.Var(name);

		// Helper methods for constructing vectors
		public static Vector2 vec(E x, E y) => new Vector2(x, y);
		public static Vector3 vec(E x, E y, E z) => new Vector3(x, y, z);
		public static Vector4 vec(E x, E y, E z, E w) => new Vector4(x, y, z, w);

		// Mathematic functions
		public static E abs(E x) => M.Abs(x);

		public static E exp(E x) => M.Exp(x);
		public static E ln(E x) => M.Ln(x);

		public static E sqrt(E x) => M.Sqrt(x);
		public static E root(E x, E y) => M.Root(x, y);
		public static E pow(E x, E y) => M.Pow(x, y);

		// Trigonometric functions
		public static E sin(E x) => M.Sin(x);
		public static E cos(E x) => M.Cos(x);
		public static E tan(E x) => M.Tan(x);

		// Host methods
		public void reset() => IsNewSessionRequested = true;
		public void exit() => IsExitRequested = true;
	}
}
