using System;
using System.Runtime.InteropServices;

namespace SolidSharp
{
	[StructLayout(LayoutKind.Sequential)]
    public struct FixedLengthArray2<T>
    {
		// Here, we are hoping that the runtime will never decide to randomly swap Item1 and Item2…
		// Which would basically break everything.

		public T Item1;
		public T Item2;

		public ref T this[int index] => ref AsSpan()[index];

		public Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref Item1, 2);
	}
}
