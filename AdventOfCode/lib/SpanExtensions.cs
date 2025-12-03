using System.Numerics;

namespace AdventOfCode.lib;

internal static class SpanExtensions
{
    extension<T>(ReadOnlySpan<T> span)
    {
        public T Max()
        {
            if (span.Length == 0)
                throw new InvalidOperationException("Sequence contains no elements");

            var max = span[0];

            for (var i = 1; i < span.Length; i++)
            {
                if (Comparer<T>.Default.Compare(span[i], max) > 0)
                {
                    max = span[i];
                }
            }

            return max;
        }
    }
}
