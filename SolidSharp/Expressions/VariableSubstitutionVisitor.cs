using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	internal sealed class VariableSubstitutionVisitor : ExpressionVisitor
    {
		private readonly ImmutableDictionary<string, SymbolicExpression> _substitutions;

		public VariableSubstitutionVisitor(ImmutableDictionary<string, SymbolicExpression> subsitutions)
			=> _substitutions = subsitutions;

		protected internal override SymbolicExpression VisitVariable(VariableExpression expression)
			=> _substitutions.TryGetValue(expression.Name, out var result) ?
				result :
				expression;
	}
}
