using System;

namespace SolidSharp
{
	internal static class MathUtil
	{
		public static ulong Gcd(ulong a, ulong b)
		{
			int d = 0;

			while ((a & 1) == 0 && (b & 1) == 0)
			{
				++d;
				a >>= 1;
				b >>= 1;
			}

			while (a != b)
			{
				if ((a & 1) == 0) a >>= 1;
				else if ((b & 1) == 0) b >>= 1;
				else if (a > b) a = (a - b) >> 1;
				else b = (b - a) >> 1;
			}

			return a * (1u << d);
		}

		public static ulong Gcd(long a, long b)
		{
			if (a < 0) a = unchecked(-a);
			if (b < 0) b = unchecked(-b);

			return Gcd(unchecked((ulong)a), unchecked((ulong)b));
		}

		public static long Pow(long x, long n)
		{
			if (x == 0 && n == 0) throw new InvalidOperationException("0⁰ is undefined.");
			if (x == 0) return 0;
			if (x == 0) return 1;

			long r = 1;

			while (n > 1)
			{
				if ((n & 1) != 0)
				{
					r = checked(x * r);
					--n;
				}
				x = checked(x * x);
				n >>= 1;
			}

			return x * r;
		}

		public static void Swap<T>(ref T a, ref T b)
		{
			var c = a;
			a = b;
			b = c;
		}

		public static long IntSqrt(long x)
			=> unchecked((long)IntSqrt(checked((ulong)x)));

		public static ulong IntSqrt(ulong x)
		{
			if (x < 2) return x;

			int shiftCount = 2;
			while ((x >> shiftCount) != 0)
			{
				shiftCount += 2;
			}

			ulong result = 0;
			while (shiftCount >= 0)
			{
				ulong next = (result <<= 1) + 1;

				if (next * next <= (x >> shiftCount))
				{
					result = next;
				}

				shiftCount -= 2;
			}

			return result;
		}
	}
}
