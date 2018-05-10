using System.Collections.Generic;
using System.Collections.Immutable;

namespace SolidSharp
{
	internal static class StructuralEqualityComparer
	{
		public static bool Equals<T>(ImmutableArray<T> a, ImmutableArray<T> b)
		{
			if (!a.IsDefault && !b.IsDefault)
			{
				if (a.Length == b.Length)
				{
					for (int i = 0; i < a.Length; i++)
					{
						if (!EqualityComparer<T>.Default.Equals(a[i], b[i]))
						{
							return false;
						}
					}

					return true;
				}
				else
				{
					return false;
				}
			}
			else if (a.IsDefault && b.IsDefault)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool Equals<T>(ImmutableArray<T> a, ImmutableArray<T>.Builder b)
		{
			if (!a.IsDefault && b != null)
			{
				if (a.Length == b.Count)
				{
					for (int i = 0; i < a.Length; i++)
					{
						if (!EqualityComparer<T>.Default.Equals(a[i], b[i]))
						{
							return false;
						}
					}

					return true;
				}
				else
				{
					return false;
				}
			}
			else if (a.IsDefault && b == null)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public static bool Equals<T>(ImmutableArray<T>.Builder a, ImmutableArray<T> b)
			=> Equals(b, a);
	}
}
