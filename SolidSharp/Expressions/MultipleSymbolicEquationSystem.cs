using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class MultipleSymbolicEquationSystem : SymbolicEquationSystem
	{
		public ImmutableArray<SymbolicEquationSystem> Equations { get; }

		public sealed override EquationSystemKind Kind { get; }

		public MultipleSymbolicEquationSystem(EquationSystemKind kind, ImmutableArray<SymbolicEquationSystem> equations)
		{
			Kind = ValidateKind(kind);
			Equations = equations;
		}

		public MultipleSymbolicEquationSystem(EquationSystemKind kind, IEnumerable<SymbolicEquationSystem> equations)
		{
			Kind = ValidateKind(kind);
			Equations = equations.ToImmutableArray();
		}

		private static EquationSystemKind ValidateKind(EquationSystemKind kind)
		{
			if (kind != EquationSystemKind.Intersection && kind != EquationSystemKind.Union) throw new ArgumentOutOfRangeException(nameof(kind));

			return kind;
		}
	}
}
