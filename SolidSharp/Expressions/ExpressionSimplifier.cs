using System;
using System.Collections.Immutable;

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
				return SymbolicExpression.Constant(-((NumberExpression)e).Value);
			}
			else if (e.IsSubtraction())
			{
				var op = (BinaryOperationExpression)e;
				return op.SecondOperand - op.FirstOperand;
			}

			return null;
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
					return ((NumberExpression)a).Value + ((NumberExpression)b).Value;
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

			return null;
		}

		public static SymbolicExpression TrySimplifyAddition(ImmutableArray<SymbolicExpression> operands)
		{
			if (operands.IsDefaultOrEmpty || operands.Length < 2) throw new ArgumentException();

			if (operands.Length == 2) TrySimplifyAddition(operands[0], operands[1]);

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
					return ((NumberExpression)a).Value - ((NumberExpression)b).Value;
				}
			}

			if (a.IsSubtraction()) // (x₁ - x₂) - y => x₁ - (x₂ + y)
			{
				var e = (BinaryOperationExpression)a;
				return SymbolicExpression.Subtract(e.FirstOperand, SymbolicExpression.Add(e.SecondOperand, b));
			}

			if (a.IsNegation())
			{
				return SymbolicExpression.Subtract
				(
					b.IsNegation() ?
						b.GetOperand() : // (-x) - (-y) => y - x
						b, // (-x) + y => y - x
					a.GetOperand()
				);
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
				if (a.Equals(b)) // x * x => x ^ 2
				{
					// This is maybe more like normalization than simplification.
					// x * x and x ^ 2 are supposed to be exactly the same thing,
					// but representing it as x ^ 2 might prove more useful later on.
					return SymbolicMath.Pow(a, 2);
				}
				else if (a.IsNumber()) // x * y => eval(x * y)
				{
					return ((NumberExpression)a).Value * ((NumberExpression)b).Value;
				}
			}

			// Power merging
			if (a.IsPower() && b.IsPower())
			{
				var op1 = (BinaryOperationExpression)a;
				var op2 = (BinaryOperationExpression)b;

				if (op1.FirstOperand.Equals(op2.FirstOperand)) // xⁿ⁰ * xⁿ¹ = xⁿ⁰⁺ⁿ¹
				{
					return SymbolicMath.Pow(op1.FirstOperand, op1.SecondOperand + op2.SecondOperand);
				}
			}
			else if (a.IsPower())
			{
			}
			else if (b.IsPower())
			{
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

			return null;
		}

		private static SymbolicExpression TryMergePowerMultiplication(SymbolicExpression e, BinaryOperationExpression power)
		{
			return null;
		}

		public static SymbolicExpression TrySimplifyMultiplication(ImmutableArray<SymbolicExpression> operands)
		{
			return null;
		}

		public static SymbolicExpression TrySimplifyDivision(SymbolicExpression a, SymbolicExpression b)
		{
			if (b.IsZero())
			{
				// It's actually fine if this method throws an exception. You are *NOT* supposed to divide by zero, anyway.
				throw new DivideByZeroException();
			}

			if (b.Equals(NumberExpression.One))
			{
				return a;
			}

			if (a.Kind == b.Kind) // Handle very trivial simplifications
			{
				// TODO: To optimize away x/x in the general case, we need to assert that x is not null… (Assertions are needed !)

				if (a.IsNumber()) // x / y => eval(x / y)
				{
					long v1 = ((NumberExpression)a).Value;
					long v2 = ((NumberExpression)b).Value;

					if (v1 == v2) return NumberExpression.One; // x / x => 1

					long gcd = checked((long)MathUtil.Gcd(v1, v2));

					if (gcd > 1)
					{
						return SymbolicExpression.Constant(v1 / gcd) / SymbolicExpression.Constant(v2 / gcd);
					}
				}
			}

			// Power merging
			if (a.IsPower() && b.IsPower())
			{
				var op1 = (BinaryOperationExpression)a;
				var op2 = (BinaryOperationExpression)b;

				if (op1.FirstOperand.Equals(op2.FirstOperand)) // xⁿ⁰ / xⁿ¹ = xⁿ⁰⁻ⁿ¹
				{
					return SymbolicMath.Pow(op1.FirstOperand, op1.SecondOperand - op2.SecondOperand);
				}
			}
			else if (a.IsPower())
			{
			}
			else if (b.IsPower()) // x / (yⁿ) => x * y⁻ⁿ
			{
				var op2 = (BinaryOperationExpression)b;
				return a * SymbolicMath.Pow(op2.FirstOperand, -op2.SecondOperand);
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

			return null;
		}

		public static SymbolicExpression TrySimplifyPower(SymbolicExpression a, SymbolicExpression b)
		{
			if (b.IsNumber())
			{
				var nb = (NumberExpression)b;

				if (nb.Value == 0) // x⁰ => 1
				{
					// NB: 0⁰ is supposed to be undefined in the general case.
					// Only simplify the expression if we can guarantee that x is non-zero.
					if (a.IsNumber() && !a.IsZero())
					{
						return 1;
					}
				}
				else if (nb.Value == 1) // x¹ => x
				{
					return a;
				}
			}

			return null;
		}
	}
}
