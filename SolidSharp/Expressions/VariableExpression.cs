using System;
using System.Collections.Immutable;

namespace SolidSharp.Expressions
{
	public sealed class VariableExpression : SymbolicExpression, IExpression
    {
		public string Name { get; }

		public override ExpressionKind Kind => ExpressionKind.Variable;
		
		internal VariableExpression(string name)
			=> Name = name ?? throw new ArgumentNullException(nameof(name));

		public override string ToString() => Name;

		#region IExpression Helpers

		bool IExpression.IsOperation => false;
		bool IExpression.IsUnaryOperation => false;
		bool IExpression.IsBinaryOperation => false;
		bool IExpression.IsVariadicOperation => false;

		bool IExpression.IsNegation => false;

		bool IExpression.IsAddition => false;
		bool IExpression.IsSubtraction => false;
		bool IExpression.IsMultiplication => false;
		bool IExpression.IsDivision => false;

		bool IExpression.IsPower => false;
		bool IExpression.IsRoot => false;

		bool IExpression.IsMathematicalFunction => false;

		bool IExpression.IsNumber => false;
		bool IExpression.IsPositiveNumber => false;
		bool IExpression.IsNegativeNumber => false;
		bool IExpression.IsOddNumber => false;
		bool IExpression.IsEvenNumber => false;

		bool IExpression.IsVariable => true;
		bool IExpression.IsConstant => false;

		SymbolicExpression IExpression.GetOperand() => throw new NotSupportedException();
		ImmutableArray<SymbolicExpression> IExpression.GetOperands() => throw new NotSupportedException();

		#endregion
	}
}
