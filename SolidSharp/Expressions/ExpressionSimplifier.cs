using SolidSharp.Expressions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using static SolidSharp.Expressions.SymbolicMath;

namespace SolidSharp.Expressions
{
	/// <summary>Provides various methods that can be used to generate simplified expressions.</summary>
	public static class ExpressionSimplifier
	{
		public static SymbolicExpression TrySimplifyNegation(SymbolicExpression e)
		{
			if (e.IsNegation())
			{
				return e.GetOperand();
			}
			else if (e.IsNumber())
			{
				return N(checked(-e.GetValue()));
			}
			else if (e.IsSubtraction())
			{
				var op = (BinaryOperationExpression)e;
				return op.SecondOperand - op.FirstOperand;
			}

			return null;
		}

		// Sorts the two expressions by arbitrary order, (mainly based on the kind of expression or the operator)
		// for easing computations a bit. (e.g. Enforcing that a constant would always come before a power operation)
		// This is to be used only for commutative operations…
		private static bool SortExpressions(ref SymbolicExpression a, ref SymbolicExpression b)
		{
			if (a.GetSortOrder() > b.GetSortOrder())
			{
				MathUtil.Swap(ref a, ref b);
				return true;
			}

			return false;
		}

		// Sorts a list of expression by arbitrary order.
		private static ImmutableArray<SymbolicExpression>.Builder SortExpressions(ImmutableArray<SymbolicExpression> operands)
		{
			// Use LINQ to provide a stable sort.
			var builder = ImmutableArray.CreateBuilder<SymbolicExpression>(operands.Length);
			builder.AddRange(operands.OrderBy(o => o.GetSortOrder()));
			return builder;
		}

		public static SymbolicExpression TrySimplifyAddition(SymbolicExpression a, SymbolicExpression b)
		{
			// Trivial simplifications
			if (a.IsZero()) return b; // 0 + x => x
			else if (b.IsZero()) return a; // x + 0 => x
			else if (a.Kind == b.Kind)
			{
				if (a.Equals(b)) // x + x => 2 * x
				{
					return 2 * a;
				}
				else if (a.IsNumber()) // x + y => eval(x + y)
				{
					return checked(a.GetValue() + b.GetValue());
				}
			}

			// Negations
			if (a.IsNegation() && b.IsNegation()) // (-x) + (-y) => -(x + y)
			{
				return -(a.GetOperand() + b.GetOperand());
			}
			else if (a.IsNegation()) // (-x) + y => y - x
			{
				return b - a.GetOperand();
			}
			else if (b.IsNegation()) // x + (-y) => x - y
			{
				return a - b.GetOperand();
			}

			// Addition merging
			if (a.IsAddition() && b.IsAddition()) // (x₁ + x₂ + … xₙ) + (y₁ + y₂ + … yₙ) => x₁ + x₂ + … xₙ + y₁ + y₂ + … yₙ
			{
				return SymbolicExpression.Add(a.GetOperands().AddRange(b.GetOperands()));
			}
			else if (a.IsAddition()) // (x₁ + x₂ + … xₙ) + y => x₁ + x₂ + … xₙ + y
			{
				return SymbolicExpression.Add(a.GetOperands().Add(b));
			}
			else if (b.IsAddition()) // x + (y₁ + y₂ + … yₙ) => x + y₁ + y₂ + … yₙ
			{
				return SymbolicExpression.Add(b.GetOperands().Insert(0, a));
			}

			if (a.IsSubtraction() && b.IsSubtraction()) // x - y + (z - w) => x + z - (y + w)
			{
				var op1 = (BinaryOperationExpression)a;
				var op2 = (BinaryOperationExpression)b;

				return op1.FirstOperand + op2.FirstOperand - (op1.SecondOperand + op2.SecondOperand);
			}
			else if (a.IsSubtraction()) // x - y + z => x + z - y
			{
				var op1 = (BinaryOperationExpression)a;

				return op1.FirstOperand + b - op1.SecondOperand;
			}
			else if (b.IsAddition()) // x + (y - z) => x + y - z
			{
				var op2 = (BinaryOperationExpression)b;

				return a + op2.FirstOperand - op2.SecondOperand;
			}

			return SortExpressions(ref a, ref b) ?
				new BinaryOperationExpression(BinaryOperator.Addition, a, b) :
				null;
		}

		public static SymbolicExpression TrySimplifyAddition(ImmutableArray<SymbolicExpression> operands)
		{
			if (operands.IsDefaultOrEmpty || operands.Length < 2) throw new ArgumentException();

			if (operands.Length == 2) return operands[0] + operands[1];

			var builder = SortExpressions(operands);

			int i = 1;

			do
			{
				var simplification = TrySimplifyAddition(builder[i - 1], builder[i]);

				if (simplification is null)
				{
					i++;
				}
				else
				{
					builder[i - 1] = simplification;
					builder.RemoveAt(i);
					if (i > 1) i--; // Track back by one item if something did change
				}
			}
			while (i < builder.Count);
			
			if (builder.Count == 1)
			{
				return builder[0];
			}
			else if (builder.Count == 2)
			{
				// Directly create the result, because everything already has been evaluated
				return new BinaryOperationExpression(BinaryOperator.Addition, builder[0], builder[1]);
			}
			else if (!StructuralEqualityComparer.Equals(operands, builder))
			{
				// Directly create the result, because everything already has been evaluated
				return new VariadicOperationExpression(VariadicOperator.Addition, builder.ToImmutableArray());
			}

			return null;
		}

		public static SymbolicExpression TrySimplifySubtraction(SymbolicExpression a, SymbolicExpression b)
		{
			// Trivial simplifications
			if (a.IsZero()) return SymbolicExpression.Negate(b); // 0 - x => -x
			else if (b.IsZero()) return a; // x - 0 => x
			else if (a.Kind == b.Kind)
			{
				if (a.Equals(b)) // x - x => 0
				{
					return NumberExpression.Zero;
				}
				else if (a.IsNumber()) // x - y => eval(x - y)
				{
					return checked(a.GetValue() - b.GetValue());
				}
			}

			if (a.IsSubtraction()) // (x₁ - x₂) - y => x₁ - (x₂ + y)
			{
				var op1 = (BinaryOperationExpression)a;
				return op1.FirstOperand - (op1.SecondOperand + b);
			}

			if (a.IsNegation())
			{
				return
				(
					b.IsNegation() ?
						b.GetOperand() : // (-x) - (-y) => y - x
						b // (-x) + y => y - x
				) - a.GetOperand();
			}
			else if (b.IsNegation()) // x - (-y) => x + y
			{
				return a + b.GetOperand();
			}

			return null;
		}

		public static SymbolicExpression TrySimplifyMultiplication(SymbolicExpression a, SymbolicExpression b)
		{
			// Trivial simplifications
			if (a.IsZero() || b.IsZero()) return 0; // 0 * x = x * 0 = 0
			else if (a.IsOne()) return b;
			else if (a.IsMinusOne()) return -b;
			else if (b.IsOne()) return a;
			else if (b.IsMinusOne()) return -a;
			else if (a.Kind == b.Kind)
			{
				if (a.IsNumber()) // x * y => eval(x * y)
				{
					return checked(a.GetValue() * b.GetValue());
				}
				else if (a.Equals(b)) // x * x => x ^ 2
				{
					// This is maybe more like normalization than simplification.
					// x * x and x ^ 2 are supposed to be exactly the same thing,
					// but representing it as x ^ 2 might prove more useful later on.
					return Pow(a, 2);
				}
			}

			// Power merging
			if (a.IsPower() && b.IsPower())
			{
				var op1 = (BinaryOperationExpression)a;
				var op2 = (BinaryOperationExpression)b;

				if (op1.FirstOperand.Equals(op2.FirstOperand)) // xⁿ⁰ * xⁿ¹ = xⁿ⁰⁺ⁿ¹
				{
					return Pow(op1.FirstOperand, op1.SecondOperand + op2.SecondOperand);
				}
			}
			else if (a.IsPower())
			{
				if (TryMergePowerMultiplication(b, (BinaryOperationExpression)a) is SymbolicExpression result)
				{
					return result;
				}
			}
			else if (b.IsPower())
			{
				if (TryMergePowerMultiplication(a, (BinaryOperationExpression)b) is SymbolicExpression result)
				{
					return result;
				}
			}

			// Division merging
			if (a.IsDivision() && b.IsDivision()) // (x / y) * (z / w) => x * z / (y * w)
			{
				var op1 = (BinaryOperationExpression)a;
				var op2 = (BinaryOperationExpression)b;

				return op1.FirstOperand * op2.FirstOperand / (op1.SecondOperand * op2.SecondOperand);
			}
			else if (a.IsDivision()) // (x / y) * z => (x * z) / y
			{
				var op1 = (BinaryOperationExpression)a;
				return op1.FirstOperand * b / op1.SecondOperand;
			}
			else if (b.IsDivision()) // x * (y / z) => (x * y) / z
			{
				var op2 = (BinaryOperationExpression)b;
				return a * op2.FirstOperand / op2.SecondOperand;
			}

			// Multiplication merging
			if (a.IsMultiplication() && b.IsMultiplication()) // (x₁ * x₂ * … xₙ) * (y₁ * y₂ * … yₙ) => x₁ * x₂ * … xₙ * y₁ * y₂ * … yₙ
			{
				return SymbolicExpression.Multiply(a.GetOperands().AddRange(b.GetOperands()));
			}
			else if (a.IsMultiplication()) // (x₁ * x₂ * … xₙ) * y => x₁ * x₂ * … xₙ * y
			{
				return SymbolicExpression.Multiply(a.GetOperands().Add(b));
			}
			else if (b.IsMultiplication()) // x * (y₁ * y₂ * … yₙ) => x * y₁ * y₂ * … yₙ
			{
				return SymbolicExpression.Multiply(b.GetOperands().Insert(0, a));
			}

			return SortExpressions(ref a, ref b) ?
				new BinaryOperationExpression(BinaryOperator.Multiplication, a, b) :
				null;
		}

		private static SymbolicExpression TryMergePowerMultiplication(SymbolicExpression x, BinaryOperationExpression power)
		{
			if (x.IsNumber() && power.FirstOperand.IsNumber())
			{
				long na = x.GetValue();
				long nb = power.FirstOperand.GetValue();

				int n = 0;

				while (na > 1 && Math.DivRem(na, nb, out long r) is long q && r == 0)
				{
					na = q;
					n++;
				}

				if (n > 0)
				{
					var powerExpression = Pow(power.FirstOperand, n + power.SecondOperand);

					return na > 1 ?
						na * powerExpression :
						powerExpression;
				}
			}

			return null;
		}

		public static SymbolicExpression TrySimplifyMultiplication(ImmutableArray<SymbolicExpression> operands)
		{
			if (operands.IsDefaultOrEmpty || operands.Length < 2) throw new ArgumentException();

			if (operands.Length == 2) return operands[0] * operands[1];
			
			var builder = SortExpressions(operands);

			int i = 1;

			do
			{
				var simplification = TrySimplifyMultiplication(builder[i - 1], builder[i]);

				if (simplification is null)
				{
					i++;
				}
				else
				{
					builder[i - 1] = simplification;
					builder.RemoveAt(i);
					if (i > 1) i--; // Track back by one item if something did change
				}
			}
			while (i < builder.Count);
			
			if (builder.Count == 1)
			{
				return builder[0];
			}
			else if (builder.Count == 2)
			{
				// Directly create the result, because everything already has been evaluated
				return new BinaryOperationExpression(BinaryOperator.Multiplication, builder[0], builder[1]);
			}
			else if (!StructuralEqualityComparer.Equals(operands, builder))
			{
				// Directly create the result, because everything already has been evaluated
				return new VariadicOperationExpression(VariadicOperator.Multiplication, builder.ToImmutableArray());
			}

			return null;
		}

		public static SymbolicExpression TrySimplifyDivision(SymbolicExpression a, SymbolicExpression b)
		{
			if (b.IsZero())
			{
				// It's actually fine if this method throws an exception. You are *NOT* supposed to divide by zero, anyway.
				throw new DivideByZeroException();
			}

			if (b.Equals(One))
			{
				return a;
			}

			if (a.Kind == b.Kind) // Handle very trivial simplifications
			{
				if (a.IsConstant() && a.Equals(b))
				{
					return One;
				}

				// TODO: To optimize away x/x in the general case, we need to assert that x is not null… (Assertions are needed !)

				// Try to simplify numeric fractions
				if (a.IsNumber() && TrySimplifyFraction(a.GetValue(), b.GetValue()) is SymbolicExpression result)
				{
					return result;
				}
	
			}
			else if (a.IsZero() && b.IsConstant()) // y ≠ 0, x / y => 0
			{
				return Zero;
			}

			// Power merging
			if (a.IsPower() && b.IsPower())
			{
				var op1 = (BinaryOperationExpression)a;
				var op2 = (BinaryOperationExpression)b;

				if (op1.FirstOperand.Equals(op2.FirstOperand)) // xⁿ⁰ / xⁿ¹ = xⁿ⁰⁻ⁿ¹
				{
					return Pow(op1.FirstOperand, op1.SecondOperand - op2.SecondOperand);
				}
			}
			else if (a.IsPower())
			{
				// TODO: Simplify power divisions
			}
			else if (b.IsPower()) // x / (yⁿ) => x * y⁻ⁿ
			{
				var op2 = (BinaryOperationExpression)b;

				if (a.IsNumber() && op2.FirstOperand.IsNumber() && op2.SecondOperand.IsNumber())
				{
					try
					{
						long value = MathUtil.Pow(op2.FirstOperand.GetValue(), op2.SecondOperand.GetValue());
						if (TrySimplifyFraction(a.GetValue(), value) is SymbolicExpression result) return result;
					}
					catch (OverflowException) // It is expected, that sometimes, power will be too big.
					{
					}
				}

				return a * Pow(op2.FirstOperand, -op2.SecondOperand);
			}

			// Division merging
			if (a.IsDivision() && b.IsDivision()) // (x / y) / (z / w) => (x * w) / (y * z)
			{
				var op1 = (BinaryOperationExpression)a;
				var op2 = (BinaryOperationExpression)b;

				return op1.FirstOperand * op2.SecondOperand / (op1.SecondOperand * op2.FirstOperand);
			}
			else if (a.IsDivision()) // (x / y) / z => x / (y * z)
			{
				var op1 = (BinaryOperationExpression)a;
				return op1.FirstOperand / (op1.SecondOperand * b);
			}
			else if (b.IsDivision()) // x / (y / z) => (x * z) / y
			{
				var op2 = (BinaryOperationExpression)b;
				return a * op2.SecondOperand / op2.FirstOperand;
			}

			// Try propagate the division inside the multiplication (That can simplify some expressions)
			if (a.IsMultiplication())
			{
				if (a.IsBinaryOperation())
				{
					var op1 = (BinaryOperationExpression)a;

					var da = TrySimplifyDivision(op1.FirstOperand, b);
					var db = TrySimplifyDivision(op1.SecondOperand, b);

					if (!(da is null && db is null))
					{
						return (da ?? op1.FirstOperand) * (db ?? op1.SecondOperand);
					}
				}
				else
				{
					var operands = a.GetOperands().ToBuilder();
					bool mutated = false;

					for (int i = 0; i < operands.Count; i++)
					{
						var d = TrySimplifyDivision(operands[i], b);

						if (!(d is null))
						{
							operands[i] = d;
							mutated = true;
						}
					}

					if (mutated)
					{
						return SymbolicExpression.Multiply(operands.ToImmutableArray());
					}
				}
			}

			return null;
		}

		private static SymbolicExpression TrySimplifyFraction(long v1, long v2)
		{
			if (v1 == 0) return NumberExpression.Zero; // 0 / x => 0
			if (v1 == v2) return NumberExpression.One; // x / x => 1

			long gcd = checked((long)MathUtil.Gcd(v1, v2));

			if (gcd > 1)
			{
				return N(v1 / gcd) / N(v2 / gcd);
			}

			return null;
		}

		public static SymbolicExpression TrySimplifyPower(SymbolicExpression a, SymbolicExpression b)
		{
			if (b.IsNumber())
			{
				var nb = b.GetValue();

				if (nb == 0) // x⁰ => 1
				{
					// NB: 0⁰ is supposed to be undefined in the general case.
					// Only simplify the expression if we can guarantee that x is non-zero.
					if (a.IsNumber() && !a.IsZero())
					{
						return 1;
					}
				}
				else if (nb == 1) // x¹ => x
				{
					return a;
				}
				else if (a.IsAbsoluteValue() && (nb & 1) == 0) // |x|²ⁿ => x²ⁿ
				{
					return Pow(a.GetOperand(), b);
				}
				else if (a.IsNumber())
				{
					var na = a.GetValue();

					try { return MathUtil.Pow(na, nb); }
					catch (OverflowException) { }
				}
				else if (a.IsRoot())
				{
					var op1 = (BinaryOperationExpression)a;

					if (op1.SecondOperand.IsNumber())
					{
						var na = op1.SecondOperand.GetValue();

						if (na == nb)
						{
							return op1.FirstOperand;
						}
					}
				}
			}

			if (a.IsPower()) // (xⁿ⁰)ⁿ¹ => xⁿ⁰ⁿ¹
			{
				var op1 = (BinaryOperationExpression)a;

				return Pow(op1.FirstOperand, op1.SecondOperand * b);
			}

			return null;
		}

		public static SymbolicExpression TrySimplifyRoot(SymbolicExpression a, SymbolicExpression b)
		{
			if (b.IsNumber())
			{
				var nb = b.GetValue();

				if (nb == 1) // x¹ => x
				{
					return a;
				}
				else if (a.IsPower())
				{
					var op1 = (BinaryOperationExpression)a;

					if (op1.SecondOperand.IsNumber())
					{
						var na = op1.SecondOperand.GetValue();

						if (na == nb)
						{
							if ((na & 1) != 0) // if y is odd: (x^y)^(1/y) => x
							{
								return op1.FirstOperand;
							}
							else // if y is even: (x^y)^(1/y) => |x|
							{
								return Abs(op1.FirstOperand);
							}
						}
					}
				}
			}

			if (a.IsRoot()) // (xⁿ⁰)ⁿ¹ => xⁿ⁰ⁿ¹
			{
				var op1 = (BinaryOperationExpression)a;

				return Root(op1.FirstOperand, op1.SecondOperand * b);
			}

			return null;
		}

		public static SymbolicExpression TrySimplifyAbs(SymbolicExpression x)
		{
			if (x.IsNumber())
			{
				return x.IsNegativeNumber() ?
					-x :
					x;
			}

			if (x.IsPower()) // |x²ⁿ| => x²ⁿ
			{
				var op = (BinaryOperationExpression)x;

				if (op.SecondOperand.IsEvenNumber())
				{
					return x;
				}
			}

			return null;
		}

		public static SymbolicExpression TrySimplifySin(SymbolicExpression x)
		{
			if (x.IsZero() || x.Equals(Pi)) return Zero;

			return null;
		}
	}
}
