using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace SolidSharp.Expressions.Extensions
{
	internal static class ReadOnlySpanExtensions
	{
		// Warning: These extensions are relying on implementation details of ImmutableArray<T>.
		// While the code itself is unsafe, the result can still be considered safe, though not officially endorsed by Microsoft.

		#region ImmutableArray<T> Related Methods

		private static ref ImmutableArray<T> AsImmutableArray<T>(ref T[] array)
			=> ref Unsafe.As<T[], ImmutableArray<T>>(ref array);

		public static ImmutableArray<T> ToImmutableArray<T>(this ReadOnlySpan<T> items)
		{
			T[] array = items.ToArray();

			return AsImmutableArray(ref array);
		}

		public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ImmutableArray<T> array)
			=> !array.IsDefault ?
				Unsafe.As<ImmutableArray<T>, T[]>(ref array) :
				ReadOnlySpan<T>.Empty;

		public static ImmutableArray<T> AddRange<T>(this ReadOnlySpan<T> a, ReadOnlySpan<T> b)
		{
			T[] array = new T[a.Length + b.Length];

			a.CopyTo(array);
			b.CopyTo(array.AsSpan(a.Length));

			return AsImmutableArray(ref array);
		}

		public static ImmutableArray<T> Add<T>(this ReadOnlySpan<T> items, T item)
		{
			T[] array = new T[items.Length + 1];

			items.CopyTo(array);
			array[items.Length] = item;

			return AsImmutableArray(ref array);
		}

		public static ImmutableArray<T> Insert<T>(this ReadOnlySpan<T> items, int index, T item)
		{
			T[] array = new T[items.Length + 1];

			items.Slice(0, index).CopyTo(array);
			array[index] = item;
			items.Slice(index).CopyTo(array.AsSpan(index + 1));

			return AsImmutableArray(ref array);
		}

		public static ImmutableArray<T> RemoveAt<T>(this ReadOnlySpan<T> items, int index)
		{
			T[] array = new T[items.Length - 1];

			items.Slice(0, index).CopyTo(array);
			items.Slice(index + 1).CopyTo(array.AsSpan(index));

			return AsImmutableArray(ref array);
		}

		public static ImmutableArray<TResult> Map<TSource, TResult>(this ReadOnlySpan<TSource> items, Func<TSource, TResult> selector)
		{
			TResult[] array = new TResult[items.Length];

			for (int i = 0; i < array.Length; i++)
			{
				array[i] = selector(items[i]);
			}

			return AsImmutableArray(ref array);
		}

		#endregion

		#region ImmutableArray<T>.Builder Related Methods

		public static ImmutableArray<T>.Builder ToImmutableArrayBuilder<T>(this ReadOnlySpan<T> items)
		{
			var builder = ImmutableArray.CreateBuilder<T>(items.Length);

			// I don' think it is *safely* posible to rely on ReadOnlySpan<T>.CopyTo there,
			// so this will be slower than ImmutableArray().ToBuilder()…
			for (int i = 0; i < items.Length; i++)
			{
				builder.Add(items[i]);
			}

			return builder;
		}

		public static void AddRange<T>(this ImmutableArray<T>.Builder builder, ReadOnlySpan<T> items)
		{
			int requiredCapacity = builder.Capacity + items.Length;

			if (requiredCapacity < builder.Capacity)
			{
				builder.Capacity = checked(builder.Capacity * 2);
			}

			for (int i = 0; i < items.Length; i++)
			{
				builder.Add(items[i]);
			}
		}

		#endregion
	}
}
