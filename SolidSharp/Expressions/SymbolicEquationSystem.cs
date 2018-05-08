using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public abstract class SymbolicEquationSystem
    {
		private protected SymbolicEquationSystem() { }

		public abstract EquationSystemKind Kind { get; }

		public static SymbolicEquationSystem And(SymbolicEquationSystem a, SymbolicEquationSystem b)
			=> a.Kind == b.Kind && a.Kind == EquationSystemKind.Intersection ?
				new MultipleSymbolicEquationSystem(a.Kind, ((MultipleSymbolicEquationSystem)a).Equations.AddRange(((MultipleSymbolicEquationSystem)b).Equations)) :
				new MultipleSymbolicEquationSystem(EquationSystemKind.Intersection, ImmutableArray.Create(a, b));

		public static SymbolicEquationSystem Or(SymbolicEquationSystem a, SymbolicEquationSystem b)
			=> a.Kind == b.Kind && a.Kind == EquationSystemKind.Union ?
				new MultipleSymbolicEquationSystem(a.Kind, ((MultipleSymbolicEquationSystem)a).Equations.AddRange(((MultipleSymbolicEquationSystem)b).Equations)) :
				new MultipleSymbolicEquationSystem(EquationSystemKind.Union, ImmutableArray.Create(a, b));

		public static SymbolicEquationSystem operator &(SymbolicEquationSystem a, SymbolicEquationSystem b) => And(a, b);
		public static SymbolicEquationSystem operator |(SymbolicEquationSystem a, SymbolicEquationSystem b) => Or(a, b);
	}
}
