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
    }
}
