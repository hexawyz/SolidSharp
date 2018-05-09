using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SolidSharp
{
	internal static class DecimalExtensions
    {
		// 1:1 mapping of the System.Decimal type, used for accessing internals with no allocations.
		[StructLayout(LayoutKind.Explicit)]
		private struct InternalDecimal
		{
			private const uint SignMask = 0x80000000;
			private const uint ScaleMask = 0x00FF0000;

			[FieldOffset(0)]
			private uint _flags;
			[FieldOffset(4)]
			private uint _hi;
			[FieldOffset(8)]
			private uint _lo;
			[FieldOffset(12)]
			private uint _mid;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsInteger() => (_flags & ScaleMask) == 0;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool IsNegative() => (_flags & SignMask) != 0;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public byte GetScale() => (byte)(_flags >> 16);

			public (long value, byte scale) Decompose()
			{
				bool isNegative = IsNegative();
				var scale = GetScale();
				if (_hi != 0) throw new OverflowException();
				ulong value = (ulong)_mid << 16 | _lo;
				if (value > (isNegative ? (ulong)long.MaxValue + 1 : (ulong)long.MaxValue)) throw new OverflowException();
				return (isNegative ? -unchecked((long)value) : unchecked((long)value), scale);
			}
		}

		/// <summary>Determines if the number is an integer.</summary>
		/// <param name="d">The decimal number.</param>
		/// <returns><see langword="true" /> if the number is an integer; otherwise, <see langword="false" />.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsInteger(this in decimal d)
			=> d.ToInternalRepresentation().IsInteger();

		/// <summary>Determines if the number is an integer.</summary>
		/// <param name="d">The decimal number.</param>
		/// <returns><see langword="true" /> if the number is an integer; otherwise, <see langword="false" />.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static (long value, byte scale) Decompose(this in decimal d)
			=> d.ToInternalRepresentation().Decompose();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ref readonly InternalDecimal ToInternalRepresentation(this in decimal d)
			=> ref Unsafe.As<decimal, InternalDecimal>(ref Unsafe.AsRef(d));
	}
}
