using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace SolidSharp.Expressions
{
	public abstract class ExpressionVisitor
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual SymbolicExpression Visit(SymbolicExpression expression)
			=> expression?.Accept(this);

		public ImmutableArray<SymbolicExpression> Visit(ImmutableArray<SymbolicExpression> expressions)
		{
			if (expressions.IsDefault) throw new ArgumentException();

			for (int i = 0; i < expressions.Length; i++)
			{
				var expression = expressions[i];
				var visitedExpression = Visit(expression);

				if (!ReferenceEquals(visitedExpression, expression))
				{
					var builder = ImmutableArray.CreateBuilder<SymbolicExpression>(expressions.Length);

					builder.AddRange(expressions, i);

					builder[i] = visitedExpression;

					for (i++; i < expressions.Length; i++)
					{
						builder[i] = Visit(expressions[i]);
					}

					return builder.MoveToImmutable();
				}
			}

			return expressions;
		}

		protected internal virtual SymbolicExpression VisitUnaryOperation(UnaryOperationExpression expression)
			=> expression.Update(Visit(expression.Operand));

		protected internal virtual SymbolicExpression VisitBinaryOperation(BinaryOperationExpression expression)
			=> expression.Update(Visit(expression.FirstOperand), Visit(expression.SecondOperand));

		protected internal virtual SymbolicExpression VisitVariadicOperation(VariadicOperationExpression expression)
			=> expression.Update(Visit(expression.Operands));

		protected internal virtual SymbolicExpression VisitVariable(VariableExpression expression)
			=> expression;

		protected internal virtual SymbolicExpression VisitConstant(ConstantExpression expression)
			=> expression;

		protected internal virtual SymbolicExpression VisitNumber(NumberExpression expression)
			=> expression;
	}
}
